using Colorado.BusinessEntityTransactionHistory.Application.Paging;
using Spectre.Console;

namespace Colorado.BusinessEntityTransactionHistory.Cli.Commands;

public sealed class TransactionHistoryTableRenderer
{
    private readonly TransactionHistoryTableRowMapper _rowMapper;

    public TransactionHistoryTableRenderer(TransactionHistoryTableRowMapper rowMapper)
    {
        _rowMapper = rowMapper;
    }

    public void Render(TransactionHistoryPage page, string? statusMessage = null)
    {
        AnsiConsole.MarkupLine($"[blue]Page {page.PageNumber}[/]");

        if (!string.IsNullOrWhiteSpace(statusMessage))
        {
            AnsiConsole.MarkupLine($"[yellow]{Markup.Escape(statusMessage)}[/]");
        }

        if (page.IsEmpty)
        {
            AnsiConsole.MarkupLine("[yellow]No transaction history records were returned for this page.[/]");
            return;
        }

        var table = new Table().Expand();
        table.AddColumn("Transaction ID");
        table.AddColumn("Entity ID");
        table.AddColumn("Name");
        table.AddColumn("History");
        table.AddColumn("Received");

        foreach (var record in page.Records)
        {
            var row = _rowMapper.Map(record);
            table.AddRow(row.TransactionId, row.EntityId, row.Name, row.History, row.Received);
        }

        AnsiConsole.Write(table);
    }
}