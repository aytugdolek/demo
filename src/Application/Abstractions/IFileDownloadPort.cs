using Colorado.BusinessEntityTransactionHistory.Application.Downloads;

namespace Colorado.BusinessEntityTransactionHistory.Application.Abstractions;

public interface IFileDownloadPort
{
    Task<DownloadTarget> GetTargetAsync(DownloadRequest request, CancellationToken cancellationToken);

    Task<DownloadedFile> DownloadAsync(
        DownloadRequest request,
        IProgress<DownloadProgressUpdate>? progress,
        CancellationToken cancellationToken);
}