using System.Net;
using System.Text;
using Colorado.BusinessEntityTransactionHistory.Application.Configuration;
using Colorado.BusinessEntityTransactionHistory.Application.Paging;
using Colorado.BusinessEntityTransactionHistory.Infrastructure.Remote;
using Microsoft.Extensions.Options;

namespace Colorado.BusinessEntityTransactionHistory.Infrastructure.IntegrationTests.Remote;

public sealed class SocrataTransactionHistoryAdapterTests
{
    [Fact]
    public async Task Get_page_maps_the_first_page_of_records()
    {
                var appToken = Guid.NewGuid().ToString("N");
        const string json = """
        {
          "results": [
            {
              "transactionid": "TX-1",
              "entityid": "ENT-1",
              "name": "Acme Holdings",
              "historydes": "Created",
              "receiveddate": "2026-03-15T10:45:00Z",
              "comment": "Initial filing",
              "effectivedate": "2026-03-15T00:00:00Z"
            }
          ]
        }
        """;

        var httpClient = new HttpClient(new StubHttpMessageHandler(json))
        {
            BaseAddress = new Uri("https://data.colorado.gov/api/v3/views/casm-dbbj/"),
        };
        var adapter = new SocrataTransactionHistoryAdapter(
            httpClient,
            Options.Create(new SocrataOptions { AppToken = appToken }));

        var page = await adapter.GetPageAsync(new RemoteTransactionHistoryQuery(1), CancellationToken.None);

        var record = Assert.Single(page.Records);
        Assert.Equal(1, page.PageNumber);
        Assert.Equal(RemoteTransactionHistoryQuery.DefaultPageSize, page.PageSize);
        Assert.Equal("TX-1", record.TransactionId);
        Assert.Equal("ENT-1", record.EntityId);
        Assert.Equal("Acme Holdings", record.Name);
        Assert.Equal("Created", record.HistoryDescription);
        Assert.Equal(DateTimeOffset.Parse("2026-03-15T10:45:00Z"), record.ReceivedDate);
    }

    private sealed class StubHttpMessageHandler : HttpMessageHandler
    {
        private readonly string _content;

        public StubHttpMessageHandler(string content)
        {
            _content = content;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(_content, Encoding.UTF8, "application/json"),
            };

            return Task.FromResult(response);
        }
    }
}