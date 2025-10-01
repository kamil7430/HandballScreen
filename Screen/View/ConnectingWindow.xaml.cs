using Keyboard.Service.TcpMessages;
using System.Net;
using System.Threading.Channels;
using System.Windows;

namespace Screen.View
{
    /// <summary>
    /// Logika interakcji dla klasy ConnectingWindow.xaml
    /// </summary>
    public partial class ConnectingWindow : Window
    {
        public ScreenManagerClient Client { get; }

        public ConnectingWindow(IPAddress ip, int port, Channel<IUpdateMessage> channel, CancellationToken cancellationToken)
        {
            InitializeComponent();
            Loaded += ConnectingWindow_Loaded;
            Client = new ScreenManagerClient(ip, port, channel, cancellationToken);
        }

        private async void ConnectingWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                await Client.ConnectAndRun();
                DialogResult = true;
            }
            catch
            {
                DialogResult = false;
            }
            Close();
        }
    }
}
