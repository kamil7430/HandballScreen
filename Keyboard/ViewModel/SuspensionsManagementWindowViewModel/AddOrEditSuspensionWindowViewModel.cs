using Keyboard.Model;
using Keyboard.View;
using System.Windows;

namespace Keyboard.ViewModel.SuspensionsManagementWindowViewModel;

public class AddOrEditSuspensionWindowViewModel : SettingsWindowViewModelBase
{
    private const long SUSPENSION_DURATION_IN_DECYSECONDS = 1200;
    public string Minutes { get; set; }
    public string Seconds { get; set; }
    public string Decyseconds { get; set; }
    public string PlayerNumber { get; set; }
    public Suspension? NewSuspension { get; private set; }

    public AddOrEditSuspensionWindowViewModel(Match match, Suspension? suspension, ICloseableWindow window)
        : base(window)
    {
        var suspensionEnd = suspension?.EndInMatchDecyseconds
            ?? match.TimeInDecyseconds + SUSPENSION_DURATION_IN_DECYSECONDS;
        Minutes = (suspensionEnd / 600).ToString();
        Seconds = (suspensionEnd / 10 % 60).ToString();
        Decyseconds = (suspensionEnd % 10).ToString();
        PlayerNumber = suspension?.PlayerNumber.ToString() ?? "";
    }

    protected override bool ValidateFields()
    {
        if (!int.TryParse(Minutes, out int minutes) || minutes < 0)
        {
            MessageBox.Show("Nieprawidłowa wartość w polu minut!");
            return false;
        }
        if (!int.TryParse(Seconds, out int seconds) || seconds < 0 || seconds >= 60)
        {
            MessageBox.Show("Nieprawidłowa wartość w polu sekund!");
            return false;
        }
        if (!int.TryParse(Decyseconds, out int decyseconds) || decyseconds < 0 || decyseconds >= 10)
        {
            MessageBox.Show("Nieprawidłowa wartość w polu decysekund!");
            return false;
        }
        if (!int.TryParse(PlayerNumber, out int playerNumber) || playerNumber < 0)
        {
            MessageBox.Show("Nieprawidłowy numer zawodnika!");
            return false;
        }
        NewSuspension = new Suspension(playerNumber, decyseconds + 10 * seconds + 600 * minutes);
        return true;
    }
}
