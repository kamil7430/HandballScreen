using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Keyboard.Model;
using Keyboard.Service.TcpMessages;
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
    {
        Match.HostsPoints++;
        EnqueueMessage(new UpdateScore(Match.HostsPoints, Match.GuestsPoints));
    }


    [RelayCommand]
    private void RemoveHostsGoal()
    {
        Match.HostsPoints--;
        EnqueueMessage(new UpdateScore(Match.HostsPoints, Match.GuestsPoints));
    }

    [RelayCommand(CanExecute = nameof(IsTimeStopped))]
    private void ManageHostsGoals()
    {
        GoalsManagementWindow window = new(Match);
        bool result = window.ShowDialog() ?? false;
        if (result)
        {
            Match = window.ViewModel.NewMatch ?? throw new ArgumentNullException();
            EnqueueMessage(new UpdateScore(Match.HostsPoints, Match.GuestsPoints));
        }
    }

    [RelayCommand(CanExecute = nameof(CanAddUsedHostsTimeout))]
    private void AddUsedHostsTimeout()
    {
        Match.HostsTimeoutsUsed++;
        EnqueueMessage(new UpdateTimeouts(Match.HostsTimeoutsUsed, Match.GuestsTimeoutsUsed));
    }

    [RelayCommand(CanExecute = nameof(CanRemoveUsedHostsTimeout))]
    private void RemoveUsedHostsTimeout()
    {
        Match.HostsTimeoutsUsed--;
        EnqueueMessage(new UpdateTimeouts(Match.HostsTimeoutsUsed, Match.GuestsTimeoutsUsed));
    }

    [RelayCommand(CanExecute = nameof(IsTimeStopped))]
    private void ManageHostsSuspensions()
    {
        var suspensions = ManageSuspensions(Match.HostsSuspensions);
        if (suspensions != null)
        {
            Match.HostsSuspensions = [.. suspensions];
            OnPropertyChanged(nameof(Match));
            EnqueueMessage(new UpdateWholeMatch(Match));
        }
    }

    [RelayCommand(CanExecute = nameof(CanTimeBeResumed))]
    private void ResumeMatchClock()
    {
        Match.IsTimeStopped = false;
        _lastClockResume = DateTime.Now;
        _timerService.Start();
        EnqueueMessage(new ResumeClockMessage(_lastClockResume));
    }

    [RelayCommand(CanExecute = nameof(IsTimeStarted))]
    private void StopMatchClock()
    {
        Match.IsTimeStopped = true;
        _decysecondsAtLastClockStop = Match.TimeInDecyseconds;
        _timerService.Stop();
        EnqueueMessage(new StopClockMessage(Match.TimeInDecyseconds));
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
            EnqueueMessage(new UpdateWholeMatch(Match));
        }
    }

    [RelayCommand]
    private void UseSoundEffect()
        => EnqueueMessage(new UseSoundEffectMessage());

    [RelayCommand]
    private void AddGuestsGoal()
    {
        Match.GuestsPoints++;
        EnqueueMessage(new UpdateScore(Match.HostsPoints, Match.GuestsPoints));
    }

    [RelayCommand]
    private void RemoveGuestsGoal()
    {
        Match.GuestsPoints--;
        EnqueueMessage(new UpdateScore(Match.HostsPoints, Match.GuestsPoints));
    }

    [RelayCommand(CanExecute = nameof(IsTimeStopped))]
    private void ManageGuestsGoals()
        => ManageHostsGoals();

    [RelayCommand(CanExecute = nameof(CanAddUsedGuestsTimeout))]
    private void AddUsedGuestsTimeout()
    {
        Match.GuestsTimeoutsUsed++;
        EnqueueMessage(new UpdateTimeouts(Match.HostsTimeoutsUsed, Match.GuestsTimeoutsUsed));
    }

    [RelayCommand(CanExecute = nameof(CanRemoveUsedGuestsTimeout))]
    private void RemoveUsedGuestsTimeout()
    {
        Match.GuestsTimeoutsUsed--;
        EnqueueMessage(new UpdateTimeouts(Match.HostsTimeoutsUsed, Match.GuestsTimeoutsUsed));
    }

    [RelayCommand(CanExecute = nameof(IsTimeStopped))]
    private void ManageGuestsSuspensions()
    {
        var suspensions = ManageSuspensions(Match.GuestsSuspensions);
        if (suspensions != null)
        {
            Match.GuestsSuspensions = [.. suspensions];
            OnPropertyChanged(nameof(Match));
            EnqueueMessage(new UpdateWholeMatch(Match));
        }
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
            EnqueueMessage(new UpdateWholeMatch(Match));
        }
    }

    private IEnumerable<Suspension>? ManageSuspensions(IEnumerable<Suspension> suspensions)
    {
        SuspensionsManagementWindow window = new(Match, suspensions);
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
