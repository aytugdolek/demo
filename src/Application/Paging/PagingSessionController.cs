namespace Colorado.BusinessEntityTransactionHistory.Application.Paging;

public sealed class PagingSessionController
{
    public PagingSessionState Initialize(TransactionHistoryPage page, NavigationCommandKind lastCommand = NavigationCommandKind.Unknown, string? statusMessage = null)
    {
        return new PagingSessionState(page.PageNumber, page.PageSize, lastCommand, statusMessage)
        {
            CanMoveNext = page.HasNextPage,
        };
    }

    public PagingSessionState Apply(PagingSessionState currentState, NavigationCommand command, TransactionHistoryPage? loadedPage = null)
    {
        return command.Kind switch
        {
            NavigationCommandKind.Previous when currentState.CurrentPageNumber == 1 => new PagingSessionState(
                1,
                currentState.PageSize,
                NavigationCommandKind.Previous,
                "No earlier page is available.")
            {
                CanMoveNext = currentState.CanMoveNext,
            },
            NavigationCommandKind.Previous => Initialize(loadedPage ?? throw new InvalidOperationException("A previous page result is required."), NavigationCommandKind.Previous),
            NavigationCommandKind.Next when loadedPage is { IsEmpty: true } => new PagingSessionState(
                currentState.CurrentPageNumber,
                currentState.PageSize,
                NavigationCommandKind.Next,
                "No further results are available.")
            {
                CanMoveNext = false,
            },
            NavigationCommandKind.Next => Initialize(loadedPage ?? throw new InvalidOperationException("A next page result is required."), NavigationCommandKind.Next),
            NavigationCommandKind.Unknown => new PagingSessionState(
                currentState.CurrentPageNumber,
                currentState.PageSize,
                NavigationCommandKind.Unknown,
                "Unsupported input. Enter >, <, or Q.")
            {
                CanMoveNext = currentState.CanMoveNext,
            },
            NavigationCommandKind.Quit => new PagingSessionState(currentState.CurrentPageNumber, currentState.PageSize, NavigationCommandKind.Quit, null)
            {
                CanMoveNext = currentState.CanMoveNext,
            },
            _ => currentState,
        };
    }
}