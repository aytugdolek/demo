namespace Colorado.BusinessEntityTransactionHistory.Application.Paging;

public sealed record RemoteTransactionHistoryRecord(
    string EntityId,
    string TransactionId,
    string Name,
    string HistoryDescription,
    string? Comment,
    DateTimeOffset? ReceivedDate,
    DateTimeOffset? EffectiveDate);