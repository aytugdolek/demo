namespace Colorado.BusinessEntityTransactionHistory.Application.Startup;

public sealed class GetBaselineStartupSummary
{
    public BaselineStartupSummary Execute()
    {
        return new BaselineStartupSummary(
            "Baseline ready",
            "The Clean Architecture solution skeleton is configured and ready for the next feature slices.",
            "Next actions: run with --validate-config to verify local configuration or --help to review the baseline CLI options.");
    }
}