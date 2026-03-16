namespace Colorado.BusinessEntityTransactionHistory.Application.Paging;

public sealed class TransactionHistoryPage
{
    public TransactionHistoryPage(int pageNumber, int pageSize, IReadOnlyList<RemoteTransactionHistoryRecord> records)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
        Records = records;
    }

    public int PageNumber { get; }

    public int PageSize { get; }

    public IReadOnlyList<RemoteTransactionHistoryRecord> Records { get; }

    public bool IsEmpty => Records.Count == 0;

    public bool HasPreviousPage => PageNumber > 1;

    public bool HasNextPage => Records.Count == PageSize;
}