using CommunityToolkit.Mvvm.ComponentModel;
using Keyboard.Model;
using Screen.Messages;
using Screen.View;
using System.Threading.Channels;
using System.Windows;
using System.Windows.Threading;

namespace Screen.ViewModel;

public partial class MainWindowViewModel : ObservableObject
{
    [ObservableProperty]
    private Match match;
    public ScreenManagerClient? Client { get; private set; }
    public DateTime LastResumeTimestamp;
    public long DecysecondsOnLastTimeStop;
    private readonly Channel<IUpdateMessage> _channel;
    private readonly CancellationTokenSource _cts;
    public readonly Dispatcher Dispatcher;

    public MainWindowViewModel()
    {
        Match = new Match();
        _channel = Channel.CreateUnbounded<IUpdateMessage>();
        _cts = new CancellationTokenSource();
        Dispatcher = Application.Current.Dispatcher;
        _ = ReadChannelInfinitely(_cts.Token);
    }

    private void HandleMessage(IUpdateMessage message)
    {
        message.HandleSelf(this);
        OnPropertyChanged(nameof(Match));
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

    private async Task ReadChannelInfinitely(CancellationToken cancellationToken)
    {
        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var msg = await _channel.Reader.ReadAsync(cancellationToken);
                Dispatcher.Invoke(() => HandleMessage(msg));
            }
        }
        catch { }
    }
}
