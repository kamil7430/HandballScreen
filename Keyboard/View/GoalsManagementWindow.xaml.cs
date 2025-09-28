using Keyboard.Model;
using Keyboard.ViewModel;
using System.Windows;

namespace Keyboard.View
{
    /// <summary>
    /// Logika interakcji dla klasy GoalsManagementWindow.xaml
    /// </summary>
    public partial class GoalsManagementWindow : Window, ICloseableWindow
    {
        public GoalsManagementWindowViewModel ViewModel { get; }

        public GoalsManagementWindow(Match match)
        {
            InitializeComponent();
            DataContext = ViewModel = new GoalsManagementWindowViewModel(match, this);
        }
    }
}
