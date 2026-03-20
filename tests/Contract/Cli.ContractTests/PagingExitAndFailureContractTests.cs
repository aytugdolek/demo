using Colorado.BusinessEntityTransactionHistory.Cli.ContractTests.TestSupport;

namespace Colorado.BusinessEntityTransactionHistory.Cli.ContractTests;

public sealed class PagingExitAndFailureContractTests
{
    [Fact]
    public async Task Quit_input_exits_cleanly_from_an_active_session()
    {
        var runner = new CliCommandRunner();
    var appToken = Guid.NewGuid().ToString("N");
        var responseFile = Path.Combine(Path.GetTempPath(), $"paging-quit-{Guid.NewGuid():N}.json");
        File.WriteAllText(responseFile, """
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

        var environmentVariables = new Dictionary<string, string?>
        {
      ["Socrata__AppToken"] = appToken,
            ["ConnectionStrings__DemoDb"] = "Server=localhost\\DEMO;Database=DemoDb;Integrated Security=True;Connect Timeout=0;TrustServerCertificate=True;",
            ["Socrata__MockResponsePath"] = responseFile,
        };

        var result = await runner.RunAsync(Array.Empty<string>(), ["Q"], environmentVariables);

        Assert.Equal(0, result.ExitCode);
        Assert.DoesNotContain("Unhandled exception", result.StandardError, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task Missing_app_token_returns_a_clear_failure_message()
    {
        var runner = new CliCommandRunner();
        var environmentVariables = new Dictionary<string, string?>
        {
            ["Socrata__AppToken"] = null,
            ["ConnectionStrings__DemoDb"] = "Server=localhost\\DEMO;Database=DemoDb;Integrated Security=True;Connect Timeout=0;TrustServerCertificate=True;",
        };

        var result = await runner.RunAsync(Array.Empty<string>(), environmentVariables);

        Assert.NotEqual(0, result.ExitCode);
        Assert.Contains("Socrata:AppToken", result.StandardOutput, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task No_argument_startup_remains_the_paging_flow_even_when_download_mode_exists()
    {
        var runner = new CliCommandRunner();
        var appToken = Guid.NewGuid().ToString("N");
        var responseFile = Path.Combine(Path.GetTempPath(), $"paging-regression-{Guid.NewGuid():N}.json");
        File.WriteAllText(responseFile, """
        {
          "results": [
            {
              "transactionid": "TX-99",
              "entityid": "ENT-99",
              "name": "Regression Check",
              "historydes": "Verified",
              "receiveddate": "2026-03-15T10:45:00Z"
            }
          ]
        }
        """);

        var environmentVariables = new Dictionary<string, string?>
        {
            ["Socrata__AppToken"] = appToken,
            ["ConnectionStrings__DemoDb"] = "Server=localhost\\DEMO;Database=DemoDb;Integrated Security=True;Connect Timeout=0;TrustServerCertificate=True;",
            ["Socrata__MockResponsePath"] = responseFile,
        };

        var result = await runner.RunAsync(Array.Empty<string>(), ["Q"], environmentVariables);

        Assert.Equal(0, result.ExitCode);
        Assert.Contains("Page 1", result.StandardOutput, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("Download completed successfully", result.StandardOutput, StringComparison.OrdinalIgnoreCase);
    }
}