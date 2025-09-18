using System.Collections.ObjectModel;

namespace Keyboard.Model;

public class Match : NotifyPropertyChangedAbstract
{
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
        }
    }

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
        }
    }

    private short _guestsTimeoutsUsed = 0;
    public short GuestsTimeoutsUsed
    {
        get => _guestsTimeoutsUsed;
        set
        {
            _guestsTimeoutsUsed = value;
            OnPropertyChanged(nameof(GuestsTimeoutsUsed));
        }
    }
}
