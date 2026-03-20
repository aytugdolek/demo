using Colorado.BusinessEntityTransactionHistory.Application.Downloads;
using Spectre.Console;

namespace Colorado.BusinessEntityTransactionHistory.Cli.Commands;

public sealed class DownloadOutputFormatter
{
    public void Write(DownloadResult result)
    {
        var color = result.Success ? "green" : "red";
        AnsiConsole.MarkupLine($"[{color}]{Markup.Escape(result.Message)}[/]");
    }
}