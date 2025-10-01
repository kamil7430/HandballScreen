namespace Keyboard.Service.TcpMessages;

public record UpdateScore(int Hosts, int Guests) : IUpdateMessage
{ }
