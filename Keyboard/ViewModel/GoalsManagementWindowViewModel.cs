using Keyboard.Model;
using Keyboard.View;
using System.Windows;

namespace Keyboard.ViewModel;

public class GoalsManagementWindowViewModel : SettingsWindowViewModelBase
{
    public string HostsGoals { get; set; }
    public string GuestsGoals { get; set; }
    public Match NewMatch { get; private set; }

    public GoalsManagementWindowViewModel(Match match, ICloseableWindow window)
        : base(window)
    {
        HostsGoals = match.HostsPoints.ToString();
        GuestsGoals = match.GuestsPoints.ToString();
        NewMatch = (Match)match.Clone();
    }

    protected override bool ValidateFields()
    {
        if (!short.TryParse(HostsGoals, out short hostsGoals))
        {
            MessageBox.Show("Nieprawidłowa wartość w polu goli gospodarzy!");
            return false;
        }
        if (!short.TryParse(GuestsGoals, out short guestsGoals))
        {
            MessageBox.Show("Nieprawidłowa wartość w polu goli gości!");
            return false;
        }
        NewMatch.HostsPoints = hostsGoals;
        NewMatch.GuestsPoints = guestsGoals;
        return true;
    }
}
