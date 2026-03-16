using Colorado.BusinessEntityTransactionHistory.Application.Startup;
using Colorado.BusinessEntityTransactionHistory.Cli.Commands;
using Colorado.BusinessEntityTransactionHistory.Application;
using Colorado.BusinessEntityTransactionHistory.Application.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Spectre.Console;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddApplication();
builder.Services.AddSingleton<BaselineCommand>();
builder.Services.AddSingleton<ValidateConfigCommand>();
builder.Services
    .AddOptions<SocrataOptions>()
    .Bind(builder.Configuration.GetSection(SocrataOptions.SectionName));
builder.Services
    .AddOptions<DemoDbOptions>()
    .Configure(options =>
    {
        options.ConnectionString = builder.Configuration.GetConnectionString("DemoDb") ?? string.Empty;
    });

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

var baselineCommand = host.Services.GetRequiredService<BaselineCommand>();
return baselineCommand.Execute();

static void ShowHelp()
{
    AnsiConsole.MarkupLine("[yellow]Colorado Business Entity Transaction History CLI[/]");
    AnsiConsole.MarkupLine("Run without arguments to confirm the baseline solution is ready.");
    AnsiConsole.MarkupLine("--validate-config  Validate required configuration without calling live integrations.");
    AnsiConsole.MarkupLine("--download         Reserved for a future slice.");
}