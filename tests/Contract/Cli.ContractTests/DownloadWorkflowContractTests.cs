using Colorado.BusinessEntityTransactionHistory.Cli.ContractTests.TestSupport;

namespace Colorado.BusinessEntityTransactionHistory.Cli.ContractTests;

public sealed class DownloadWorkflowContractTests
{
    [Fact]
    public async Task Download_argument_saves_a_local_file_without_entering_paging_mode()
    {
        var runner = new CliCommandRunner();
        var workingDirectory = Path.Combine(Path.GetTempPath(), $"download-success-{Guid.NewGuid():N}");
        Directory.CreateDirectory(workingDirectory);

        var mockResponsePath = Path.Combine(workingDirectory, "mock-response.json");
        var payload = """
        {
          "results": [
            {
              "transactionid": "TX-1",
              "entityid": "ENT-1"
            }
          ]
        }
        """;
        await File.WriteAllTextAsync(mockResponsePath, payload);

        var environmentVariables = new Dictionary<string, string?>
        {
            ["Socrata__AppToken"] = Guid.NewGuid().ToString("N"),
            ["Socrata__MockResponsePath"] = mockResponsePath,
            ["ConnectionStrings__DemoDb"] = "Server=localhost\\DEMO;Database=DemoDb;Integrated Security=True;Connect Timeout=0;TrustServerCertificate=True;",
        };

        var result = await runner.RunAsync(["--download"], environmentVariables, workingDirectory);

        var outputFile = Path.Combine(workingDirectory, "downloads", "casm-dbbj-query.json");

        Assert.Equal(0, result.ExitCode);
        Assert.Contains("Download completed successfully", result.StandardOutput, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("Enter > for next page", result.StandardOutput, StringComparison.OrdinalIgnoreCase);
        Assert.True(File.Exists(outputFile));
        Assert.Equal(NormalizeLineEndings(payload), NormalizeLineEndings(await File.ReadAllTextAsync(outputFile)));
    }

      private static string NormalizeLineEndings(string value)
      {
        return value.Replace("\r\n", "\n", StringComparison.Ordinal).Replace("\r", "\n", StringComparison.Ordinal);
      }
}