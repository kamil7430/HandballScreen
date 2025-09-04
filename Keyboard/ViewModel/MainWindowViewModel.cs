using Keyboard.Model;

namespace Keyboard.ViewModel;

public class MainWindowViewModel : NotifyPropertyChangedAbstract
{
    public Match Match { get; protected set; }

    public MainWindowViewModel()
    {
        Match = new Match();
    }
}
