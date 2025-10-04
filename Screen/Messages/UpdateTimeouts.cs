using Screen.ViewModel;

namespace Screen.Messages;

public record UpdateTimeouts(short Hosts, short Guests) : IUpdateMessage
{
    public void HandleSelf(MainWindowViewModel viewModel)
    {
        viewModel.Match.HostsTimeoutsUsed = Hosts;
        viewModel.Match.GuestsTimeoutsUsed = Guests;
    }
}
