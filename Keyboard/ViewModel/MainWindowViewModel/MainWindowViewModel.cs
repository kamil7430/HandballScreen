using Keyboard.Model;
using Keyboard.Service.Time;
using System.ComponentModel;

namespace Keyboard.ViewModel;

public partial class MainWindowViewModel
{
    private Match? _match;
    public Match Match
    {
        get => _match!;
        set
        {
            if (_match != null)
                _match.PropertyChanged -= OnMatchPropertyChanged;
            SetProperty(ref _match, value);
            if (_match != null)
                _match.PropertyChanged += OnMatchPropertyChanged;
        }
    }

    private readonly ITimerService _timerService;
    private DateTime _lastClockResume;
    private long _decysecondsAtLastClockStop;

    public MainWindowViewModel()
    {
        Match = new Match();
        _timerService = new WpfTimerService(100); // TODO: Injection
        _timerService.TimerTicked += OnTimerTicked;
    }

    private void OnTimerTicked(DateTime dateTime)
    {
        if (IsTimeStopped)
            return;

        UpdateClock(dateTime);
        CleanUpSuspensions();

        if (Match.TimeInDecyseconds == Match.MaxMatchTimeInDecyseconds
            || Match.TimeInDecyseconds == Match.MaxMatchTimeInDecyseconds * 2)
            EndHalf();
    }

    private void EndHalf()
    {
        if (Match.HalfNumber <= 1)
            Match.HalfNumber++;
        StopMatchClock();
        UseSoundEffect();
    }

    private void UpdateClock(DateTime dateTime)
    {
        Match.TimeInDecyseconds = (long)Math.Round((dateTime - _lastClockResume).TotalSeconds * 10) + _decysecondsAtLastClockStop;
    }

    private void CleanUpSuspensions()
        => Match.CleanUpSuspensions();

    private void OnMatchPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        NotifyCommandsCanExecute();
    }
}
