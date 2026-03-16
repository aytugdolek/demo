using System.Net;
using System.Text;
using Colorado.BusinessEntityTransactionHistory.Application.Configuration;
using Colorado.BusinessEntityTransactionHistory.Application.Paging;
using Colorado.BusinessEntityTransactionHistory.Infrastructure.Remote;
using Microsoft.Extensions.Options;

namespace Colorado.BusinessEntityTransactionHistory.Infrastructure.IntegrationTests.Remote;

public sealed class SocrataPagingIntegrationTests
{
    [Fact]
    public async Task Adapter_returns_different_records_for_different_page_requests()
    {
    var appToken = Guid.NewGuid().ToString("N");
        var httpClient = new HttpClient(new PagingHttpMessageHandler())
        {
            BaseAddress = new Uri("https://data.colorado.gov/api/v3/views/casm-dbbj/"),
        };
        var adapter = new SocrataTransactionHistoryAdapter(
            httpClient,
      Options.Create(new SocrataOptions { AppToken = appToken }));

        var firstPage = await adapter.GetPageAsync(new RemoteTransactionHistoryQuery(1), CancellationToken.None);
        var secondPage = await adapter.GetPageAsync(new RemoteTransactionHistoryQuery(2), CancellationToken.None);

        Assert.Equal("TX-1", Assert.Single(firstPage.Records).TransactionId);
        Assert.Equal("TX-2", Assert.Single(secondPage.Records).TransactionId);
    }

    private sealed class PagingHttpMessageHandler : HttpMessageHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var content = await request.Content!.ReadAsStringAsync(cancellationToken);
            var payload = content.Contains("\"pageNumber\":2", StringComparison.Ordinal)
                ? """
                {
                  "results": [
                    {
                      "transactionid": "TX-2",
                      "entityid": "ENT-2",
                      "name": "Beacon Services",
                      "historydes": "Updated",
                      "receiveddate": "2026-03-14T09:15:00Z"
                    }
                  ]
                }
                """
                : """
                {
                  "results": [
                    {
                      "transactionid": "TX-1",
                      "entityid": "ENT-1",
                      "name": "Acme Holdings",
                      "historydes": "Created",
                      "receiveddate": "2026-03-15T10:45:00Z"
                    }
                  ]
                }
                """;

            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(payload, Encoding.UTF8, "application/json"),
            };
        }
    }
}