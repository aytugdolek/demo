using Colorado.BusinessEntityTransactionHistory.Application.Configuration;
using Microsoft.Extensions.Options;

namespace Colorado.BusinessEntityTransactionHistory.Application.UnitTests.Configuration;

public sealed class RuntimeConfigurationValidatorTests
{
    [Fact]
    public void Validate_returns_failures_when_required_values_are_missing()
    {
        var validator = CreateValidator(appToken: string.Empty, connectionString: string.Empty);

        var outcomes = validator.Validate();

        Assert.Contains(outcomes, outcome => outcome.CheckName == "Socrata:AppToken" && !outcome.IsSuccess);
        Assert.Contains(outcomes, outcome => outcome.CheckName == "ConnectionStrings:DemoDb" && !outcome.IsSuccess);
    }

    [Fact]
    public void Validate_returns_failure_for_an_unparseable_connection_string()
    {
        var validator = CreateValidator(appToken: "demo-token", connectionString: "Server");

        var outcomes = validator.Validate();

        Assert.Contains(outcomes, outcome => outcome.CheckName == "ConnectionStrings:DemoDb" && !outcome.IsSuccess);
    }

    [Fact]
    public void Validate_returns_success_when_required_values_are_valid()
    {
        var validator = CreateValidator(
            appToken: "demo-token",
            connectionString: "Server=localhost\\DEMO;Database=DemoDb;Integrated Security=True;Connect Timeout=0;TrustServerCertificate=True;");

        var outcomes = validator.Validate();

        Assert.All(outcomes, outcome => Assert.True(outcome.IsSuccess));
        Assert.Contains(outcomes, outcome => outcome.CheckName == "Socrata:AppToken");
        Assert.Contains(outcomes, outcome => outcome.CheckName == "ConnectionStrings:DemoDb");
    }

    private static RuntimeConfigurationValidator CreateValidator(string appToken, string connectionString)
    {
        return new RuntimeConfigurationValidator(
            Options.Create(new SocrataOptions { AppToken = appToken }),
            Options.Create(new DemoDbOptions { ConnectionString = connectionString }));
    }
}