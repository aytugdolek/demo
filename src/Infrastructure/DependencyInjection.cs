using Colorado.BusinessEntityTransactionHistory.Application.Abstractions;
using Colorado.BusinessEntityTransactionHistory.Application.Configuration;
using Colorado.BusinessEntityTransactionHistory.Infrastructure.Persistence;
using Colorado.BusinessEntityTransactionHistory.Infrastructure.Remote;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Colorado.BusinessEntityTransactionHistory.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddOptions<SocrataOptions>()
            .Bind(configuration.GetSection(SocrataOptions.SectionName));

        services
            .AddOptions<DemoDbOptions>()
            .Configure(options =>
            {
                options.ConnectionString = configuration.GetConnectionString("DemoDb") ?? string.Empty;
            });

        services.AddHttpClient<IRemoteTransactionHistoryPort, SocrataTransactionHistoryAdapter>();
        services.AddSingleton<IPersistencePort, PlaceholderPersistenceAdapter>();

        return services;
    }
}