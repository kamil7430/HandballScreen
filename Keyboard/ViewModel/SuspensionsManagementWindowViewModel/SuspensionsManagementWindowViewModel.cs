using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Keyboard.Model;
using Keyboard.View;
using Keyboard.View.SuspensionsManagementWindow;
using System.Collections.ObjectModel;

namespace Keyboard.ViewModel.SuspensionsManagementWindowViewModel;

public partial class SuspensionsManagementWindowViewModel : SettingsWindowViewModelBase
{
    private Match _match;

    public ObservableCollection<Suspension> Suspensions { get; set; }

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(EditClickCommand))]
    [NotifyCanExecuteChangedFor(nameof(DeleteClickCommand))]
    private Suspension? selectedSuspension;

    public SuspensionsManagementWindowViewModel(Match match, IEnumerable<Suspension> suspensions, ICloseableWindow window)
        : base(window)
    {
        _match = match;
        Suspensions = [.. suspensions];
    }

    private bool IsAnySuspensionSelected
        => SelectedSuspension != null;

    [RelayCommand]
    private void AddClick()
    {
        var newSuspension = ShowSuspensionEditWindow(null);
        if (newSuspension != null)
            Suspensions.Add(newSuspension);
    }

    [RelayCommand(CanExecute = nameof(IsAnySuspensionSelected))]
    private void EditClick()
    {
        var newSuspension = ShowSuspensionEditWindow(SelectedSuspension);
        if (newSuspension != null)
        {
            var index = Suspensions.IndexOf(SelectedSuspension!);
            Suspensions.Insert(index, newSuspension);
            Suspensions.Remove(SelectedSuspension!);
            SelectedSuspension = null;
        }
    }

    [RelayCommand(CanExecute = nameof(IsAnySuspensionSelected))]
    private void DeleteClick()
    {
        Suspensions.Remove(SelectedSuspension!);
        SelectedSuspension = null;
    }

    private Suspension? ShowSuspensionEditWindow(Suspension? suspension)
    {
        AddOrEditSuspensionWindow window = new(_match, suspension);
        bool result = window.ShowDialog() ?? false;
        return result ? window.ViewModel.NewSuspension : null;
    }

    protected override bool ValidateFields()
        => true;
}
