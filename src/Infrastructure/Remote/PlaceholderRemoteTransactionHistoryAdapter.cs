using Colorado.BusinessEntityTransactionHistory.Application.Abstractions;

namespace Colorado.BusinessEntityTransactionHistory.Infrastructure.Remote;

public sealed class PlaceholderRemoteTransactionHistoryAdapter : IRemoteTransactionHistoryPort
{
    public Task<string> GetStatusAsync(CancellationToken cancellationToken)
    {
        return Task.FromResult("Remote transaction history adapter placeholder is registered.");
    }
}