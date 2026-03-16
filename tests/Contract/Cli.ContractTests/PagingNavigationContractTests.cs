using Colorado.BusinessEntityTransactionHistory.Cli.ContractTests.TestSupport;

namespace Colorado.BusinessEntityTransactionHistory.Cli.ContractTests;

public sealed class PagingNavigationContractTests
{
    [Fact]
    public async Task Paging_session_moves_forward_and_backward_between_pages()
    {
        var runner = new CliCommandRunner();
        var responsePattern = CreateResponseFiles();
        var environmentVariables = new Dictionary<string, string?>
        {
            ["Socrata__AppToken"] = "demo-token",
            ["ConnectionStrings__DemoDb"] = "Server=localhost\\DEMO;Database=DemoDb;Integrated Security=True;Connect Timeout=0;TrustServerCertificate=True;",
            ["Socrata__MockResponsePath"] = responsePattern,
        };

        var result = await runner.RunAsync(Array.Empty<string>(), [">", "<", "Q"], environmentVariables);

        Assert.Equal(0, result.ExitCode);
        Assert.Contains("Page 2", result.StandardOutput, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("TX-2", result.StandardOutput, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("TX-1", result.StandardOutput, StringComparison.OrdinalIgnoreCase);
    }

    private static string CreateResponseFiles()
    {
        var directory = Path.Combine(Path.GetTempPath(), $"paging-navigation-{Guid.NewGuid():N}");
        Directory.CreateDirectory(directory);

        File.WriteAllText(Path.Combine(directory, "page-1.json"), """
        {
          "results": [
            {
              "transactionid": "TX-1",
              "entityid": "ENT-1",
              "name": "Acme Holdings",
              "historydes": "Created",
              "receiveddate": "2026-03-15T10:45:00Z"
            }
          ]
        }
        """);

        File.WriteAllText(Path.Combine(directory, "page-2.json"), """
        {
          "results": [
            {
              "transactionid": "TX-2",
              "entityid": "ENT-2",
              "name": "Beacon Services",
              "historydes": "Updated",
              "receiveddate": "2026-03-14T09:15:00Z"
            }
          ]
        }
        """);

        return Path.Combine(directory, "page-{pageNumber}.json");
    }
}