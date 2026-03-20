using Colorado.BusinessEntityTransactionHistory.Application.Configuration;
using Colorado.BusinessEntityTransactionHistory.Application.Downloads;
using Colorado.BusinessEntityTransactionHistory.Infrastructure.Persistence;
using Microsoft.Extensions.Options;

namespace Colorado.BusinessEntityTransactionHistory.Infrastructure.IntegrationTests.Persistence;

public sealed class FileDownloadAdapterFailureTests
{
    [Fact]
    public async Task Get_target_async_reports_a_non_writable_directory_when_the_target_directory_is_a_file()
    {
        var workingDirectory = Path.Combine(Path.GetTempPath(), $"download-adapter-not-writable-{Guid.NewGuid():N}");
        Directory.CreateDirectory(workingDirectory);

        var blockedPath = Path.Combine(workingDirectory, "blocked");
        await File.WriteAllTextAsync(blockedPath, "not-a-directory");

        var adapter = new FileDownloadAdapter(new HttpClient(), Options.Create(new SocrataOptions()));
        var target = await adapter.GetTargetAsync(new DownloadRequest(true, blockedPath, "casm-dbbj-query.json"), CancellationToken.None);

        Assert.False(target.Exists);
        Assert.False(target.IsWritable);
    }

    [Fact]
    public async Task Download_async_rejects_a_malformed_payload()
    {
        var workingDirectory = Path.Combine(Path.GetTempPath(), $"download-adapter-malformed-{Guid.NewGuid():N}");
        Directory.CreateDirectory(workingDirectory);

        var mockResponsePath = Path.Combine(workingDirectory, "mock-response.json");
        await File.WriteAllTextAsync(mockResponsePath, "not-json");

        var adapter = new FileDownloadAdapter(
            new HttpClient(),
            Options.Create(new SocrataOptions
            {
                AppToken = Guid.NewGuid().ToString("N"),
                MockResponsePath = mockResponsePath,
            }));

        var request = new DownloadRequest(true, Path.Combine(workingDirectory, "downloads"), "casm-dbbj-query.json");

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => adapter.DownloadAsync(request, progress: null, CancellationToken.None));

        Assert.Contains("malformed JSON", exception.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task Download_async_rejects_an_existing_target_file()
    {
        var workingDirectory = Path.Combine(Path.GetTempPath(), $"download-adapter-existing-file-{Guid.NewGuid():N}");
        var downloadDirectory = Path.Combine(workingDirectory, "downloads");
        Directory.CreateDirectory(downloadDirectory);

        var mockResponsePath = Path.Combine(workingDirectory, "mock-response.json");
        await File.WriteAllTextAsync(mockResponsePath, "{\"results\":[]}");

        var existingFile = Path.Combine(downloadDirectory, "casm-dbbj-query.json");
        await File.WriteAllTextAsync(existingFile, "existing");

        var adapter = new FileDownloadAdapter(
            new HttpClient(),
            Options.Create(new SocrataOptions
            {
                AppToken = Guid.NewGuid().ToString("N"),
                MockResponsePath = mockResponsePath,
            }));

        var request = new DownloadRequest(true, downloadDirectory, "casm-dbbj-query.json");

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => adapter.DownloadAsync(request, progress: null, CancellationToken.None));

        Assert.Contains("already exists", exception.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task Download_async_deletes_stale_in_progress_temp_files_before_writing_the_new_file()
    {
        var workingDirectory = Path.Combine(Path.GetTempPath(), $"download-adapter-stale-temp-{Guid.NewGuid():N}");
        var downloadDirectory = Path.Combine(workingDirectory, "downloads");
        Directory.CreateDirectory(downloadDirectory);

        var staleTempFile = Path.Combine(downloadDirectory, ".casm-dbbj-query.json.stale.tmp");
        await File.WriteAllTextAsync(staleTempFile, "partial-download");

        var mockResponsePath = Path.Combine(workingDirectory, "mock-response.json");
        var payload = "{\"results\":[{\"transactionid\":\"TX-1\"}]}";
        await File.WriteAllTextAsync(mockResponsePath, payload);

        var adapter = new FileDownloadAdapter(
            new HttpClient(),
            Options.Create(new SocrataOptions
            {
                AppToken = Guid.NewGuid().ToString("N"),
                MockResponsePath = mockResponsePath,
            }));

        var request = new DownloadRequest(true, downloadDirectory, "casm-dbbj-query.json");

        var downloadedFile = await adapter.DownloadAsync(request, progress: null, CancellationToken.None);

        Assert.False(File.Exists(staleTempFile));
        Assert.True(File.Exists(downloadedFile.Path));
        Assert.Equal(payload, await File.ReadAllTextAsync(downloadedFile.Path));
    }
}