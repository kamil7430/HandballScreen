using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Keyboard.ViewModel;

public partial class MainWindowViewModel : ObservableObject
{
    private bool IsTimeStopped
        => Match.IsTimeStopped;

    private bool IsTimeStarted
        => !IsTimeStopped;

    private bool CanAddUsedHostsTimeout
        => Match.HostsTimeoutsUsed <= 2;

    private bool CanRemoveUsedHostsTimeout
        => Match.HostsTimeoutsUsed >= 1;

    private bool CanAddUsedGuestsTimeout
        => Match.GuestsTimeoutsUsed <= 2;

    private bool CanRemoveUsedGuestsTimeout
        => Match.GuestsTimeoutsUsed >= 1;

    [RelayCommand]
    private void AddHostsGoal()
        => Match.HostsPoints++;

    [RelayCommand]
    private void RemoveHostsGoal()
        => Match.HostsPoints--;

    [RelayCommand(CanExecute = nameof(IsTimeStopped))]
    private void ManageHostsGoals() { }

    [RelayCommand(CanExecute = nameof(CanAddUsedHostsTimeout))]
    private void AddUsedHostsTimeout()
        => Match.HostsTimeoutsUsed++;

    [RelayCommand(CanExecute = nameof(CanRemoveUsedHostsTimeout))]
    private void RemoveUsedHostsTimeout()
        => Match.HostsTimeoutsUsed--;

    [RelayCommand(CanExecute = nameof(IsTimeStopped))]
    private void ManageHostsSuspensions() { }

    [RelayCommand(CanExecute = nameof(IsTimeStopped))]
    private void ResumeMatchClock()
        => Match.IsTimeStopped = false;

    [RelayCommand(CanExecute = nameof(IsTimeStarted))]
    private void StopMatchClock()
        => Match.IsTimeStopped = true;

    [RelayCommand(CanExecute = nameof(IsTimeStopped))]
    private void SetClock() { }

    [RelayCommand]
    private void UseSoundEffect() { }

    [RelayCommand]
    private void AddGuestsGoal()
        => Match.GuestsPoints++;

    [RelayCommand]
    private void RemoveGuestsGoal()
        => Match.GuestsPoints--;

    [RelayCommand(CanExecute = nameof(IsTimeStopped))]
    private void ManageGuestsGoals() { }

    [RelayCommand(CanExecute = nameof(CanAddUsedGuestsTimeout))]
    private void AddUsedGuestsTimeout()
        => Match.GuestsTimeoutsUsed++;

    [RelayCommand(CanExecute = nameof(CanRemoveUsedGuestsTimeout))]
    private void RemoveUsedGuestsTimeout()
        => Match.GuestsTimeoutsUsed--;

    [RelayCommand(CanExecute = nameof(IsTimeStopped))]
    private void ManageGuestsSuspensions() { }

    private void NotifyCommandsCanExecute()
    {
        ManageHostsGoalsCommand.NotifyCanExecuteChanged();
        AddUsedHostsTimeoutCommand.NotifyCanExecuteChanged();
        RemoveUsedHostsTimeoutCommand.NotifyCanExecuteChanged();
        ManageHostsSuspensionsCommand.NotifyCanExecuteChanged();
        ResumeMatchClockCommand.NotifyCanExecuteChanged();
        StopMatchClockCommand.NotifyCanExecuteChanged();
        SetClockCommand.NotifyCanExecuteChanged();
        ManageGuestsGoalsCommand.NotifyCanExecuteChanged();
        AddUsedGuestsTimeoutCommand.NotifyCanExecuteChanged();
        RemoveUsedGuestsTimeoutCommand.NotifyCanExecuteChanged();
        ManageGuestsSuspensionsCommand.NotifyCanExecuteChanged();
    }
}
