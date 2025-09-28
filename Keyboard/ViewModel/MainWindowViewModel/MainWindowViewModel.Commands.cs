using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Keyboard.Model;
using Keyboard.View;
using Keyboard.View.SuspensionsManagementWindow;

namespace Keyboard.ViewModel;

public partial class MainWindowViewModel : ObservableObject
{
    private bool IsTimeStopped
        => Match.IsTimeStopped;

    private bool IsTimeStarted
        => !IsTimeStopped;

    private bool CanTimeBeResumed
        => IsTimeStopped && Match.MaxMatchTimeInDecyseconds * 2 > Match.TimeInDecyseconds;

    private bool CanAddUsedHostsTimeout
        => Match.HostsTimeoutsUsed < Match.MaxTimeouts;

    private bool CanRemoveUsedHostsTimeout
        => Match.HostsTimeoutsUsed >= 1;

    private bool CanAddUsedGuestsTimeout
        => Match.GuestsTimeoutsUsed < Match.MaxTimeouts;

    private bool CanRemoveUsedGuestsTimeout
        => Match.GuestsTimeoutsUsed >= 1;

    [RelayCommand]
    private void AddHostsGoal()
        => Match.HostsPoints++;

    [RelayCommand]
    private void RemoveHostsGoal()
        => Match.HostsPoints--;

    [RelayCommand(CanExecute = nameof(IsTimeStopped))]
    private void ManageHostsGoals()
    {
        GoalsManagementWindow window = new(Match);
        bool result = window.ShowDialog() ?? false;
        if (result)
            Match = window.ViewModel.NewMatch ?? throw new ArgumentNullException();
    }

    [RelayCommand(CanExecute = nameof(CanAddUsedHostsTimeout))]
    private void AddUsedHostsTimeout()
        => Match.HostsTimeoutsUsed++;

    [RelayCommand(CanExecute = nameof(CanRemoveUsedHostsTimeout))]
    private void RemoveUsedHostsTimeout()
        => Match.HostsTimeoutsUsed--;

    [RelayCommand(CanExecute = nameof(IsTimeStopped))]
    private void ManageHostsSuspensions()
    {
        var suspensions = ManageSuspensions(Match.HostsSuspensions);
        if (suspensions != null)
            Match.HostsSuspensions = [.. suspensions];
    }

    [RelayCommand(CanExecute = nameof(CanTimeBeResumed))]
    private void ResumeMatchClock()
    {
        Match.IsTimeStopped = false;
        _lastClockResume = DateTime.Now;
        _timerService.Start();
    }

    [RelayCommand(CanExecute = nameof(IsTimeStarted))]
    private void StopMatchClock()
    {
        Match.IsTimeStopped = true;
        _decysecondsAtLastClockStop = Match.TimeInDecyseconds;
        _timerService.Stop();
    }

    [RelayCommand(CanExecute = nameof(IsTimeStopped))]
    private void SetClock()
    {
        TimeSettingWindow window = new(Match);
        bool result = window.ShowDialog() ?? false;
        if (result)
        {
            Match = window.ViewModel.NewMatch ?? throw new ArgumentNullException();
            _decysecondsAtLastClockStop = Match.TimeInDecyseconds;
            CleanUpSuspensions();
        }
    }

    [RelayCommand]
    private void UseSoundEffect() { }

    [RelayCommand]
    private void AddGuestsGoal()
        => Match.GuestsPoints++;

    [RelayCommand]
    private void RemoveGuestsGoal()
        => Match.GuestsPoints--;

    [RelayCommand(CanExecute = nameof(IsTimeStopped))]
    private void ManageGuestsGoals()
        => ManageHostsGoals();

    [RelayCommand(CanExecute = nameof(CanAddUsedGuestsTimeout))]
    private void AddUsedGuestsTimeout()
        => Match.GuestsTimeoutsUsed++;

    [RelayCommand(CanExecute = nameof(CanRemoveUsedGuestsTimeout))]
    private void RemoveUsedGuestsTimeout()
        => Match.GuestsTimeoutsUsed--;

    [RelayCommand(CanExecute = nameof(IsTimeStopped))]
    private void ManageGuestsSuspensions()
    {
        var suspensions = ManageSuspensions(Match.GuestsSuspensions);
        if (suspensions != null)
            Match.GuestsSuspensions = [.. suspensions];
    }

    [RelayCommand(CanExecute = nameof(IsTimeStopped))]
    private void NewMatch()
    {
        NewMatchWindow window = new(Match);
        bool result = window.ShowDialog() ?? false;
        if (result)
        {
            Match = window.ViewModel.NewMatch ?? throw new ArgumentNullException();
            _decysecondsAtLastClockStop = 0;
        }
    }

    private IEnumerable<Suspension>? ManageSuspensions(IEnumerable<Suspension> suspensions)
    {
        SuspensionsManagementWindow window = new(suspensions);
        bool result = window.ShowDialog() ?? false;
        return result ? window.ViewModel.Suspensions : null;
    }

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
        NewMatchCommand.NotifyCanExecuteChanged();
    }
}
