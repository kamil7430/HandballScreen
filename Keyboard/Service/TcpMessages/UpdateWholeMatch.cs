using Keyboard.Model;

namespace Keyboard.Service.TcpMessages;

public record UpdateWholeMatch(Match Match) : IUpdateMessage
{ }
