using Colorado.BusinessEntityTransactionHistory.Application.Paging;

namespace Colorado.BusinessEntityTransactionHistory.Application.UnitTests.Paging;

public sealed class PagingSessionStateTests
{
    [Fact]
    public void Move_previous_from_page_one_keeps_the_session_on_page_one()
    {
        var controller = new PagingSessionController();
        var state = controller.Initialize(new TransactionHistoryPage(1, 20, []));

        var nextState = controller.Apply(state, new NavigationCommand("<", NavigationCommandKind.Previous));

        Assert.Equal(1, nextState.CurrentPageNumber);
        Assert.Equal("No earlier page is available.", nextState.StatusMessage);
    }

    [Fact]
    public void Move_next_updates_the_current_page_when_results_are_available()
    {
        var controller = new PagingSessionController();
        var currentState = controller.Initialize(new TransactionHistoryPage(1, 20, []));
        var loadedPage = new TransactionHistoryPage(2, 20, [new RemoteTransactionHistoryRecord("ENT-2", "TX-2", "Beacon", "Updated", null, null, null)]);

        var nextState = controller.Apply(currentState, new NavigationCommand(">", NavigationCommandKind.Next), loadedPage);

        Assert.Equal(2, nextState.CurrentPageNumber);
        Assert.Null(nextState.StatusMessage);
    }
}