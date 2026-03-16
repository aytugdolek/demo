using Microsoft.Extensions.Options;
using Microsoft.Data.SqlClient;

namespace Colorado.BusinessEntityTransactionHistory.Application.Configuration;

public sealed class RuntimeConfigurationValidator
{
    private readonly IOptions<SocrataOptions> _socrataOptions;
    private readonly IOptions<DemoDbOptions> _demoDbOptions;

    public RuntimeConfigurationValidator(IOptions<SocrataOptions> socrataOptions, IOptions<DemoDbOptions> demoDbOptions)
    {
        _socrataOptions = socrataOptions;
        _demoDbOptions = demoDbOptions;
    }

    public IReadOnlyList<ValidationOutcome> Validate()
    {
        var outcomes = new List<ValidationOutcome>();

        if (string.IsNullOrWhiteSpace(_socrataOptions.Value.AppToken))
        {
            outcomes.Add(ValidationOutcome.Error("Socrata:AppToken", "Required configuration value is missing."));
        }
        else
        {
            outcomes.Add(ValidationOutcome.Success("Socrata:AppToken", "Configuration value is present."));
        }

        if (string.IsNullOrWhiteSpace(_demoDbOptions.Value.ConnectionString))
        {
            outcomes.Add(ValidationOutcome.Error("ConnectionStrings:DemoDb", "Required configuration value is missing."));
            return outcomes;
        }

        try
        {
            _ = new SqlConnectionStringBuilder(_demoDbOptions.Value.ConnectionString);
            outcomes.Add(ValidationOutcome.Success("ConnectionStrings:DemoDb", "Configuration value is present and parseable."));
        }
        catch (ArgumentException)
        {
            outcomes.Add(ValidationOutcome.Error("ConnectionStrings:DemoDb", "Configuration value is not a valid SQL Server connection string."));
        }

        return outcomes;
    }
}