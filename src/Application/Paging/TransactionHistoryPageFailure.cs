namespace Colorado.BusinessEntityTransactionHistory.Application.Paging;

public sealed record TransactionHistoryPageFailure(string Message, Exception? Exception = null);