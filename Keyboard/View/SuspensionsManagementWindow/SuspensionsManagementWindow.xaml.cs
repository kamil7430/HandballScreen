using Keyboard.Model;
using Keyboard.ViewModel.SuspensionsManagementWindowViewModel;
using System.Windows;

namespace Keyboard.View.SuspensionsManagementWindow
{
    /// <summary>
    /// Logika interakcji dla klasy SuspensionsManagementWindow.xaml
    /// </summary>
    public partial class SuspensionsManagementWindow : Window, ICloseableWindow
    {
        public SuspensionsManagementWindowViewModel ViewModel { get; set; }

        public SuspensionsManagementWindow(IEnumerable<Suspension> suspensions)
        {
            InitializeComponent();
            DataContext = ViewModel = new SuspensionsManagementWindowViewModel(suspensions, this);
        }
    }
}
