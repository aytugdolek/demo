using Colorado.BusinessEntityTransactionHistory.Application.Configuration;

namespace Colorado.BusinessEntityTransactionHistory.Application.UnitTests.Configuration;

public sealed class ValidationOutcomeTests
{
    [Fact]
    public void Success_creates_an_informational_result()
    {
        var outcome = ValidationOutcome.Success("Socrata:AppToken", "Configuration value is present.");

        Assert.Equal("Socrata:AppToken", outcome.CheckName);
        Assert.True(outcome.IsSuccess);
        Assert.Equal(ValidationSeverity.Info, outcome.Severity);
    }

    [Fact]
    public void Error_creates_a_failure_result()
    {
        var outcome = ValidationOutcome.Error("ConnectionStrings:DemoDb", "Configuration value is missing.");

        Assert.False(outcome.IsSuccess);
        Assert.Equal(ValidationSeverity.Error, outcome.Severity);
        Assert.Equal("Configuration value is missing.", outcome.Message);
    }
}