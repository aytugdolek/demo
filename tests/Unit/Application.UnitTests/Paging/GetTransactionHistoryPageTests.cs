using Colorado.BusinessEntityTransactionHistory.Application.Abstractions;
using Colorado.BusinessEntityTransactionHistory.Application.Paging;

namespace Colorado.BusinessEntityTransactionHistory.Application.UnitTests.Paging;

public sealed class GetTransactionHistoryPageTests
{
    [Fact]
    public async Task Execute_async_requests_the_first_page_and_returns_the_mapped_result()
    {
        var remotePort = new RecordingRemoteTransactionHistoryPort();
        var useCase = new GetTransactionHistoryPage(remotePort, new TransactionHistoryPageMapper());

        var page = await useCase.ExecuteAsync(1, CancellationToken.None);

        Assert.Equal(1, remotePort.LastQuery?.PageNumber);
        Assert.Equal(RemoteTransactionHistoryQuery.DefaultPageSize, remotePort.LastQuery?.PageSize);
        Assert.Equal(1, page.PageNumber);
        Assert.Equal(RemoteTransactionHistoryQuery.DefaultPageSize, page.PageSize);
        Assert.Equal("TX-1", Assert.Single(page.Records).TransactionId);
    }

    private sealed class RecordingRemoteTransactionHistoryPort : IRemoteTransactionHistoryPort
    {
        public RemoteTransactionHistoryQuery? LastQuery { get; private set; }

        public Task<TransactionHistoryPage> GetPageAsync(RemoteTransactionHistoryQuery query, CancellationToken cancellationToken)
        {
            LastQuery = query;

            return Task.FromResult(new TransactionHistoryPage(
                query.PageNumber,
                query.PageSize,
                [new RemoteTransactionHistoryRecord("ENT-1", "TX-1", "Acme", "Created", null, null, null)]));
        }
    }
}