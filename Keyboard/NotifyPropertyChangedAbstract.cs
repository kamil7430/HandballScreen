using System.ComponentModel;

namespace Keyboard;

public abstract class NotifyPropertyChangedAbstract : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged(string property)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
}
