using System.Text.Json.Serialization;

namespace Colorado.BusinessEntityTransactionHistory.Infrastructure.Remote;

public sealed record SocrataTransactionHistoryRecordDto(
    [property: JsonPropertyName("entityid")] string? EntityId,
    [property: JsonPropertyName("transactionid")] string? TransactionId,
    [property: JsonPropertyName("name")] string? Name,
    [property: JsonPropertyName("historydes")] string? HistoryDescription,
    [property: JsonPropertyName("comment")] string? Comment,
    [property: JsonPropertyName("receiveddate")] string? ReceivedDate,
    [property: JsonPropertyName("effectivedate")] string? EffectiveDate);