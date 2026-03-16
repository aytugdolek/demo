namespace Colorado.BusinessEntityTransactionHistory.Application.Paging;

public sealed class NavigationCommandParser
{
    public NavigationCommand Parse(string? input)
    {
        var trimmed = input?.Trim() ?? string.Empty;

        return trimmed switch
        {
            ">" => new NavigationCommand(trimmed, NavigationCommandKind.Next),
            "<" => new NavigationCommand(trimmed, NavigationCommandKind.Previous),
            _ when trimmed.Equals("Q", StringComparison.OrdinalIgnoreCase) => new NavigationCommand(trimmed, NavigationCommandKind.Quit),
            _ => new NavigationCommand(trimmed, NavigationCommandKind.Unknown),
        };
    }
}