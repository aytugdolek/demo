namespace Colorado.BusinessEntityTransactionHistory.Application.Paging;

public sealed class PagingCommandResult
{
    private PagingCommandResult(bool shouldExit, string? statusMessage, TransactionHistoryPage? page, TransactionHistoryPageFailure? failure)
    {
        ShouldExit = shouldExit;
        StatusMessage = statusMessage;
        Page = page;
        Failure = failure;
    }

    public bool ShouldExit { get; }

    public string? StatusMessage { get; }

    public TransactionHistoryPage? Page { get; }

    public TransactionHistoryPageFailure? Failure { get; }

    public static PagingCommandResult Exit()
    {
        return new PagingCommandResult(true, null, null, null);
    }

    public static PagingCommandResult InvalidInput()
    {
        return new PagingCommandResult(false, "Unsupported input. Enter >, <, or Q.", null, null);
    }

    public static PagingCommandResult Status(string message)
    {
        return new PagingCommandResult(false, message, null, null);
    }

    public static PagingCommandResult FromFailure(string message, Exception? exception = null)
    {
        return new PagingCommandResult(false, message, null, new TransactionHistoryPageFailure(message, exception));
    }
}