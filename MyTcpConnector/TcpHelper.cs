using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace MyTcpConnector;

/// <summary>
/// Provides static helper methods for asynchronous TCP communication, 
/// handling object serialization (JSON) and message framing using a fixed-length header.
/// </summary>
public static class TcpHelper
{
    /// <summary>
    /// The fixed number of digits used to represent the length of the following message payload.
    /// </summary>
    private const int FIXED_DIGIT_COUNT = 10;

    /// <summary>
    /// The standard format specifier used to convert an integer (the message length) 
    /// into a string of exactly 10 digits, padded with leading zeros (e.g., "D10").
    /// </summary>
    private static readonly string FIXED_DIGIT_COUNT_FORMATTING = $"D{FIXED_DIGIT_COUNT}";

    /// <summary>
    /// Serializes a generic object of type <typeparamref name="T"/> into a JSON byte array, 
    /// prefixes it with a 10-digit length header, and asynchronously sends both through the network stream.
    /// </summary>
    /// <remarks>
    /// This method ensures message integrity by implementing a message framing protocol.
    /// The receiver must read exactly 10 bytes first to determine the payload size.
    /// </remarks>
    /// <typeparam name="T">The type of the object to be sent (e.g., a custom response or request class).</typeparam>
    /// <param name="stream">The active <see cref="NetworkStream"/> used for communication.</param>
    /// <param name="objectToSend">The object instance to serialize and send.</param>
    /// <param name="cancellationToken">The token used to cancel the asynchronous write operation.</param>
    /// <param name="jsonSerializerOptions">Optional settings for the <see cref="JsonSerializer"/>.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous write operation.</returns>
    public static async Task SendAsync<T>(NetworkStream stream, T objectToSend,
        CancellationToken cancellationToken, JsonSerializerOptions? jsonSerializerOptions = null)
    {
        byte[] bytes = JsonSerializer.SerializeToUtf8Bytes(objectToSend, jsonSerializerOptions);
        byte[] sizeBytes = Encoding.UTF8.GetBytes(bytes.Length.ToString(FIXED_DIGIT_COUNT_FORMATTING));
        await stream.WriteAsync(sizeBytes, cancellationToken);
        await stream.WriteAsync(bytes, cancellationToken);
    }

    /// <summary>
    /// Asynchronously receives a message from the network stream by first reading a 10-digit 
    /// length header and then reading the full payload before deserializing it into an object of type <typeparamref name="T"/>.
    /// </summary>
    /// <remarks>
    /// This method uses blocking loops over <see cref="NetworkStream.ReadAsync"/> to ensure 
    /// that the exact number of bytes for both the header and the payload are received.
    /// </remarks>
    /// <typeparam name="T">The type to deserialize the received JSON payload into.</typeparam>
    /// <param name="stream">The active <see cref="NetworkStream"/> used for communication.</param>
    /// <param name="cancellationToken">The token used to cancel the asynchronous read operation.</param>
    /// <param name="jsonSerializerOptions">Optional settings for the <see cref="JsonSerializer"/>.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous read and deserialization operation, returning the deserialized object or default if the message size is zero.</returns>
    public static async Task<T?> ReceiveAsync<T>(NetworkStream stream, CancellationToken cancellationToken,
        JsonSerializerOptions? jsonSerializerOptions = null)
    {
        byte[] sizeBytes = new byte[FIXED_DIGIT_COUNT];
        int receivedBytes = 0;
        while (receivedBytes < FIXED_DIGIT_COUNT)
            receivedBytes += await stream.ReadAsync(sizeBytes, receivedBytes,
                FIXED_DIGIT_COUNT - receivedBytes, cancellationToken);

        int size = int.Parse(Encoding.UTF8.GetString(sizeBytes));
        if (size == 0)
            return default;

        byte[] bytes = new byte[size];
        receivedBytes = 0;
        while (receivedBytes < size)
            receivedBytes += await stream.ReadAsync(bytes, receivedBytes,
                size - receivedBytes, cancellationToken);

        return JsonSerializer.Deserialize<T>(bytes, jsonSerializerOptions);
    }
}
