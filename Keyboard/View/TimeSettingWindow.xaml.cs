using Keyboard.Model;
using Keyboard.ViewModel;
using System.Windows;

namespace Keyboard.View
{
    /// <summary>
    /// Logika interakcji dla klasy TimeSettingWindow.xaml
    /// </summary>
    public partial class TimeSettingWindow : Window, ICloseableWindow
    {
        public TimeSettingWindowViewModel ViewModel { get; }

        public TimeSettingWindow(Match match)
        {
            InitializeComponent();
            DataContext = ViewModel = new TimeSettingWindowViewModel(match, this);
        }
    }
}
