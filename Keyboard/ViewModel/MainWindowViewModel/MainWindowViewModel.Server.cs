using Keyboard.Service;
using Keyboard.Service.TcpMessages;
using System.Threading.Channels;

namespace Keyboard.ViewModel;

public partial class MainWindowViewModel
{
    private readonly Channel<IUpdateMessage> _channel;
    private readonly CancellationToken _serverCancellationToken;
    public ScreenManagerServer Server { get; }

    public async Task RunServer()
    {
        await Server.StartListener();
        OnPropertyChanged(nameof(Server));
    }
}
