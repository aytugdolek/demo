using Colorado.BusinessEntityTransactionHistory.Application.Configuration;
using Spectre.Console;

namespace Colorado.BusinessEntityTransactionHistory.Cli.Commands;

public sealed class ValidateConfigCommand
{
    private readonly RuntimeConfigurationValidator _runtimeConfigurationValidator;

    public ValidateConfigCommand(RuntimeConfigurationValidator runtimeConfigurationValidator)
    {
        _runtimeConfigurationValidator = runtimeConfigurationValidator;
    }

    public int Execute()
    {
        var outcomes = _runtimeConfigurationValidator.Validate();

        foreach (var outcome in outcomes)
        {
            var color = outcome.IsSuccess ? "green" : "red";
            AnsiConsole.MarkupLine($"[{color}]{outcome.CheckName}[/]: {outcome.Message}");
        }

        if (outcomes.All(outcome => outcome.IsSuccess))
        {
            AnsiConsole.MarkupLine("[green]Configuration validation succeeded.[/]");
            return 0;
        }

        AnsiConsole.MarkupLine("[red]Configuration validation failed.[/]");
        return 1;
    }
}