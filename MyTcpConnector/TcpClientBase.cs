using System.Net;
using System.Net.Sockets;

namespace MyTcpConnector;

/// <summary>
/// An abstract base class implementing core functionality for an asynchronous TCP client.
/// It handles connection establishment, resource management (<see cref="IDisposable"/>),
/// and cancellation via a token. Derived classes must implement the specific
/// communication with server logic.
/// </summary>
public abstract class TcpClientBase : IDisposable
{
    /// <summary>
    /// The IP address of the server the client intends to connect to.
    /// </summary>
    protected readonly IPAddress IpAddress;

    /// <summary>
    /// The port number of the server.
    /// </summary>
    protected readonly int Port;

    /// <summary>
    /// The combined IP address and port number defining the server endpoint.
    /// </summary>
    protected readonly IPEndPoint IpEndPoint;

    /// <summary>
    /// The underlying <see cref="TcpClient"/> instance used to manage the network connection.
    /// </summary>
    protected readonly TcpClient TcpClient;

    /// <summary>
    /// The cancellation token used to propagate a cancellation request to connection and I/O operations.
    /// </summary>
    protected readonly CancellationToken CancellationToken;

    /// <summary>
    /// A value indicating whether the object has been disposed.
    /// </summary>
    protected bool Disposed;

    /// <summary>
    /// Initializes a new instance of the <see cref="TcpClientBase"/> class.
    /// </summary>
    /// <param name="ipAddress">The IP address of the server.</param>
    /// <param name="port">The port number of the server.</param>
    /// <param name="cancellationToken">The token used to cancel connection or communication operations.</param>
    public TcpClientBase(IPAddress ipAddress, int port, CancellationToken cancellationToken)
    {
        IpAddress = ipAddress;
        Port = port;
        IpEndPoint = new IPEndPoint(ipAddress, port);
        TcpClient = new TcpClient();
        CancellationToken = cancellationToken;
        Disposed = false;
    }

    /// <summary>
    /// Asynchronously connects to the server and then starts the background
    /// communication loop by invoking <see cref="CommunicateWithServer"/>.
    /// </summary>
    /// <remarks>
    /// The communication loop is launched as a fire-and-forget task (without <c>Task.Run</c>)
    /// as it is an I/O-bound operation.
    /// </remarks>
    /// <returns>A Task that completes when the connection is established.</returns>
    /// <exception cref="ObjectDisposedException">Thrown if the client has already been disposed.</exception>
    /// <exception cref="SocketException">Thrown if the connection cannot be established.</exception>
    public virtual async Task ConnectAndRun()
    {
        CheckIfDisposed();
        await TcpClient.ConnectAsync(IpEndPoint, CancellationToken);
        _ = CommunicateWithServer();
    }

    /// <summary>
    /// The abstract method that defines the client's communication logic (e.g., continuous read/write loop).
    /// This method must be implemented by derived classes and should respect the <see cref="CancellationToken"/>.
    /// </summary>
    /// <remarks>
    /// Always remember to handle <see cref="Responses.Server.WelcomeResponse"/> gracefully! It could
    /// indicate either success or failure to connect to the server.
    /// </remarks>
    /// <returns>A Task representing the asynchronous communication loop.</returns>
    protected abstract Task CommunicateWithServer();

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    /// <param name="disposing">
    /// <c>true</c> to release both managed and unmanaged resources; 
    /// <c>false</c> to release only unmanaged resources.
    /// </param>
    protected virtual void Dispose(bool disposing)
    {
        if (!Disposed)
        {
            if (disposing)
            {
                TcpClient?.Close();
            }
            Disposed = true;
        }
    }

    /// <summary>
    /// Disposes this client instance and closes connection with the server.
    /// </summary>
    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Throws an <see cref="ObjectDisposedException"/> if the object has been disposed.
    /// </summary>
    /// <exception cref="ObjectDisposedException">Thrown if <see cref="Disposed"/> is <c>true</c>.</exception>
    protected void CheckIfDisposed()
        => ObjectDisposedException.ThrowIf(Disposed, this);
}
