using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace Keyboard.Model;

public class Match(short maxTimeouts = 3, int maxMatchTimeInDecyseconds = 18_000)
    : NotifyPropertyChangedAbstract, ICloneable
{
    [JsonInclude]
    public short MaxTimeouts = maxTimeouts;

    [JsonInclude]
    public int MaxMatchTimeInDecyseconds = maxMatchTimeInDecyseconds;

    [JsonIgnore]
    private bool _isTimeStopped = true;
    [JsonInclude]
    public bool IsTimeStopped
    {
        get => _isTimeStopped;
        set
        {
            _isTimeStopped = value;
            OnPropertyChanged(nameof(IsTimeStopped));
        }
    }

    [JsonIgnore]
    private long _timeInDecyseconds = 0;
    [JsonInclude]
    public long TimeInDecyseconds
    {
        get => _timeInDecyseconds;
        set
        {
            _timeInDecyseconds = value;
            OnPropertyChanged(nameof(TimeInDecyseconds));
        }
    }

    [JsonIgnore]
    private short _halfNumber = 1;
    [JsonInclude]
    public short HalfNumber
    {
        get => _halfNumber;
        set
        {
            _halfNumber = value;
            OnPropertyChanged(nameof(HalfNumber));
        }
    }

    [JsonIgnore]
    private short _hostsPoints = 0;
    [JsonInclude]
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

    [JsonIgnore]
    private short _guestsPoints = 0;
    [JsonInclude]
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

    [JsonIgnore]
    public (short, short) ActualScore
        => (HostsPoints, GuestsPoints);

    [JsonInclude]
    public ObservableCollection<Suspension> HostsSuspensions { get; set; } = [];

    [JsonInclude]
    public ObservableCollection<Suspension> GuestsSuspensions { get; set; } = [];

    public void CleanUpSuspensions()
    {
        for (int i = HostsSuspensions.Count - 1; i >= 0; i--)
            if (HostsSuspensions[i].EndInMatchDecyseconds < TimeInDecyseconds)
                HostsSuspensions.RemoveAt(i);

        for (int i = GuestsSuspensions.Count - 1; i >= 0; i--)
            if (GuestsSuspensions[i].EndInMatchDecyseconds < TimeInDecyseconds)
                GuestsSuspensions.RemoveAt(i);
    }

    [JsonIgnore]
    private short _hostsTimeoutsUsed = 0;
    [JsonInclude]
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
    [JsonIgnore]
    public short HostsTimeoutsLeft
        => (short)(MaxTimeouts - HostsTimeoutsUsed);

    [JsonIgnore]
    private short _guestsTimeoutsUsed = 0;
    [JsonInclude]
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
    [JsonIgnore]
    public short GuestsTimeoutsLeft
        => (short)(MaxTimeouts - GuestsTimeoutsUsed);

    public object Clone()
        => new Match()
        {
            MaxTimeouts = MaxTimeouts,
            MaxMatchTimeInDecyseconds = MaxMatchTimeInDecyseconds,
            IsTimeStopped = IsTimeStopped,
            TimeInDecyseconds = TimeInDecyseconds,
            HalfNumber = HalfNumber,
            HostsPoints = HostsPoints,
            GuestsPoints = GuestsPoints,
            HostsSuspensions = [.. HostsSuspensions],
            GuestsSuspensions = [.. GuestsSuspensions],
            HostsTimeoutsUsed = HostsTimeoutsUsed,
            GuestsTimeoutsUsed = GuestsTimeoutsUsed
        };
}
