using Keyboard.Service.TcpMessages;
using MyTcpConnector;
using MyTcpConnector.Responses.Server;
using Screen.Messages;
using System.Net;
using System.Threading.Channels;

namespace Screen;

public class ScreenManagerClient : TcpClientBase
{
    private readonly Channel<IUpdateMessage> _channel;

    public ScreenManagerClient(IPAddress ipAddress, int port, Channel<IUpdateMessage> channel, CancellationToken cancellationToken)
        : base(ipAddress, port, cancellationToken)
    {
        _channel = channel;
    }

    protected override async Task CommunicateWithServer()
    {
        using var stream = TcpClient.GetStream();
        try
        {
            var response = await TcpHelper.ReceiveAsync<WelcomeResponse>(stream, CancellationToken);
            if (response == null || response.Status == false)
                throw new WebException("Connection refused.");

            while (!CancellationToken.IsCancellationRequested)
            {
                var msg = await TcpHelper.ReceiveAsync<IUpdateMessage>(stream, CancellationToken)
                    ?? throw new ArgumentNullException("Deserialization failure.");
                await _channel.Writer.WriteAsync(msg, CancellationToken);
            }
        }
        catch (Exception exception)
        {
            await _channel.Writer.WriteAsync(new ConnectionLostMessage(exception), CancellationToken);
        }
    }
}
