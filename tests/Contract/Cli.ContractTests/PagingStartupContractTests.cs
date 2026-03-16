using Colorado.BusinessEntityTransactionHistory.Cli.ContractTests.TestSupport;

namespace Colorado.BusinessEntityTransactionHistory.Cli.ContractTests;

public sealed class PagingStartupContractTests
{
    [Fact]
    public async Task No_arguments_start_the_paging_session_and_render_the_first_page()
    {
        var runner = new CliCommandRunner();
        var appToken = Guid.NewGuid().ToString("N");
        var responseFile = CreateResponseFile();
        var environmentVariables = new Dictionary<string, string?>
        {
            ["Socrata__AppToken"] = appToken,
            ["ConnectionStrings__DemoDb"] = "Server=localhost\\DEMO;Database=DemoDb;Integrated Security=True;Connect Timeout=0;TrustServerCertificate=True;",
            ["Socrata__MockResponsePath"] = responseFile,
        };

        var result = await runner.RunAsync(Array.Empty<string>(), ["Q"], environmentVariables);

        Assert.Equal(0, result.ExitCode);
        Assert.Contains("Page 1", result.StandardOutput, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("Transaction ID", result.StandardOutput, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("Baseline ready", result.StandardOutput, StringComparison.OrdinalIgnoreCase);
    }

        private static string CreateResponseFile()
        {
                var path = Path.Combine(Path.GetTempPath(), $"paging-startup-{Guid.NewGuid():N}.json");
                File.WriteAllText(path, """
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

                return path;
        }
}