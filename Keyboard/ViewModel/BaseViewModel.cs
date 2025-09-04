using System.ComponentModel;

namespace Keyboard.ViewModel;

public abstract class BaseViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged(string property)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
}
