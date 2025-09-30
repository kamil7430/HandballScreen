using Keyboard.Service.TcpMessages;
using MyTcpConnector;
using MyTcpConnector.Responses.Server;
using System.Net;
using System.Net.Sockets;
using System.Threading.Channels;

namespace Keyboard.Service;

public class ScreenManagerServer : TcpServerBase
{
    private readonly Channel<IUpdateMessage> _channel;

    public string IpAddressString
        => IpAddress.ToString();

    public string PortString
        => Port.ToString();

    public ScreenManagerServer(Channel<IUpdateMessage> channel, CancellationToken cancellationToken)
        : base(1, IPAddress.Any, 5050, cancellationToken)
    {
        _channel = channel;
    }

    protected override async Task ServeClient(int index, TcpClient client)
    {
        using var stream = client.GetStream();
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
        }
    }

    protected override async Task WelcomeClient(int index, TcpClient client)
    {
        using var stream = client.GetStream();
        var response = new WelcomeResponse(true, "", index);
        await TcpHelper.SendAsync(stream, response, CancellationToken);
    }
}
