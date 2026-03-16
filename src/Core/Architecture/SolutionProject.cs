namespace Colorado.BusinessEntityTransactionHistory.Core.Architecture;

public sealed record SolutionProject(
    string Name,
    string Path,
    string Layer,
    string OutputType,
    IReadOnlyCollection<string> AllowedReferences,
    IReadOnlyCollection<string> Responsibilities)
{
    public bool AllowsReferenceTo(string referencedLayer)
    {
        return AllowedReferences.Contains(referencedLayer, StringComparer.Ordinal);
    }
}