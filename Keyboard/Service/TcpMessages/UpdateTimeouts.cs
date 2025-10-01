namespace Keyboard.Service.TcpMessages;

public record UpdateTimeouts(short Hosts, short Guests) : IUpdateMessage
{ }
