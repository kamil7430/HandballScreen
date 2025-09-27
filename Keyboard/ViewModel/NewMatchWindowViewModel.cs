using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Keyboard.Model;
using Keyboard.View;
using System.Windows;

namespace Keyboard.ViewModel;

public partial class NewMatchWindowViewModel : ObservableObject
{
    public string Minutes { get; set; }
    public string Seconds { get; set; }
    public string MaxTimeouts { get; set; }
    private readonly ICloseableWindow _window;
    public Match? NewMatch { get; private set; }

    public NewMatchWindowViewModel(Match match, ICloseableWindow window)
    {
        Minutes = (match.MaxMatchTimeInDecyseconds / 10 / 60).ToString();
        Seconds = (match.MaxMatchTimeInDecyseconds / 10 % 60).ToString();
        MaxTimeouts = match.MaxTimeouts.ToString();
        _window = window;
        NewMatch = null;
    }

    private bool ValidateFields()
    {
        if (!int.TryParse(Minutes, out int minutes) || minutes < 0)
        {
            MessageBox.Show("Nieprawidłowa wartość w polu minut!");
            return false;
        }
        if (!int.TryParse(Seconds, out int seconds) || seconds < 0 || seconds >= 60
            || (seconds == 0 && minutes == 0))
        {
            MessageBox.Show("Nieprawidłowa wartość w polu sekund!");
            return false;
        }
        if (!int.TryParse(MaxTimeouts, out int maxTimeouts) || maxTimeouts <= 0)
        {
            MessageBox.Show("Nieprawidłowa wartość w polu czasów!");
            return false;
        }
        NewMatch = new Match((short)maxTimeouts, (minutes * 600) + seconds * 10);
        return true;
    }

    [RelayCommand]
    private void CancelClick()
        => _window.CloseWindow(false);

    [RelayCommand]
    private void OKClick()
    {
        if (ValidateFields())
            _window.CloseWindow(true);
    }
}
