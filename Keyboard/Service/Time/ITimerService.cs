namespace Keyboard.Service.Time;

public interface ITimerService
{
    public event Action<DateTime>? TimerTicked;
    public void Start();
    public void Stop();
}
