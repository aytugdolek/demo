using System.Xml.Linq;
using Colorado.BusinessEntityTransactionHistory.Core.Architecture;

namespace Colorado.BusinessEntityTransactionHistory.Core.UnitTests.Architecture;

public sealed class SolutionProjectTests
{
    [Fact]
    public void Project_references_follow_the_documented_dependency_direction()
    {
        var projects = new[]
        {
            new SolutionProject("Core", "src/Core/Colorado.BusinessEntityTransactionHistory.Core.csproj", "Core", "Library", [], ["Business rules"]),
            new SolutionProject("Application", "src/Application/Colorado.BusinessEntityTransactionHistory.Application.csproj", "Application", "Library", ["Core"], ["Use cases"]),
            new SolutionProject("Infrastructure", "src/Infrastructure/Colorado.BusinessEntityTransactionHistory.Infrastructure.csproj", "Infrastructure", "Library", ["Application", "Core"], ["Adapters"]),
            new SolutionProject("Cli", "src/Cli/Colorado.BusinessEntityTransactionHistory.Cli.csproj", "Cli", "Exe", ["Application", "Infrastructure"], ["Terminal entry point", "Composition root"]),
        };

        var repositoryRoot = ResolveRepositoryRoot();
        var layersByProjectName = projects.ToDictionary(project => project.Name, project => project.Layer, StringComparer.OrdinalIgnoreCase);

        foreach (var project in projects)
        {
            var fullPath = Path.Combine(repositoryRoot, project.Path.Replace('/', Path.DirectorySeparatorChar));
            var document = XDocument.Load(fullPath);
            var referenceLayers = document
                .Descendants("ProjectReference")
                .Select(reference => reference.Attribute("Include")?.Value)
                .Where(value => !string.IsNullOrWhiteSpace(value))
                .Select(value => Path.GetFileNameWithoutExtension(value!))
                .Select(GetProjectNameSuffix)
                .ToArray();

            Assert.All(referenceLayers, referenceLayer => Assert.True(project.AllowsReferenceTo(referenceLayer), $"{project.Name} should not reference {referenceLayer}."));

            var expectedReferences = project.AllowedReferences.OrderBy(value => value).ToArray();
            var actualReferences = referenceLayers.OrderBy(value => value).ToArray();
            Assert.Equal(expectedReferences, actualReferences);
        }
    }

    private static string ResolveRepositoryRoot()
    {
        var current = new DirectoryInfo(AppContext.BaseDirectory);

        while (current is not null)
        {
            if (File.Exists(Path.Combine(current.FullName, "Colorado.BusinessEntityTransactionHistory.sln")))
            {
                return current.FullName;
            }

            current = current.Parent;
        }

        throw new InvalidOperationException("Could not resolve repository root.");
    }

    private static string GetProjectNameSuffix(string projectName)
    {
        const string prefix = "Colorado.BusinessEntityTransactionHistory.";
        return projectName.StartsWith(prefix, StringComparison.Ordinal) ? projectName[prefix.Length..] : projectName;
    }
}