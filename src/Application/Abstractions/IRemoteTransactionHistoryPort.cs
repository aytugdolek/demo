using Colorado.BusinessEntityTransactionHistory.Application.Paging;

namespace Colorado.BusinessEntityTransactionHistory.Application.Abstractions;

public interface IRemoteTransactionHistoryPort
{
    Task<TransactionHistoryPage> GetPageAsync(RemoteTransactionHistoryQuery query, CancellationToken cancellationToken);
}