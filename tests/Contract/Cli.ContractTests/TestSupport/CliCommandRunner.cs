using System.Diagnostics;
using System.Text;

namespace Colorado.BusinessEntityTransactionHistory.Cli.ContractTests.TestSupport;

internal sealed class CliCommandRunner
{
    private readonly string _repositoryRoot;

    public CliCommandRunner()
    {
        _repositoryRoot = ResolveRepositoryRoot();
    }

    public async Task<CliCommandResult> RunAsync(
        IReadOnlyCollection<string> arguments,
        IReadOnlyDictionary<string, string?>? environmentVariables = null,
        string? workingDirectory = null,
        CancellationToken cancellationToken = default)
    {
        return await RunAsync(arguments, null, environmentVariables, workingDirectory, cancellationToken);
    }

    public async Task<CliCommandResult> RunAsync(
        IReadOnlyCollection<string> arguments,
        IReadOnlyCollection<string>? standardInputLines = null,
        IReadOnlyDictionary<string, string?>? environmentVariables = null,
        string? workingDirectory = null,
        CancellationToken cancellationToken = default)
    {
        var outputBuilder = new StringBuilder();
        var errorBuilder = new StringBuilder();

        using var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = $"run --project \"{Path.Combine(_repositoryRoot, "src", "Cli", "Colorado.BusinessEntityTransactionHistory.Cli.csproj")}\" -- {string.Join(' ', arguments)}",
                WorkingDirectory = workingDirectory ?? _repositoryRoot,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                RedirectStandardInput = standardInputLines is not null,
                UseShellExecute = false,
            },
            EnableRaisingEvents = true,
        };

        if (environmentVariables is not null)
        {
            foreach (var entry in environmentVariables)
            {
                if (entry.Value is null)
                {
                    process.StartInfo.Environment.Remove(entry.Key);
                    continue;
                }

                process.StartInfo.Environment[entry.Key] = entry.Value;
            }
        }

        process.OutputDataReceived += (_, eventArgs) =>
        {
            if (eventArgs.Data is not null)
            {
                outputBuilder.AppendLine(eventArgs.Data);
            }
        };

        process.ErrorDataReceived += (_, eventArgs) =>
        {
            if (eventArgs.Data is not null)
            {
                errorBuilder.AppendLine(eventArgs.Data);
            }
        };

        process.Start();
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();

        if (standardInputLines is not null)
        {
            foreach (var line in standardInputLines)
            {
                await process.StandardInput.WriteLineAsync(line.AsMemory(), cancellationToken);
            }

            process.StandardInput.Close();
        }

        await process.WaitForExitAsync(cancellationToken);

        return new CliCommandResult(
            process.ExitCode,
            outputBuilder.ToString(),
            errorBuilder.ToString());
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
}

internal sealed record CliCommandResult(int ExitCode, string StandardOutput, string StandardError);