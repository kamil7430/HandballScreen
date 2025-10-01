using CommunityToolkit.Mvvm.ComponentModel;
using Keyboard.Model;
using Keyboard.Service.TcpMessages;
using Screen.View;
using System.Threading.Channels;
using System.Windows;

namespace Screen.ViewModel;

public class MainWindowViewModel : ObservableObject
{
    public Match Match { get; set; }
    public ScreenManagerClient? Client { get; private set; }
    private readonly Channel<IUpdateMessage> _channel;
    private readonly CancellationTokenSource _cts;

    public MainWindowViewModel()
    {
        Match = new Match();
        _channel = Channel.CreateUnbounded<IUpdateMessage>();
        _cts = new CancellationTokenSource();
    }

    public void Connect()
    {
        ConnectionWindow window = new(_channel, _cts.Token);
        bool result = window.ShowDialog() ?? false;
        if (!result)
        {
            MessageBox.Show("Połączenie nie powiodło się. Spróbuj ponownie!");
            Connect();
            return;
        }
        Client = window.ViewModel.Client;
    }
}
