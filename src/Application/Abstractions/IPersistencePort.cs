namespace Colorado.BusinessEntityTransactionHistory.Application.Abstractions;

public interface IPersistencePort
{
    Task<string> GetStatusAsync(CancellationToken cancellationToken);
}