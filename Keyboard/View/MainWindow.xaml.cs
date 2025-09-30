using Keyboard.ViewModel;
using System.Windows;

namespace Keyboard.View
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindowViewModel ViewModel { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = ViewModel = new MainWindowViewModel();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
            => _ = ViewModel.RunServer();
    }
}
