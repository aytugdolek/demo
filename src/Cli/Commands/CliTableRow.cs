namespace Colorado.BusinessEntityTransactionHistory.Cli.Commands;

public sealed record CliTableRow(
    string TransactionId,
    string EntityId,
    string Name,
    string History,
    string Received);