using System.Windows;

namespace Keyboard.View;

public interface ICloseableWindow
{
    public void CloseWindow(bool result)
    {
        ((Window)this).DialogResult = result;
        ((Window)this).Close();
    }
}
