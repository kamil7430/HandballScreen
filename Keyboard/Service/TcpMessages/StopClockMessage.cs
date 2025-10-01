namespace Keyboard.Service.TcpMessages;

public record StopClockMessage(long DecysecondsOnClock) : IUpdateMessage
{ }
