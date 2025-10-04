using Keyboard.View;
using Keyboard.ViewModel;
using Screen.Messages;
using Screen.View;
using System.Net;
using System.Threading.Channels;
using System.Windows;

namespace Screen.ViewModel;

public class ConnectionWindowViewModel : SettingsWindowViewModelBase
{
    public string IpAddress { get; set; }
    public string Port { get; set; }
    private readonly CancellationToken _cancellationToken;
    private readonly Channel<IUpdateMessage> _channel;

    public ScreenManagerClient? Client { get; private set; }

    public ConnectionWindowViewModel(CancellationToken cancellationToken, Channel<IUpdateMessage> channel, ICloseableWindow window)
        : base(window)
    {
        IpAddress = "";
        Port = "";
        _cancellationToken = cancellationToken;
        _channel = channel;
    }

    protected override bool ValidateFields()
    {
        if (!IPAddress.TryParse(IpAddress, out var ipAddress))
        {
            MessageBox.Show("Błędny adres IP!");
            return false;
        }
        if (!int.TryParse(Port, out var port) || port < 0 || port > 65536)
        {
            MessageBox.Show("Błędny port!");
            return false;
        }

        ConnectingWindow window = new(ipAddress, port, _channel, _cancellationToken);
        var result = window.ShowDialog() ?? false;
        if (result)
        {
            Client = window.Client;
            return true;
        }

        return false;
    }
}
