using System.Collections.ObjectModel;

namespace Keyboard.Model;

public class Match : NotifyPropertyChangedAbstract
{
    public short MaxTimeouts = 3;

    private bool _isTimeStopped = true;
    public bool IsTimeStopped
    {
        get => _isTimeStopped;
        set
        {
            _isTimeStopped = value;
            OnPropertyChanged(nameof(IsTimeStopped));
        }
    }

    private long _timeInDecyseconds = 0;
    public long TimeInDecyseconds
    {
        get => _timeInDecyseconds;
        set
        {
            _timeInDecyseconds = value;
            OnPropertyChanged(nameof(TimeInDecyseconds));
        }
    }

    private short _hostsPoints = 0;
    public short HostsPoints
    {
        get => _hostsPoints;
        set
        {
            _hostsPoints = value;
            OnPropertyChanged(nameof(HostsPoints));
            OnPropertyChanged(nameof(ActualScore));
        }
    }

    private short _guestsPoints = 0;
    public short GuestsPoints
    {
        get => _guestsPoints;
        set
        {
            _guestsPoints = value;
            OnPropertyChanged(nameof(GuestsPoints));
            OnPropertyChanged(nameof(ActualScore));
        }
    }

    public (short, short) ActualScore
        => (HostsPoints, GuestsPoints);

    public ObservableCollection<Suspension> HostsSuspensions = [];

    public ObservableCollection<Suspension> GuestsSuspensions = [];

    private short _hostsTimeoutsUsed = 0;
    public short HostsTimeoutsUsed
    {
        get => _hostsTimeoutsUsed;
        set
        {
            _hostsTimeoutsUsed = value;
            OnPropertyChanged(nameof(HostsTimeoutsUsed));
            OnPropertyChanged(nameof(HostsTimeoutsLeft));
        }
    }
    public short HostsTimeoutsLeft
        => (short)(MaxTimeouts - HostsTimeoutsUsed);

    private short _guestsTimeoutsUsed = 0;
    public short GuestsTimeoutsUsed
    {
        get => _guestsTimeoutsUsed;
        set
        {
            _guestsTimeoutsUsed = value;
            OnPropertyChanged(nameof(GuestsTimeoutsUsed));
            OnPropertyChanged(nameof(GuestsTimeoutsLeft));
        }
    }
    public short GuestsTimeoutsLeft
        => (short)(MaxTimeouts - GuestsTimeoutsUsed);
}
