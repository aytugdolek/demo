using Colorado.BusinessEntityTransactionHistory.Application.Abstractions;

namespace Colorado.BusinessEntityTransactionHistory.Infrastructure.Persistence;

public sealed class PlaceholderPersistenceAdapter : IPersistencePort
{
    public Task<string> GetStatusAsync(CancellationToken cancellationToken)
    {
        return Task.FromResult("Persistence adapter placeholder is registered.");
    }
}