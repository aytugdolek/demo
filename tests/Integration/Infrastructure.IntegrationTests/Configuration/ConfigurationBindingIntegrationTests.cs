using Colorado.BusinessEntityTransactionHistory.Application.Configuration;
using Colorado.BusinessEntityTransactionHistory.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Colorado.BusinessEntityTransactionHistory.Infrastructure.IntegrationTests.Configuration;

public sealed class ConfigurationBindingIntegrationTests
{
    [Fact]
    public void Infrastructure_registration_binds_runtime_configuration_options()
    {
        var appToken = Guid.NewGuid().ToString("N");
        var values = new Dictionary<string, string?>
        {
            ["Socrata:AppToken"] = appToken,
            ["ConnectionStrings:DemoDb"] = "Server=localhost\\DEMO;Database=DemoDb;Integrated Security=True;Connect Timeout=0;TrustServerCertificate=True;",
        };

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(values)
            .Build();

        var services = new ServiceCollection();
        services.AddInfrastructure(configuration);

        using var provider = services.BuildServiceProvider();

        var socrataOptions = provider.GetRequiredService<IOptions<SocrataOptions>>().Value;
        var demoDbOptions = provider.GetRequiredService<IOptions<DemoDbOptions>>().Value;

        Assert.Equal(appToken, socrataOptions.AppToken);
        Assert.Contains("Server=localhost\\DEMO", demoDbOptions.ConnectionString, StringComparison.Ordinal);
    }
}