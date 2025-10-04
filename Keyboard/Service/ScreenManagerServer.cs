using Keyboard.Service.TcpMessages;
using MyTcpConnector;
using MyTcpConnector.Responses.Server;
using System.ComponentModel;
using System.Net;
using System.Net.Sockets;
using System.Threading.Channels;

namespace Keyboard.Service;

public class ScreenManagerServer : TcpServerBase, INotifyPropertyChanged
{
    private readonly Channel<IUpdateMessage> _channel;
    private readonly Action _welcomeAction;

    public string IpAddressString
        => IpAddress.ToString();

    public string PortString
        => Port.ToString();

    private bool _isClientConnected;
    public bool IsClientConnected
    {
        get => _isClientConnected;
        set
        {
            _isClientConnected = value;
            OnPropertyChanged(nameof(IsClientConnected));
        }
    }

    public ScreenManagerServer(Channel<IUpdateMessage> channel, Action welcome, CancellationToken cancellationToken)
        : base(1, IPAddress.Any, 5050, cancellationToken)
    {
        _channel = channel;
        _welcomeAction = welcome;
        IsClientConnected = false;
    }

    protected override async Task ServeClient(int index, TcpClient client)
    {
        var stream = client.GetStream();
        try
        {
            while (!CancellationToken.IsCancellationRequested
                && await _channel.Reader.WaitToReadAsync(CancellationToken))
            {
                var toSend = await _channel.Reader.ReadAsync(CancellationToken);
                await TcpHelper.SendAsync(stream, toSend, CancellationToken);
            }
        }
        finally
        {
            DisconnectClient(index);
            IsClientConnected = false;
        }
    }

    protected override async Task WelcomeClient(int index, TcpClient client)
    {
        var stream = client.GetStream();
        IsClientConnected = true;
        var response = new WelcomeResponse(true, "", index);
        await TcpHelper.SendAsync(stream, response, CancellationToken);
        _welcomeAction();
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged(string propertyName)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
