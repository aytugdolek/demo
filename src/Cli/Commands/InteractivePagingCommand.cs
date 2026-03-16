using Colorado.BusinessEntityTransactionHistory.Application.Configuration;
using Colorado.BusinessEntityTransactionHistory.Application.Paging;
using Microsoft.Extensions.Options;
using Spectre.Console;

namespace Colorado.BusinessEntityTransactionHistory.Cli.Commands;

public sealed class InteractivePagingCommand
{
    private readonly GetTransactionHistoryPage _getTransactionHistoryPage;
    private readonly NavigationCommandParser _navigationCommandParser;
    private readonly PagingSessionController _pagingSessionController;
    private readonly TransactionHistoryTableRenderer _transactionHistoryTableRenderer;
    private readonly IOptions<SocrataOptions> _socrataOptions;

    public InteractivePagingCommand(
        GetTransactionHistoryPage getTransactionHistoryPage,
        NavigationCommandParser navigationCommandParser,
        PagingSessionController pagingSessionController,
        TransactionHistoryTableRenderer transactionHistoryTableRenderer,
        IOptions<SocrataOptions> socrataOptions)
    {
        _getTransactionHistoryPage = getTransactionHistoryPage;
        _navigationCommandParser = navigationCommandParser;
        _pagingSessionController = pagingSessionController;
        _transactionHistoryTableRenderer = transactionHistoryTableRenderer;
        _socrataOptions = socrataOptions;
    }

    public async Task<int> ExecuteAsync(CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(_socrataOptions.Value.AppToken))
        {
            var result = PagingCommandResult.FromFailure("Unable to start paging because Socrata:AppToken is missing.");
            AnsiConsole.MarkupLine($"[red]{Markup.Escape(result.Failure!.Message)}[/]");
            return 1;
        }

        TransactionHistoryPage currentPage;

        try
        {
            currentPage = await _getTransactionHistoryPage.ExecuteAsync(1, cancellationToken);
        }
        catch (Exception exception)
        {
            var result = PagingCommandResult.FromFailure(exception.Message, exception);
            AnsiConsole.MarkupLine($"[red]{Markup.Escape(result.Failure!.Message)}[/]");
            return 1;
        }

        _transactionHistoryTableRenderer.Render(currentPage);
        var currentState = _pagingSessionController.Initialize(currentPage);

        while (true)
        {
            AnsiConsole.Markup("[grey]Enter > for next page, < for previous page, or Q to quit:[/] ");
            var command = _navigationCommandParser.Parse(Console.ReadLine());

            if (command.Kind == NavigationCommandKind.Quit)
            {
                _ = PagingCommandResult.Exit();
                return 0;
            }

            if (command.Kind == NavigationCommandKind.Previous)
            {
                if (currentState.CurrentPageNumber == 1)
                {
                    currentState = _pagingSessionController.Apply(currentState, command);
                    var result = PagingCommandResult.Status(currentState.StatusMessage ?? string.Empty);
                    AnsiConsole.MarkupLine($"[yellow]{Markup.Escape(result.StatusMessage ?? string.Empty)}[/]");
                    continue;
                }

                currentPage = await _getTransactionHistoryPage.ExecuteAsync(currentState.CurrentPageNumber - 1, cancellationToken);
                currentState = _pagingSessionController.Apply(currentState, command, currentPage);
                _transactionHistoryTableRenderer.Render(currentPage);
                continue;
            }

            if (command.Kind == NavigationCommandKind.Next)
            {
                var nextPage = await _getTransactionHistoryPage.ExecuteAsync(currentState.CurrentPageNumber + 1, cancellationToken);
                currentState = _pagingSessionController.Apply(currentState, command, nextPage);
                if (nextPage.IsEmpty)
                {
                    var result = PagingCommandResult.Status(currentState.StatusMessage ?? string.Empty);
                    AnsiConsole.MarkupLine($"[yellow]{Markup.Escape(result.StatusMessage ?? string.Empty)}[/]");
                    continue;
                }

                currentPage = nextPage;
                _transactionHistoryTableRenderer.Render(currentPage);
                continue;
            }

            currentState = _pagingSessionController.Apply(currentState, command);
            var invalidResult = PagingCommandResult.InvalidInput();
            AnsiConsole.MarkupLine($"[yellow]{Markup.Escape(invalidResult.StatusMessage ?? string.Empty)}[/]");
        }
    }
}