using Screen.ViewModel;
using System.Media;

namespace Screen.Messages;

public record UseSoundEffectMessage : IUpdateMessage
{
    public void HandleSelf(MainWindowViewModel viewModel)
        => SystemSounds.Beep.Play();
}
