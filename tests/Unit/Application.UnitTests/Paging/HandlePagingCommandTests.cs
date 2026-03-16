using Colorado.BusinessEntityTransactionHistory.Application.Paging;

namespace Colorado.BusinessEntityTransactionHistory.Application.UnitTests.Paging;

public sealed class HandlePagingCommandTests
{
    [Fact]
    public void Exit_result_marks_the_session_for_shutdown()
    {
        var result = PagingCommandResult.Exit();

        Assert.True(result.ShouldExit);
        Assert.Null(result.StatusMessage);
    }

    [Fact]
    public void Invalid_input_result_returns_the_expected_message()
    {
        var result = PagingCommandResult.InvalidInput();

        Assert.False(result.ShouldExit);
        Assert.Equal("Unsupported input. Enter >, <, or Q.", result.StatusMessage);
    }
}