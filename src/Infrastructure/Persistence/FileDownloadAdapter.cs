using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using Colorado.BusinessEntityTransactionHistory.Application.Abstractions;
using Colorado.BusinessEntityTransactionHistory.Application.Configuration;
using Colorado.BusinessEntityTransactionHistory.Application.Downloads;
using Microsoft.Extensions.Options;

namespace Colorado.BusinessEntityTransactionHistory.Infrastructure.Persistence;

public sealed class FileDownloadAdapter : IFileDownloadPort
{
    private const string DownloadQuery = "SELECT transactionid, entityid, name, historydes, receiveddate, comment, effectivedate ORDER BY receiveddate DESC, transactionid DESC";

    private readonly HttpClient _httpClient;
    private readonly IOptions<SocrataOptions> _socrataOptions;

    public FileDownloadAdapter(HttpClient httpClient, IOptions<SocrataOptions> socrataOptions)
    {
        _httpClient = httpClient;
        _socrataOptions = socrataOptions;
    }

    public Task<DownloadTarget> GetTargetAsync(DownloadRequest request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var fullPath = request.GetOutputPath();
        var exists = File.Exists(fullPath);
        var isWritable = !exists && CanWriteToDirectory(request.TargetDirectory);

        return Task.FromResult(new DownloadTarget(fullPath, exists, isWritable));
    }

    public async Task<DownloadedFile> DownloadAsync(
        DownloadRequest request,
        IProgress<DownloadProgressUpdate>? progress,
        CancellationToken cancellationToken)
    {
        var fullPath = request.GetOutputPath();

        if (File.Exists(fullPath))
        {
            throw new InvalidOperationException($"Download target already exists: {fullPath}");
        }

        Directory.CreateDirectory(request.TargetDirectory);
        DeleteStaleTempFiles(request.TargetDirectory, request.TargetFileName);
        var tempPath = Path.Combine(request.TargetDirectory, $".{request.TargetFileName}.{Guid.NewGuid():N}.tmp");

        try
        {
            await DownloadToFileAsync(tempPath, progress, cancellationToken);
            await ValidateDownloadedPayloadAsync(tempPath, cancellationToken);
            File.Move(tempPath, fullPath, overwrite: false);
        }
        catch (OperationCanceledException)
        {
            DeleteIfExists(tempPath);
            throw;
        }
        catch (UnauthorizedAccessException)
        {
            DeleteIfExists(tempPath);
            throw new InvalidOperationException($"Download target cannot be written: {fullPath}");
        }
        catch (IOException)
        {
            DeleteIfExists(tempPath);
            throw new InvalidOperationException($"Download target cannot be written: {fullPath}");
        }

        var fileInfo = new FileInfo(fullPath);
        return new DownloadedFile(fileInfo.FullName, fileInfo.Length, fileInfo.LastWriteTimeUtc);
    }

    private async Task DownloadToFileAsync(
        string tempPath,
        IProgress<DownloadProgressUpdate>? progress,
        CancellationToken cancellationToken)
    {
        if (!string.IsNullOrWhiteSpace(_socrataOptions.Value.MockResponsePath))
        {
            await using var sourceStream = new FileStream(
                _socrataOptions.Value.MockResponsePath,
                FileMode.Open,
                FileAccess.Read,
                FileShare.Read,
                bufferSize: 81920,
                useAsync: true);
            await using var destinationStream = new FileStream(
                tempPath,
                FileMode.CreateNew,
                FileAccess.Write,
                FileShare.None,
                bufferSize: 81920,
                useAsync: true);
            await CopyToAsync(sourceStream, destinationStream, sourceStream.Length, progress, cancellationToken);
            return;
        }

        if (string.IsNullOrWhiteSpace(_socrataOptions.Value.AppToken))
        {
            throw new InvalidOperationException("Unable to start download because Socrata:AppToken is missing.");
        }

        using var request = new HttpRequestMessage(HttpMethod.Post, _socrataOptions.Value.BaseUrl);
        request.Headers.Add("X-App-Token", _socrataOptions.Value.AppToken);
        request.Content = JsonContent.Create(new
        {
            query = DownloadQuery,
            includeSynthetic = false,
        });

        using var response = await _httpClient.SendAsync(
            request,
            HttpCompletionOption.ResponseHeadersRead,
            cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync(cancellationToken);
            throw new InvalidOperationException(GetErrorMessage(errorContent, response.ReasonPhrase));
        }

        await using var responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
        await using var fileStream = new FileStream(
            tempPath,
            FileMode.CreateNew,
            FileAccess.Write,
            FileShare.None,
            bufferSize: 81920,
            useAsync: true);

        await CopyToAsync(
            responseStream,
            fileStream,
            GetKnownContentLength(response),
            progress,
            cancellationToken);
    }

    private static async Task CopyToAsync(
        Stream sourceStream,
        Stream destinationStream,
        long? totalBytes,
        IProgress<DownloadProgressUpdate>? progress,
        CancellationToken cancellationToken)
    {
        var buffer = new byte[81920];
        long bytesTransferred = 0;
        ReportProgress(progress, bytesTransferred, totalBytes);

        while (true)
        {
            var bytesRead = await sourceStream.ReadAsync(buffer, cancellationToken);
            if (bytesRead == 0)
            {
                break;
            }

            await destinationStream.WriteAsync(buffer.AsMemory(0, bytesRead), cancellationToken);
            bytesTransferred += bytesRead;
            ReportProgress(progress, bytesTransferred, totalBytes);
        }

        await destinationStream.FlushAsync(cancellationToken);
    }

    private static long? GetKnownContentLength(HttpResponseMessage response)
    {
        var contentLength = response.Content.Headers.ContentLength;
        return contentLength is > 0 ? contentLength : null;
    }

    private static void ReportProgress(IProgress<DownloadProgressUpdate>? progress, long bytesTransferred, long? totalBytes)
    {
        progress?.Report(new DownloadProgressUpdate(bytesTransferred, totalBytes));
    }

    private static async Task ValidateDownloadedPayloadAsync(string tempPath, CancellationToken cancellationToken)
    {
        var fileInfo = new FileInfo(tempPath);
        if (!fileInfo.Exists || fileInfo.Length == 0)
        {
            DeleteIfExists(tempPath);
            throw new InvalidOperationException("Download failed because the remote source returned an empty payload.");
        }

        try
        {
            await using var fileStream = new FileStream(
                tempPath,
                FileMode.Open,
                FileAccess.Read,
                FileShare.Read,
                bufferSize: 81920,
                useAsync: true);
            using var _ = await JsonDocument.ParseAsync(fileStream, cancellationToken: cancellationToken);
        }
        catch (JsonException)
        {
            DeleteIfExists(tempPath);
            throw new InvalidOperationException("Download failed because the remote source returned malformed JSON.");
        }
    }

    private static string GetErrorMessage(string content, string? reasonPhrase)
    {
        try
        {
            using var document = JsonDocument.Parse(content);
            if (document.RootElement.ValueKind == JsonValueKind.Object &&
                document.RootElement.TryGetProperty("message", out var messageElement))
            {
                return $"Remote request failed: {messageElement.GetString()}";
            }
        }
        catch (JsonException)
        {
        }

        return $"Remote request failed: {reasonPhrase ?? "Unknown error"}";
    }

    private static bool CanWriteToDirectory(string directory)
    {
        try
        {
            Directory.CreateDirectory(directory);
            var probePath = Path.Combine(directory, $".write-test-{Guid.NewGuid():N}.tmp");
            File.WriteAllText(probePath, "probe");
            File.Delete(probePath);
            return true;
        }
        catch (UnauthorizedAccessException)
        {
            return false;
        }
        catch (IOException)
        {
            return false;
        }
    }

    private static void DeleteStaleTempFiles(string directory, string targetFileName)
    {
        try
        {
            if (!Directory.Exists(directory))
            {
                return;
            }

            foreach (var tempFile in Directory.EnumerateFiles(directory, GetTempFilePattern(targetFileName)))
            {
                DeleteIfExists(tempFile);
            }
        }
        catch (IOException)
        {
        }
        catch (UnauthorizedAccessException)
        {
        }
    }

    private static string GetTempFilePattern(string targetFileName)
    {
        return $".{targetFileName}.*.tmp";
    }

    private static void DeleteIfExists(string path)
    {
        try
        {
            if (!string.IsNullOrWhiteSpace(path) && File.Exists(path))
            {
                File.Delete(path);
            }
        }
        catch (IOException)
        {
        }
        catch (UnauthorizedAccessException)
        {
        }
    }
}