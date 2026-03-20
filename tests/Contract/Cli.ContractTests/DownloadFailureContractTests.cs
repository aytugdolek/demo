using Colorado.BusinessEntityTransactionHistory.Cli.ContractTests.TestSupport;

namespace Colorado.BusinessEntityTransactionHistory.Cli.ContractTests;

public sealed class DownloadFailureContractTests
{
    [Fact]
    public async Task Existing_target_file_returns_a_clear_failure_without_overwriting_it()
    {
        var runner = new CliCommandRunner();
        var workingDirectory = Path.Combine(Path.GetTempPath(), $"download-existing-file-{Guid.NewGuid():N}");
        var downloadDirectory = Path.Combine(workingDirectory, "downloads");
        Directory.CreateDirectory(downloadDirectory);

        var existingFile = Path.Combine(downloadDirectory, "casm-dbbj-query.json");
        await File.WriteAllTextAsync(existingFile, "existing-content");

        var mockResponsePath = Path.Combine(workingDirectory, "mock-response.json");
        await File.WriteAllTextAsync(mockResponsePath, "{\"results\":[]}");

        var environmentVariables = new Dictionary<string, string?>
        {
            ["Socrata__AppToken"] = Guid.NewGuid().ToString("N"),
            ["Socrata__MockResponsePath"] = mockResponsePath,
            ["ConnectionStrings__DemoDb"] = "Server=localhost\\DEMO;Database=DemoDb;Integrated Security=True;Connect Timeout=0;TrustServerCertificate=True;",
        };

        var result = await runner.RunAsync(["--download"], environmentVariables, workingDirectory);

        Assert.NotEqual(0, result.ExitCode);
        Assert.Contains("already exists", result.StandardOutput, StringComparison.OrdinalIgnoreCase);
        Assert.Equal("existing-content", await File.ReadAllTextAsync(existingFile));
    }

    [Fact]
    public async Task Missing_app_token_returns_a_clear_failure_message()
    {
        var runner = new CliCommandRunner();
        var workingDirectory = Path.Combine(Path.GetTempPath(), $"download-missing-token-{Guid.NewGuid():N}");
        Directory.CreateDirectory(workingDirectory);

        var environmentVariables = new Dictionary<string, string?>
        {
            ["Socrata__AppToken"] = null,
            ["Socrata__MockResponsePath"] = string.Empty,
            ["ConnectionStrings__DemoDb"] = "Server=localhost\\DEMO;Database=DemoDb;Integrated Security=True;Connect Timeout=0;TrustServerCertificate=True;",
        };

        var result = await runner.RunAsync(["--download"], environmentVariables, workingDirectory);

        Assert.NotEqual(0, result.ExitCode);
        Assert.Contains("Socrata:AppToken", result.StandardOutput, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("Enter > for next page", result.StandardOutput, StringComparison.OrdinalIgnoreCase);
    }
}