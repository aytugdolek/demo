using Colorado.BusinessEntityTransactionHistory.Application.Startup;

namespace Colorado.BusinessEntityTransactionHistory.Application.UnitTests.Startup;

public sealed class GetBaselineStartupSummaryTests
{
    [Fact]
    public void Execute_returns_the_expected_baseline_summary()
    {
        var useCase = new GetBaselineStartupSummary();

        var summary = useCase.Execute();

        Assert.Equal("Baseline ready", summary.Title);
        Assert.Contains("Clean Architecture", summary.Description, StringComparison.Ordinal);
        Assert.Contains("--validate-config", summary.NextActions);
        Assert.Contains("--help", summary.NextActions);
    }
}