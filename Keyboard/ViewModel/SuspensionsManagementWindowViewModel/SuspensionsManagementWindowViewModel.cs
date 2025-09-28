using Keyboard.Model;
using Keyboard.View;
using System.Collections.ObjectModel;

namespace Keyboard.ViewModel.SuspensionsManagementWindowViewModel;

public class SuspensionsManagementWindowViewModel : SettingsWindowViewModelBase
{
    public ObservableCollection<Suspension> Suspensions { get; set; }

    public SuspensionsManagementWindowViewModel(IEnumerable<Suspension> suspensions, ICloseableWindow window)
        : base(window)
    {
        Suspensions = [.. suspensions];
    }

    protected override bool ValidateFields()
    {
        throw new NotImplementedException();
    }
}
