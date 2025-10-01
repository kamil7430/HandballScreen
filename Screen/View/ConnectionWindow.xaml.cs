using Keyboard.Service.TcpMessages;
using Keyboard.View;
using Screen.ViewModel;
using System.Threading.Channels;
using System.Windows;

namespace Screen.View
{
    /// <summary>
    /// Logika interakcji dla klasy ConnectionWindow.xaml
    /// </summary>
    public partial class ConnectionWindow : Window, ICloseableWindow
    {
        public ConnectionWindowViewModel ViewModel { get; set; }

        public ConnectionWindow(Channel<IUpdateMessage> channel, CancellationToken cancellationToken)
        {
            InitializeComponent();
            DataContext = ViewModel = new ConnectionWindowViewModel(cancellationToken, channel, this);
        }
    }
}
