using CommunityToolkit.Mvvm.ComponentModel;
using Keyboard.Model;

namespace Screen.ViewModel;

public class MainWindowViewModel : ObservableObject
{
    public Match Match { get; set; }

    public MainWindowViewModel()
    {
        Match = new Match();
    }
}
