using System.Windows.Threading;

namespace Keyboard.Service.Time;

public class WpfTimerService : ITimerService
{
    private readonly DispatcherTimer _timer;
    private readonly int _intervalInMilliseconds;

    public event Action<DateTime>? TimerTicked;

    public WpfTimerService(int intervalInMilliseconds)
    {
        _timer = new DispatcherTimer();
        _intervalInMilliseconds = intervalInMilliseconds;
        _timer.Interval = TimeSpan.FromMilliseconds(intervalInMilliseconds);
        _timer.Tick += (sender, e) => TimerTicked?.Invoke(DateTime.Now);
    }

    public void Start() => _timer.Start();

    public void Stop() => _timer.Stop();
}
