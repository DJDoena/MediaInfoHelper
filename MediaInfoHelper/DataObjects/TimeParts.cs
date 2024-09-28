namespace DoenaSoft.MediaInfoHelper.DataObjects;

/// <summary/>
public readonly struct TimeParts
{
    /// <summary/>
    public ulong Days { get; }

    /// <summary/>
    public ushort Hours { get; }

    /// <summary/>
    public ushort Minutes { get; }

    /// <summary/>
    public ushort Seconds { get; }

    /// <summary/>
    public TimeParts(ulong days, ushort hours, ushort minutes, ushort seconds)
    {
        this.Days = days;
        this.Hours = hours;
        this.Minutes = minutes;
        this.Seconds = seconds;
    }

    /// <summary/>
    public void Deconstruct(out ulong days, out ushort hours, out ushort minutes, out ushort seconds)
    {
        days = this.Days;
        hours = this.Hours;
        minutes = this.Minutes;
        seconds = this.Seconds;
    }

    /// <summary/>
    public void Deconstruct(out ulong days, out ushort hours, out ushort minutes)
    {
        days = this.Days;
        hours = this.Hours;
        minutes = this.Minutes;

        if (this.Seconds >= 30)
        {
            minutes++;
        }

        if (minutes == 60)
        {
            minutes = 0;

            hours++;
        }

        if (hours == 24)
        {
            hours = 0;

            days++;
        }
    }
}