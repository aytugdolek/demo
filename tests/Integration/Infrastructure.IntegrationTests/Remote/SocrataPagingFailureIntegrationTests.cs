using System.Net;
using System.Text;
using Colorado.BusinessEntityTransactionHistory.Application.Configuration;
using Colorado.BusinessEntityTransactionHistory.Application.Paging;
using Colorado.BusinessEntityTransactionHistory.Infrastructure.Remote;
using Microsoft.Extensions.Options;

namespace Colorado.BusinessEntityTransactionHistory.Infrastructure.IntegrationTests.Remote;

public sealed class SocrataPagingFailureIntegrationTests
{
    [Fact]
    public async Task Empty_results_return_an_empty_page()
    {
        var appToken = Guid.NewGuid().ToString("N");
        var httpClient = new HttpClient(new StaticHttpMessageHandler(HttpStatusCode.OK, "{\"results\":[]}"));
        var adapter = new SocrataTransactionHistoryAdapter(httpClient, Options.Create(new SocrataOptions { AppToken = appToken }));

        var page = await adapter.GetPageAsync(new RemoteTransactionHistoryQuery(2), CancellationToken.None);

        Assert.True(page.IsEmpty);
        Assert.Equal(2, page.PageNumber);
    }

    [Fact]
    public async Task Transport_failures_surface_a_clear_message()
    {
        var appToken = Guid.NewGuid().ToString("N");
        var httpClient = new HttpClient(new StaticHttpMessageHandler(HttpStatusCode.BadGateway, "{\"message\":\"Gateway timeout\"}"));
        var adapter = new SocrataTransactionHistoryAdapter(httpClient, Options.Create(new SocrataOptions { AppToken = appToken }));

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => adapter.GetPageAsync(new RemoteTransactionHistoryQuery(1), CancellationToken.None));

        Assert.Contains("Gateway timeout", exception.Message, StringComparison.OrdinalIgnoreCase);
    }

    private sealed class StaticHttpMessageHandler : HttpMessageHandler
    {
        private readonly HttpStatusCode _statusCode;
        private readonly string _payload;

        public StaticHttpMessageHandler(HttpStatusCode statusCode, string payload)
        {
            _statusCode = statusCode;
            _payload = payload;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new HttpResponseMessage(_statusCode)
            {
                Content = new StringContent(_payload, Encoding.UTF8, "application/json"),
            });
        }
    }
}