namespace Colorado.BusinessEntityTransactionHistory.Application.Paging;

public enum NavigationCommandKind
{
    Next,
    Previous,
    Quit,
    Unknown,
}

public sealed record NavigationCommand(string RawInput, NavigationCommandKind Kind);