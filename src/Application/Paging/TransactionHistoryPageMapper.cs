namespace Colorado.BusinessEntityTransactionHistory.Application.Paging;

public sealed class TransactionHistoryPageMapper
{
    public TransactionHistoryPage Map(RemoteTransactionHistoryQuery query, IReadOnlyList<RemoteTransactionHistoryRecord> records)
    {
        return new TransactionHistoryPage(query.PageNumber, query.PageSize, records);
    }
}