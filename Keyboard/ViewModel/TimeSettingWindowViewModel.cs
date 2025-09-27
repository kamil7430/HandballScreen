using Keyboard.Model;
using Keyboard.View;
using System.Windows;

namespace Keyboard.ViewModel;

public partial class TimeSettingWindowViewModel : SettingsWindowViewModelBase
{
    public string Minutes { get; set; }
    public string Seconds { get; set; }
    public string Decyseconds { get; set; }
    public string HalfNumber { get; set; }
    public Match? NewMatch { get; private set; }
    private Match _oldMatch;

    public TimeSettingWindowViewModel(Match match, ICloseableWindow window)
        : base(window)
    {
        Minutes = (match.TimeInDecyseconds / 600).ToString();
        Seconds = (match.TimeInDecyseconds / 10 % 60).ToString();
        Decyseconds = (match.TimeInDecyseconds % 10).ToString();
        HalfNumber = match.HalfNumber.ToString();
        _oldMatch = match;
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
        if (!int.TryParse(HalfNumber, out int halfNumber) || halfNumber < 1 || halfNumber > 2)
        {
            MessageBox.Show("Nieprawidłowy numer połowy!");
            return false;
        }
        NewMatch = (Match)_oldMatch.Clone();
        NewMatch.TimeInDecyseconds = decyseconds + seconds * 10 + minutes * 600;
        NewMatch.HalfNumber = (short)halfNumber;
        return true;
    }
}
