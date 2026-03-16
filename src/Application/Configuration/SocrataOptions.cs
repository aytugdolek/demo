namespace Colorado.BusinessEntityTransactionHistory.Application.Configuration;

public sealed class SocrataOptions
{
    public const string SectionName = "Socrata";

    public string AppToken { get; set; } = string.Empty;
}