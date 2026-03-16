using Colorado.BusinessEntityTransactionHistory.Cli.ContractTests.TestSupport;

namespace Colorado.BusinessEntityTransactionHistory.Cli.ContractTests;

public sealed class BaselineStartupContractTests
{
    [Fact]
    public async Task No_arguments_prints_the_baseline_ready_summary()
    {
        var runner = new CliCommandRunner();

        var result = await runner.RunAsync(Array.Empty<string>());

        Assert.Equal(0, result.ExitCode);
        Assert.Contains("Baseline ready", result.StandardOutput, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("--validate-config", result.StandardOutput, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("Unhandled exception", result.StandardError, StringComparison.OrdinalIgnoreCase);
    }
}