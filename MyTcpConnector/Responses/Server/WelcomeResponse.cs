namespace MyTcpConnector.Responses.Server;

public record WelcomeResponse(
    bool Status,
    string Message,
    int Index
)
{ }
