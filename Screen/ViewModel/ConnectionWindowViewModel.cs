using Keyboard.View;
using Keyboard.ViewModel;

namespace Screen.ViewModel;

public class ConnectionWindowViewModel : SettingsWindowViewModelBase
{
    public string IpAddress { get; set; }
    public string Port { get; set; }

    public ConnectionWindowViewModel(ICloseableWindow window)
        : base(window)
    {
        IpAddress = "";
        Port = "";
    }

    protected override bool ValidateFields()
    {
        throw new NotImplementedException();
    }
}
