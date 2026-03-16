using System.Text.Json.Serialization;

namespace Colorado.BusinessEntityTransactionHistory.Infrastructure.Remote;

public sealed record SocrataQueryRequest(
    [property: JsonPropertyName("query")] string Query,
    [property: JsonPropertyName("page")] SocrataPageRequest Page,
    [property: JsonPropertyName("includeSynthetic")] bool IncludeSynthetic = false);

public sealed record SocrataPageRequest(
    [property: JsonPropertyName("pageNumber")] int PageNumber,
    [property: JsonPropertyName("pageSize")] int PageSize);