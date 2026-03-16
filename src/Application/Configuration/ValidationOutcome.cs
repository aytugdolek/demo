namespace Colorado.BusinessEntityTransactionHistory.Application.Configuration;

public enum ValidationSeverity
{
    Info,
    Warning,
    Error,
}

public sealed record ValidationOutcome(string CheckName, bool IsSuccess, ValidationSeverity Severity, string Message)
{
    public static ValidationOutcome Success(string checkName, string message)
    {
        return new ValidationOutcome(checkName, true, ValidationSeverity.Info, message);
    }

    public static ValidationOutcome Warning(string checkName, string message)
    {
        return new ValidationOutcome(checkName, true, ValidationSeverity.Warning, message);
    }

    public static ValidationOutcome Error(string checkName, string message)
    {
        return new ValidationOutcome(checkName, false, ValidationSeverity.Error, message);
    }
}