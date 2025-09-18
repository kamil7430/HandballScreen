using System.Net.Sockets;
using System.Text.Json;

namespace MyTcpConnector;

internal static class TcpHelper
{
    public static async Task SendAsync<T>(NetworkStream stream, T objectToSend,
        CancellationToken cancellationToken, JsonSerializerOptions? jsonSerializerOptions = null)
    {
        byte[] bytes = JsonSerializer.SerializeToUtf8Bytes(objectToSend, jsonSerializerOptions);
        await stream.WriteAsync(bytes, cancellationToken);
    }
}
