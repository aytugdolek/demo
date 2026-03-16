namespace Colorado.BusinessEntityTransactionHistory.Application.Paging;

public sealed class PagingSessionState
{
    public PagingSessionState(int currentPageNumber, int pageSize, NavigationCommandKind lastCommand, string? statusMessage)
    {
        CurrentPageNumber = currentPageNumber;
        PageSize = pageSize;
        LastCommand = lastCommand;
        StatusMessage = statusMessage;
    }

    public int CurrentPageNumber { get; }

    public int PageSize { get; }

    public bool CanMovePrevious => CurrentPageNumber > 1;

    public bool CanMoveNext { get; init; }

    public NavigationCommandKind LastCommand { get; }

    public string? StatusMessage { get; }
}