using Keyboard.Model;
using System.Windows.Input;

namespace Keyboard.ViewModel;

public class MainWindowViewModel : NotifyPropertyChangedAbstract
{
    public Match Match { get; protected set; }

    public MainWindowViewModel()
    {
        Match = new Match();
    }

    public ICommand AddHostsGoalCommand { get; }
    public ICommand RemoveHostsGoalCommand { get; }
    public ICommand ManageHostsGoalsCommand { get; }
    public ICommand AddUsedHostsTimeoutCommand { get; }
    public ICommand RemoveUsedHostsTimeoutCommand { get; }
    public ICommand ManageHostsSuspensionsCommand { get; }
    public ICommand ResumeMatchClockCommand { get; }
    public ICommand UseSoundEffectCommand { get; }
    public ICommand StopMatchClockCommand { get; }
    public ICommand AddGuestsGoalCommand { get; }
    public ICommand RemoveGuestsGoalCommand { get; }
    public ICommand ManageGuestsGoalsCommand { get; }
    public ICommand AddUsedGuestsTimeoutCommand { get; }
    public ICommand RemoveUsedGuestsTimeoutCommand { get; }
    public ICommand ManageGuestsSuspensionsCommand { get; }
}
