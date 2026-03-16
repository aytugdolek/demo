namespace Colorado.BusinessEntityTransactionHistory.Application.Paging;

public sealed class RemoteTransactionHistoryQuery
{
    public const int DefaultPageSize = 20;
    public const string PrimarySortField = "receiveddate";
    public const string SecondarySortField = "transactionid";

    public RemoteTransactionHistoryQuery(int pageNumber, int pageSize = DefaultPageSize)
    {
        if (pageNumber < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(pageNumber), pageNumber, "Page number must be at least 1.");
        }

        if (pageSize < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(pageSize), pageSize, "Page size must be positive.");
        }

        PageNumber = pageNumber;
        PageSize = pageSize;
    }

    public int PageNumber { get; }

    public int PageSize { get; }

    public string SortField => PrimarySortField;

    public string SecondarySortFieldName => SecondarySortField;
}