using Screen.ViewModel;

namespace Screen.Messages;

public record ResumeClockMessage(DateTime DateTime) : IUpdateMessage
{
    public void HandleSelf(MainWindowViewModel viewModel)
    {
        viewModel.LastResumeTimestamp = DateTime;
        viewModel.Match.IsTimeStopped = false;
        viewModel.Timer.Start();
    }
}
