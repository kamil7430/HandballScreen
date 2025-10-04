using Screen.ViewModel;

namespace Screen.Messages;

public record StopClockMessage(long DecysecondsOnClock) : IUpdateMessage
{
    public void HandleSelf(MainWindowViewModel viewModel)
    {
        viewModel.Match.TimeInDecyseconds = viewModel.DecysecondsOnLastTimeStop = DecysecondsOnClock;
        // TODO: zegar
    }
}
