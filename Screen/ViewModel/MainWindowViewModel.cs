using CommunityToolkit.Mvvm.ComponentModel;
using Keyboard.Model;
using Keyboard.Service.Time;
using Screen.Messages;
using Screen.View;
using System.Threading.Channels;
using System.Windows;
using System.Windows.Threading;

namespace Screen.ViewModel;

public partial class MainWindowViewModel : ObservableObject
{
    [ObservableProperty]
    private Match match;
    public ScreenManagerClient? Client { get; private set; }
    public DateTime LastResumeTimestamp;
    public long DecysecondsOnLastTimeStop;
    private readonly Channel<IUpdateMessage> _channel;
    private readonly CancellationTokenSource _cts;
    public readonly Dispatcher Dispatcher;
    public readonly ITimerService Timer;

    public MainWindowViewModel()
    {
        Match = new Match();
        _channel = Channel.CreateUnbounded<IUpdateMessage>();
        _cts = new CancellationTokenSource();
        Dispatcher = Application.Current.Dispatcher;
        Timer = new WpfTimerService(100);
        Timer.TimerTicked += OnTimerTicked;
        _ = ReadChannelInfinitely(_cts.Token);
    }

    private void OnTimerTicked(DateTime dateTime)
    {
        if (Match.IsTimeStopped)
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
        HandleMessage(new StopClockMessage(Match.TimeInDecyseconds));
        HandleMessage(new UseSoundEffectMessage());
    }

    private void UpdateClock(DateTime dateTime)
    {
        Match.TimeInDecyseconds = (long)Math.Round((dateTime - LastResumeTimestamp).TotalSeconds * 10) + DecysecondsOnLastTimeStop;
        OnPropertyChanged(nameof(Match));
    }

    private void CleanUpSuspensions()
        => Match.CleanUpSuspensions();

    private void HandleMessage(IUpdateMessage message)
    {
        message.HandleSelf(this);
        OnPropertyChanged(nameof(Match));
    }

    public void Connect()
    {
        ConnectionWindow window = new(_channel, _cts.Token);
        bool result = window.ShowDialog() ?? false;
        if (!result)
        {
            MessageBox.Show("Połączenie nie powiodło się. Spróbuj ponownie!");
            Connect();
            return;
        }
        Client = window.ViewModel.Client;
    }

    private async Task ReadChannelInfinitely(CancellationToken cancellationToken)
    {
        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var msg = await _channel.Reader.ReadAsync(cancellationToken);
                Dispatcher.Invoke(() => HandleMessage(msg));
            }
        }
        catch { }
    }
}
