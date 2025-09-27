using Keyboard.Model;
using Keyboard.ViewModel;
using System.Windows;

namespace Keyboard.View
{
    /// <summary>
    /// Logika interakcji dla klasy NewMatchWindow.xaml
    /// </summary>
    public partial class NewMatchWindow : Window, ICloseableWindow
    {
        public NewMatchWindowViewModel ViewModel { get; }

        public NewMatchWindow(Match match)
        {
            InitializeComponent();
            DataContext = ViewModel = new NewMatchWindowViewModel(match, this);
        }
    }
}
