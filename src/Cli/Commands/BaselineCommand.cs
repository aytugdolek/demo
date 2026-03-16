using Colorado.BusinessEntityTransactionHistory.Application.Startup;
using Spectre.Console;

namespace Colorado.BusinessEntityTransactionHistory.Cli.Commands;

public sealed class BaselineCommand
{
    private readonly GetBaselineStartupSummary _getBaselineStartupSummary;

    public BaselineCommand(GetBaselineStartupSummary getBaselineStartupSummary)
    {
        _getBaselineStartupSummary = getBaselineStartupSummary;
    }

    public int Execute()
    {
        var summary = _getBaselineStartupSummary.Execute();

        AnsiConsole.MarkupLine($"[green]{summary.Title}[/]");
        AnsiConsole.MarkupLine(summary.Description);
        AnsiConsole.MarkupLine(summary.NextActions);

        return 0;
    }
}