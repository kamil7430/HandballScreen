using Keyboard.View;
using Screen.ViewModel;
using System.Windows;

namespace Screen.View
{
    /// <summary>
    /// Logika interakcji dla klasy ConnectionWindow.xaml
    /// </summary>
    public partial class ConnectionWindow : Window, ICloseableWindow
    {
        public ConnectionWindowViewModel ViewModel { get; set; }

        public ConnectionWindow()
        {
            InitializeComponent();
            DataContext = ViewModel = new ConnectionWindowViewModel(this);
        }
    }
}
