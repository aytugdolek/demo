using Colorado.BusinessEntityTransactionHistory.Application.Downloads;
using Spectre.Console;

namespace Colorado.BusinessEntityTransactionHistory.Cli.Commands;

public sealed class DownloadCommand
{
    private readonly DownloadOutputFormatter _downloadOutputFormatter;
    private readonly DownloadWorkflow _downloadWorkflow;

    public DownloadCommand(DownloadWorkflow downloadWorkflow, DownloadOutputFormatter downloadOutputFormatter)
    {
        _downloadWorkflow = downloadWorkflow;
        _downloadOutputFormatter = downloadOutputFormatter;
    }

    public async Task<int> ExecuteAsync(CancellationToken cancellationToken = default)
    {
        var result = await AnsiConsole.Progress()
            .AutoClear(false)
            .HideCompleted(false)
            .Columns(
                new TaskDescriptionColumn(),
                new SpinnerColumn())
            .StartAsync(async context =>
            {
                var progressTask = context.AddTask("[green]Downloading transaction history[/]", maxValue: 1);
                progressTask.IsIndeterminate = true;
                var progress = new Progress<DownloadProgressUpdate>(update => UpdateProgressTask(progressTask, update));

                try
                {
                    var downloadResult = await _downloadWorkflow.ExecuteAsync(Environment.CurrentDirectory, progress, cancellationToken);
                    progressTask.IsIndeterminate = false;
                    progressTask.Value = progressTask.MaxValue;
                    return downloadResult;
                }
                finally
                {
                    progressTask.StopTask();
                }
            });

        _downloadOutputFormatter.Write(result);
        return result.Success ? 0 : 1;
    }

    private static void UpdateProgressTask(ProgressTask progressTask, DownloadProgressUpdate update)
    {
        progressTask.Description = update.TotalBytes is > 0
            ? $"[green]Downloading transaction history ({FormatBytes(update.BytesTransferred)} / {FormatBytes(update.TotalBytes.Value)})[/]"
            : $"[green]Downloading transaction history ({FormatBytes(update.BytesTransferred)})[/]";

        if (update.TotalBytes is > 0)
        {
            progressTask.IsIndeterminate = false;
            progressTask.MaxValue = update.TotalBytes.Value;
            progressTask.Value = Math.Min(update.BytesTransferred, update.TotalBytes.Value);
            return;
        }

        progressTask.IsIndeterminate = true;
    }

    private static string FormatBytes(long bytes)
    {
        string[] suffixes = ["B", "KB", "MB", "GB", "TB"];
        var value = (double)bytes;
        var suffixIndex = 0;

        while (value >= 1024 && suffixIndex < suffixes.Length - 1)
        {
            value /= 1024;
            suffixIndex++;
        }

        return $"{value:0.##} {suffixes[suffixIndex]}";
    }
}