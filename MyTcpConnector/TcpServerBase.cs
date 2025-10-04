using MyTcpConnector.Responses.Server;
using System.Net;
using System.Net.Sockets;

namespace MyTcpConnector;

/// <summary>
/// Provides an abstract base class for a multi-client TCP server.
/// It manages the listening loop, client capacity, and thread-safe client list management.
/// Derived classes must implement logic for welcoming and serving individual clients.
/// </summary>
public abstract class TcpServerBase : IDisposable
{
    /// <summary>
    /// The maximum number of concurrent clients the server can handle.
    /// This field is read-only.
    /// </summary>
    protected readonly int MaxClients;

    /// <summary>
    /// The IP address on which the server will listen.
    /// This field is read-only.
    /// </summary>
    protected readonly IPAddress IpAddress;

    /// <summary>
    /// The port number on which the server will listen.
    /// This field is read-only.
    /// </summary>
    protected readonly int Port;

    /// <summary>
    /// An array to hold the currently connected <see cref="TcpClient"/> objects. 
    /// Null entries indicate a free slot.
    /// </summary>
    protected TcpClient?[] Clients;

    /// <summary>
    /// A counter for the current number of connected clients. (Note: May not be actively maintained in all derived classes.)
    /// </summary>
    protected int ClientsCount;

    /// <summary>
    /// A synchronization object used to ensure thread-safe access to the <see cref="Clients"/> array.
    /// This field is read-only.
    /// </summary>
    protected readonly object ClientsArrayLock;

    /// <summary>
    /// A cancellation token used to signal the server's main listening loop to stop gracefully.
    /// This field is read-only.
    /// </summary>
    protected readonly CancellationToken CancellationToken;

    /// <summary>
    /// Flag indicating whether the object has been disposed.
    /// </summary>
    protected bool Disposed;

    /// <summary>
    /// Initializes a new instance of the <see cref="TcpServerBase"/> class.
    /// </summary>
    /// <param name="maxClients">The maximum number of clients the server can accept.</param>
    /// <param name="ipAddress">The IP address to bind the server to (e.g., <see cref="IPAddress.Any"/>).</param>
    /// <param name="port">The port number to listen on.</param>
    /// <param name="cancellationToken">The token used to manage the server's lifecycle.</param>
    public TcpServerBase(int maxClients, IPAddress ipAddress, int port, CancellationToken cancellationToken)
    {
        MaxClients = maxClients;
        IpAddress = ipAddress;
        Port = port;
        Clients = new TcpClient?[maxClients];
        ClientsArrayLock = new object();
        CancellationToken = cancellationToken;
        Disposed = false;
    }

    /// <summary>
    /// Starts the server's main listening loop, continuously accepting incoming connections.
    /// Once the connection is accepted, the check for free slot in <see cref="Clients"/> array is performed.
    /// If there are no free slots, <see cref="RefuseToConnect(TcpClient)"/> is runned, otherwise
    /// the <see cref="WelcomeClient(int, TcpClient)"/> is performed and new <see cref="Task"/> is created
    /// to run <see cref="ServeClient(int, TcpClient)"/> on it.
    /// </summary>
    /// <remarks>
    /// This method runs until the associated <see cref="CancellationToken"/> is signaled.
    /// </remarks>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
    /// <exception cref="ObjectDisposedException">Thrown if the server instance has already been disposed.</exception>
    public virtual async Task StartListener()
    {
        CheckIfDisposed();

        var ipEndPoint = new IPEndPoint(IpAddress, Port);
        TcpListener listener = new(ipEndPoint);
        TcpClient handler;

        try
        {
            listener.Start();
            while (!CancellationToken.IsCancellationRequested)
            {
                handler = await listener.AcceptTcpClientAsync(CancellationToken);

                var index = GetFreeIndexInClientsArray();
                if (index == null)
                {
                    await RefuseToConnect(handler);
                    continue;
                }

                lock (ClientsArrayLock)
                {
                    Clients[index.Value] = handler;
                }
                await WelcomeClient(index.Value, handler);
                _ = ServeClient(index.Value, handler);
            }
        }
        finally
        {
            listener.Stop();
        }
    }

    /// <summary>
    /// Sends a refusal response to a newly connected client when the server is full, and then closes the connection.
    /// </summary>
    /// <param name="client">The client that attempted to connect.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
    protected virtual async Task RefuseToConnect(TcpClient client)
    {
        var response = new WelcomeResponse(false, "Server is full!", -1);
        var stream = client.GetStream();
        await TcpHelper.SendAsync(stream, response, CancellationToken);
        client.Close();
    }

    /// <summary>
    /// Abstract method called when a new client is successfully accepted.
    /// Derived classes must implement logic for initial communication (e.g., sending a welcome message).
    /// </summary>
    /// <param name="index">The index of the client in the <see cref="Clients"/> array.</param>
    /// <param name="client">The newly accepted <see cref="TcpClient"/>.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
    protected abstract Task WelcomeClient(int index, TcpClient client);

    /// <summary>
    /// Abstract method containing the main communication logic for a connected client.
    /// This method runs concurrently for each client.
    /// </summary>
    /// <remarks>
    /// Always remember to close connections with clients properly! See <see cref="DisconnectClient(int)"/>.
    /// </remarks>
    /// <param name="index">The index of the client in the <see cref="Clients"/> array.</param>
    /// <param name="client">The <see cref="TcpClient"/> to serve.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
    protected abstract Task ServeClient(int index, TcpClient client);

    /// <summary>
    /// Finds the first available (null) index in the <see cref="Clients"/> array.
    /// </summary>
    /// <returns>The integer index of the free slot, or <c>null</c> if the array is full.</returns>
    protected int? GetFreeIndexInClientsArray()
    {
        lock (ClientsArrayLock)
        {
            for (int i = 0; i < MaxClients; i++)
                if (Clients[i] == null)
                    return i;
            return null;
        }
    }

    /// <summary>
    /// Closes connection with client with provided index and sets appropriate <see cref="Clients"/> array cell to null.
    /// </summary>
    /// <remarks>
    /// When overriding, always remember to set the appropriate <see cref="Clients"/> cell to null!
    /// </remarks>
    /// <param name="index">Index of the client to be disconnected.</param>
    protected virtual void DisconnectClient(int index)
    {
        lock (ClientsArrayLock)
        {
            Clients[index]?.Close();
            Clients[index] = null;
        }
    }

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
                lock (ClientsArrayLock)
                {
                    for (int i = 0; i < MaxClients; i++)
                        DisconnectClient(i);
                }
            }
            Disposed = true;
        }
    }

    /// <summary>
    /// Disposes of the server instance and closes all connected clients.
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
