using Colorado.BusinessEntityTransactionHistory.Cli.ContractTests.TestSupport;

namespace Colorado.BusinessEntityTransactionHistory.Cli.ContractTests;

public sealed class ValidateConfigContractTests
{
    [Fact]
    public async Task Validate_config_returns_success_when_required_values_are_present()
    {
        var runner = new CliCommandRunner();
        var environmentVariables = new Dictionary<string, string?>
        {
            ["Socrata__AppToken"] = "demo-token",
            ["ConnectionStrings__DemoDb"] = "Server=localhost\\DEMO;Database=DemoDb;Integrated Security=True;Connect Timeout=0;TrustServerCertificate=True;",
        };

        var result = await runner.RunAsync(["--validate-config"], environmentVariables);

        Assert.Equal(0, result.ExitCode);
        Assert.Contains("Configuration validation succeeded", result.StandardOutput, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task Validate_config_returns_failure_when_required_values_are_missing()
    {
        var runner = new CliCommandRunner();
        var environmentVariables = new Dictionary<string, string?>
        {
            ["Socrata__AppToken"] = null,
            ["ConnectionStrings__DemoDb"] = null,
        };

        var result = await runner.RunAsync(["--validate-config"], environmentVariables);

        Assert.NotEqual(0, result.ExitCode);
        Assert.Contains("Socrata:AppToken", result.StandardOutput, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("ConnectionStrings:DemoDb", result.StandardOutput, StringComparison.OrdinalIgnoreCase);
    }
}