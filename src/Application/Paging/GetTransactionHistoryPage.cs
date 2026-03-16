using Colorado.BusinessEntityTransactionHistory.Application.Abstractions;

namespace Colorado.BusinessEntityTransactionHistory.Application.Paging;

public sealed class GetTransactionHistoryPage
{
    private readonly IRemoteTransactionHistoryPort _remoteTransactionHistoryPort;
    private readonly TransactionHistoryPageMapper _transactionHistoryPageMapper;

    public GetTransactionHistoryPage(
        IRemoteTransactionHistoryPort remoteTransactionHistoryPort,
        TransactionHistoryPageMapper transactionHistoryPageMapper)
    {
        _remoteTransactionHistoryPort = remoteTransactionHistoryPort;
        _transactionHistoryPageMapper = transactionHistoryPageMapper;
    }

    public async Task<TransactionHistoryPage> ExecuteAsync(int pageNumber, CancellationToken cancellationToken = default)
    {
        var query = new RemoteTransactionHistoryQuery(pageNumber);
        var page = await _remoteTransactionHistoryPort.GetPageAsync(query, cancellationToken);

        return _transactionHistoryPageMapper.Map(query, page.Records);
    }
}