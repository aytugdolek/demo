namespace Colorado.BusinessEntityTransactionHistory.Application.Abstractions;

public interface IRemoteTransactionHistoryPort
{
    Task<string> GetStatusAsync(CancellationToken cancellationToken);
}