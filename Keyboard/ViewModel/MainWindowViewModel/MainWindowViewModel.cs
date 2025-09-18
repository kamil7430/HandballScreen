using CommunityToolkit.Mvvm.ComponentModel;
using Keyboard.Model;
using System.ComponentModel;

namespace Keyboard.ViewModel;

public partial class MainWindowViewModel : ObservableObject
{
    private Match? _match;
    public Match Match
    {
        get => _match!;
        set
        {
            if (_match != null)
                _match.PropertyChanged -= OnMatchPropertyChanged;
            SetProperty(ref _match, value);
            if (_match != null)
                _match.PropertyChanged += OnMatchPropertyChanged;
        }
    }

    public MainWindowViewModel()
    {
        Match = new Match();
    }

    private void OnMatchPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        NotifyCommandsCanExecute();
    }
}
