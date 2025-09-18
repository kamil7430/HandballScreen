using MyTcpConnector.Responses.Server;
using System.Net;
using System.Net.Sockets;

namespace MyTcpConnector;

public class TcpServer : IDisposable
{
    private readonly int _maxClients;
    private readonly IPAddress _ipAddress;
    private readonly int _port;
    private TcpClient?[] _clients;
    private int _clientsCount;
    private readonly object _clientsArrayLock;
    private readonly CancellationToken _cancellationToken;
    private bool _disposed;

    public TcpServer(int maxClients, IPAddress ipAddress, int port, CancellationToken cancellationToken)
    {
        _maxClients = maxClients;
        _ipAddress = ipAddress;
        _port = port;
        _clients = new TcpClient?[maxClients];
        _clientsCount = 0;
        _clientsArrayLock = new object();
        _cancellationToken = cancellationToken;
        _disposed = false;
    }

    public async Task StartListener()
    {
        CheckIfDisposed();

        var ipEndPoint = new IPEndPoint(_ipAddress, _port);
        TcpListener listener = new(ipEndPoint);
        TcpClient handler;

        try
        {
            listener.Start();
            while (!_cancellationToken.IsCancellationRequested)
            {
                handler = await listener.AcceptTcpClientAsync(_cancellationToken);

                var index = GetFreeIndexInClientsArray();
                if (index == null)
                {
                    var response = new WelcomeResponse(false, "Server is full!", -1);
                    using var stream = handler.GetStream();
                    await TcpHelper.SendAsync(stream, response, _cancellationToken);
                    continue;
                }


            }
        }
        finally
        {
            listener.Stop();
        }
    }

    private int? GetFreeIndexInClientsArray()
    {
        lock (_clientsArrayLock)
        {
            if (_clientsCount == _maxClients)
                return null;

            int free = 0;
            while (_clients[free] != null)
                free++;
            return free;
        }
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                lock (_clientsArrayLock)
                    foreach (var client in _clients)
                        client?.Close();
            }
            _disposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    private void CheckIfDisposed()
        => ObjectDisposedException.ThrowIf(_disposed, this);
}
