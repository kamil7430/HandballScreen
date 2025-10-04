using Keyboard.Model;
using Screen.ViewModel;

namespace Screen.Messages;

public record UpdateWholeMatch(Match Match) : IUpdateMessage
{
    public void HandleSelf(MainWindowViewModel viewModel)
    {
        viewModel.Match = Match;
        viewModel.DecysecondsOnLastTimeStop = Match.TimeInDecyseconds;
    }
}
