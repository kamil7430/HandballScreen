using Screen.ViewModel;

namespace Screen.Messages;

public class ConnectionLostMessage : IUpdateMessage
{
    public Exception Exception { get; private set; }

    public ConnectionLostMessage(Exception exception)
    {
        Exception = exception;
    }

    public void HandleSelf(MainWindowViewModel viewModel)
        => viewModel.Connect();
}
