using Colorado.BusinessEntityTransactionHistory.Application.Abstractions;
using Colorado.BusinessEntityTransactionHistory.Application.Configuration;
using Colorado.BusinessEntityTransactionHistory.Application.Paging;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;
using System.Text.Json;

namespace Colorado.BusinessEntityTransactionHistory.Infrastructure.Remote;

public sealed class SocrataTransactionHistoryAdapter : IRemoteTransactionHistoryPort
{
    private readonly HttpClient _httpClient;
    private readonly IOptions<SocrataOptions> _socrataOptions;

    public SocrataTransactionHistoryAdapter(HttpClient httpClient, IOptions<SocrataOptions> socrataOptions)
    {
        _httpClient = httpClient;
        _socrataOptions = socrataOptions;
    }

    public Task<TransactionHistoryPage> GetPageAsync(RemoteTransactionHistoryQuery query, CancellationToken cancellationToken)
    {
        return GetPageCoreAsync(query, cancellationToken);
    }

    private async Task<TransactionHistoryPage> GetPageCoreAsync(RemoteTransactionHistoryQuery query, CancellationToken cancellationToken)
    {
        var payload = await GetPayloadAsync(query, cancellationToken);
        var records = ParseRecords(payload);

        return new TransactionHistoryPage(query.PageNumber, query.PageSize, records);
    }

    private async Task<string> GetPayloadAsync(RemoteTransactionHistoryQuery query, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrWhiteSpace(_socrataOptions.Value.MockResponsePath))
        {
            var mockResponsePath = _socrataOptions.Value.MockResponsePath.Replace("{pageNumber}", query.PageNumber.ToString(), StringComparison.Ordinal);
            return await File.ReadAllTextAsync(mockResponsePath, cancellationToken);
        }

        using var request = new HttpRequestMessage(HttpMethod.Post, _socrataOptions.Value.BaseUrl);
        request.Headers.Add("X-App-Token", _socrataOptions.Value.AppToken);
        request.Content = JsonContent.Create(new SocrataQueryRequest(
            "SELECT transactionid, entityid, name, historydes, receiveddate, comment, effectivedate ORDER BY receiveddate DESC, transactionid DESC",
            new SocrataPageRequest(query.PageNumber, query.PageSize)));

        using var response = await _httpClient.SendAsync(request, cancellationToken);
        var content = await response.Content.ReadAsStringAsync(cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            throw new InvalidOperationException(GetErrorMessage(content, response.ReasonPhrase));
        }

        return content;
    }

    private static string GetErrorMessage(string content, string? reasonPhrase)
    {
        try
        {
            using var document = JsonDocument.Parse(content);
            if (document.RootElement.ValueKind == JsonValueKind.Object &&
                document.RootElement.TryGetProperty("message", out var messageElement))
            {
                return $"Remote request failed: {messageElement.GetString()}";
            }
        }
        catch (JsonException)
        {
        }

        return $"Remote request failed: {reasonPhrase ?? "Unknown error"}";
    }

    private static IReadOnlyList<RemoteTransactionHistoryRecord> ParseRecords(string payload)
    {
        using var document = JsonDocument.Parse(payload);
        var records = new List<RemoteTransactionHistoryRecord>();

        foreach (var row in GetRowElements(document.RootElement))
        {
            records.Add(row.ValueKind switch
            {
                JsonValueKind.Object => MapObjectRow(row),
                JsonValueKind.Array => MapArrayRow(row),
                _ => throw new InvalidOperationException("Unsupported Socrata row format."),
            });
        }

        return records;
    }

    private static IEnumerable<JsonElement> GetRowElements(JsonElement root)
    {
        if (root.ValueKind == JsonValueKind.Array)
        {
            return root.EnumerateArray();
        }

        if (root.ValueKind == JsonValueKind.Object)
        {
            if (root.TryGetProperty("results", out var results) && results.ValueKind == JsonValueKind.Array)
            {
                return results.EnumerateArray();
            }

            if (root.TryGetProperty("data", out var data) && data.ValueKind == JsonValueKind.Array)
            {
                return data.EnumerateArray();
            }
        }

        return Array.Empty<JsonElement>();
    }

    private static RemoteTransactionHistoryRecord MapObjectRow(JsonElement row)
    {
        var dto = JsonSerializer.Deserialize<SocrataTransactionHistoryRecordDto>(row.GetRawText())
            ?? new SocrataTransactionHistoryRecordDto(null, null, null, null, null, null, null);

        return new RemoteTransactionHistoryRecord(
            dto.EntityId ?? string.Empty,
            dto.TransactionId ?? string.Empty,
            dto.Name ?? string.Empty,
            dto.HistoryDescription ?? string.Empty,
            dto.Comment,
            ParseDate(dto.ReceivedDate),
            ParseDate(dto.EffectiveDate));
    }

    private static RemoteTransactionHistoryRecord MapArrayRow(JsonElement row)
    {
        var values = row.EnumerateArray().ToArray();

        string? GetValue(int index) => index < values.Length ? values[index].GetString() : null;

        return new RemoteTransactionHistoryRecord(
            GetValue(1) ?? string.Empty,
            GetValue(0) ?? string.Empty,
            GetValue(2) ?? string.Empty,
            GetValue(3) ?? string.Empty,
            GetValue(5),
            ParseDate(GetValue(4)),
            ParseDate(GetValue(6)));
    }

    private static DateTimeOffset? ParseDate(string? value)
    {
        return DateTimeOffset.TryParse(value, out var parsed) ? parsed : null;
    }
}