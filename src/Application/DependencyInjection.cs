using Colorado.BusinessEntityTransactionHistory.Application.Configuration;
using Colorado.BusinessEntityTransactionHistory.Application.Paging;
using Colorado.BusinessEntityTransactionHistory.Application.Startup;
using Microsoft.Extensions.DependencyInjection;

namespace Colorado.BusinessEntityTransactionHistory.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddSingleton<GetBaselineStartupSummary>();
        services.AddSingleton<GetTransactionHistoryPage>();
        services.AddSingleton<NavigationCommandParser>();
        services.AddSingleton<PagingSessionController>();
        services.AddSingleton<RuntimeConfigurationValidator>();
        services.AddSingleton<TransactionHistoryPageMapper>();

        return services;
    }
}