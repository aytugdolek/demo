using Colorado.BusinessEntityTransactionHistory.Application.Paging;

namespace Colorado.BusinessEntityTransactionHistory.Cli.Commands;

public sealed class TransactionHistoryTableRowMapper
{
    public CliTableRow Map(RemoteTransactionHistoryRecord record)
    {
        return new CliTableRow(
            Clip(record.TransactionId, 18),
            Clip(record.EntityId, 18),
            Clip(record.Name, 28),
            Clip(record.HistoryDescription, 36),
            record.ReceivedDate?.ToString("yyyy-MM-dd") ?? string.Empty);
    }

    private static string Clip(string? value, int maxLength)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return string.Empty;
        }

        return value.Length <= maxLength ? value : $"{value[..(maxLength - 1)]}…";
    }
}