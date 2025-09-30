using Screen.ViewModel;
using System.Windows;

namespace Screen.View;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindowViewModel ViewModel { get; set; }

    public MainWindow()
    {
        InitializeComponent();
        DataContext = ViewModel = new MainWindowViewModel();
    }
}