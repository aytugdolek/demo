using Colorado.BusinessEntityTransactionHistory.Application.Configuration;
using Colorado.BusinessEntityTransactionHistory.Application.Downloads;
using Colorado.BusinessEntityTransactionHistory.Infrastructure.Persistence;
using Microsoft.Extensions.Options;

namespace Colorado.BusinessEntityTransactionHistory.Infrastructure.IntegrationTests.Persistence;

public sealed class FileDownloadAdapterTests
{
    [Fact]
    public async Task Download_async_writes_the_mock_response_to_a_local_file()
    {
        var workingDirectory = Path.Combine(Path.GetTempPath(), $"download-adapter-success-{Guid.NewGuid():N}");
        Directory.CreateDirectory(workingDirectory);

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

        var request = new DownloadRequest(true, Path.Combine(workingDirectory, "downloads"), "casm-dbbj-query.json");
        var downloadedFile = await adapter.DownloadAsync(request, progress: null, CancellationToken.None);

        Assert.True(File.Exists(downloadedFile.Path));
        Assert.Equal(payload, await File.ReadAllTextAsync(downloadedFile.Path));
        Assert.True(downloadedFile.SizeInBytes > 0);
    }

    [Fact]
    public async Task Download_async_reports_byte_progress_for_mock_payloads()
    {
        var workingDirectory = Path.Combine(Path.GetTempPath(), $"download-adapter-progress-{Guid.NewGuid():N}");
        Directory.CreateDirectory(workingDirectory);

        var mockResponsePath = Path.Combine(workingDirectory, "mock-response.json");
        var payload = "{\"results\":[{\"transactionid\":\"TX-1\"},{\"transactionid\":\"TX-2\"}]}";
        await File.WriteAllTextAsync(mockResponsePath, payload);

        var adapter = new FileDownloadAdapter(
            new HttpClient(),
            Options.Create(new SocrataOptions
            {
                AppToken = Guid.NewGuid().ToString("N"),
                MockResponsePath = mockResponsePath,
            }));

        var request = new DownloadRequest(true, Path.Combine(workingDirectory, "downloads"), "casm-dbbj-query.json");
        var updates = new List<DownloadProgressUpdate>();

        await adapter.DownloadAsync(request, new Progress<DownloadProgressUpdate>(updates.Add), CancellationToken.None);

        Assert.NotEmpty(updates);
        Assert.Equal(0, updates[0].BytesTransferred);
        Assert.Equal(payload.Length, updates[0].TotalBytes);

        var finalUpdate = updates[^1];
        Assert.Equal(payload.Length, finalUpdate.BytesTransferred);
        Assert.Equal(payload.Length, finalUpdate.TotalBytes);
    }
}