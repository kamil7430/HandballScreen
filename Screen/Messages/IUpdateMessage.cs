using Screen.ViewModel;
using System.Text.Json.Serialization;

namespace Screen.Messages;

[JsonDerivedType(typeof(ResumeClockMessage), 1)]
[JsonDerivedType(typeof(StopClockMessage), 2)]
[JsonDerivedType(typeof(UpdateScore), 3)]
[JsonDerivedType(typeof(UpdateTimeouts), 4)]
[JsonDerivedType(typeof(UpdateWholeMatch), 5)]
[JsonDerivedType(typeof(UseSoundEffectMessage), 6)]
public interface IUpdateMessage
{
    public void HandleSelf(MainWindowViewModel viewModel);
}
