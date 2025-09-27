using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Keyboard.View;

namespace Keyboard.ViewModel;

public abstract partial class SettingsWindowViewModelBase : ObservableObject
{
    protected readonly ICloseableWindow _window;

    public SettingsWindowViewModelBase(ICloseableWindow window)
    {
        _window = window;
    }

    protected abstract bool ValidateFields();

    [RelayCommand]
    protected virtual void CancelClick()
        => _window.CloseWindow(false);

    [RelayCommand]
    protected virtual void OKClick()
    {
        if (ValidateFields())
            _window.CloseWindow(true);
    }
}
