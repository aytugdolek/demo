using Colorado.BusinessEntityTransactionHistory.Application;
using Colorado.BusinessEntityTransactionHistory.Application.Abstractions;
using Colorado.BusinessEntityTransactionHistory.Application.Configuration;
using Colorado.BusinessEntityTransactionHistory.Application.Startup;
using Colorado.BusinessEntityTransactionHistory.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Colorado.BusinessEntityTransactionHistory.Infrastructure.IntegrationTests.Composition;

public sealed class InfrastructureCompositionTests
{
    [Fact]
    public void Composition_root_registers_application_and_infrastructure_services()
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>())
            .Build();

        var services = new ServiceCollection();
        services.AddApplication();
        services.AddInfrastructure(configuration);

        using var provider = services.BuildServiceProvider();

        Assert.NotNull(provider.GetService<GetBaselineStartupSummary>());
        Assert.NotNull(provider.GetService<RuntimeConfigurationValidator>());
        Assert.NotNull(provider.GetService<IRemoteTransactionHistoryPort>());
        Assert.NotNull(provider.GetService<IPersistencePort>());
    }
}