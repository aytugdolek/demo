using Colorado.BusinessEntityTransactionHistory.Application.Startup;
using Colorado.BusinessEntityTransactionHistory.Cli.Commands;
using Colorado.BusinessEntityTransactionHistory.Application;
using Colorado.BusinessEntityTransactionHistory.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Spectre.Console;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddSingleton<BaselineCommand>();
builder.Services.AddSingleton<InteractivePagingCommand>();
builder.Services.AddSingleton<TransactionHistoryTableRowMapper>();
builder.Services.AddSingleton<TransactionHistoryTableRenderer>();
builder.Services.AddSingleton<ValidateConfigCommand>();

using var host = builder.Build();

if (args.Contains("--help", StringComparer.OrdinalIgnoreCase))
{
    ShowHelp();
    return 0;
}

if (args.Contains("--validate-config", StringComparer.OrdinalIgnoreCase))
{
    var validateConfigCommand = host.Services.GetRequiredService<ValidateConfigCommand>();
    return validateConfigCommand.Execute();
}

var interactivePagingCommand = host.Services.GetRequiredService<InteractivePagingCommand>();
return await interactivePagingCommand.ExecuteAsync();

static void ShowHelp()
{
    AnsiConsole.MarkupLine("[yellow]Colorado Business Entity Transaction History CLI[/]");
    AnsiConsole.MarkupLine("Run without arguments to start the interactive paging workflow.");
    AnsiConsole.MarkupLine("--validate-config  Validate required configuration without calling live integrations.");
    AnsiConsole.MarkupLine("--download         Reserved for a future slice.");
    AnsiConsole.MarkupLine("Paging controls: > for next page, < for previous page, Q to quit.");
}