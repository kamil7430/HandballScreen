using Keyboard.Model;
using Keyboard.ViewModel.SuspensionsManagementWindowViewModel;
using System.Windows;

namespace Keyboard.View.SuspensionsManagementWindow
{
    /// <summary>
    /// Logika interakcji dla klasy AddOrEditSuspensionWindow.xaml
    /// </summary>
    public partial class AddOrEditSuspensionWindow : Window, ICloseableWindow
    {
        public AddOrEditSuspensionWindowViewModel ViewModel { get; set; }

        public AddOrEditSuspensionWindow(Match match, Suspension? suspension = null)
        {
            InitializeComponent();
            DataContext = ViewModel = new AddOrEditSuspensionWindowViewModel(match, suspension, this);
        }
    }
}
