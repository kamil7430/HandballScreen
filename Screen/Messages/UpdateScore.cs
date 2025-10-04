using Screen.ViewModel;

namespace Screen.Messages;

public record UpdateScore(int Hosts, int Guests) : IUpdateMessage
{
    public void HandleSelf(MainWindowViewModel viewModel)
    {
        viewModel.Match.HostsPoints = (short)Hosts;
        viewModel.Match.GuestsPoints = (short)Guests;
    }
}
