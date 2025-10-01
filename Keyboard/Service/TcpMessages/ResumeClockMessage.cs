namespace Keyboard.Service.TcpMessages;

public record ResumeClockMessage(DateTime DateTime) : IUpdateMessage
{ }
