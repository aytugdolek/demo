namespace Colorado.BusinessEntityTransactionHistory.Application.Configuration;

public sealed class SocrataOptions
{
    public const string SectionName = "Socrata";

    public const string DefaultBaseUrl = "https://data.colorado.gov/api/v3/views/casm-dbbj/query.json";

    public string AppToken { get; set; } = string.Empty;

    public string BaseUrl { get; set; } = DefaultBaseUrl;

    public string MockResponsePath { get; set; } = string.Empty;
}