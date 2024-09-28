namespace DoenaSoft.MediaInfoHelper.DataObjects.AudioBookMetaXml;

public partial class RunningTime
{
    /// <summary/>
    public static implicit operator TimeParts(RunningTime runningTime)
    {
        if (runningTime == null)
        {
            return new TimeParts();
        }
        else
        {
            ulong days;
            ushort hours;
            if (runningTime.Hours > 24)
            {
                days = runningTime.Hours / 24;
                hours = (ushort)(runningTime.Hours % 24);
            }
            else
            {
                days = 0;
                hours = (ushort)runningTime.Hours;
            }

            return new TimeParts(days, hours, runningTime.Minutes, runningTime.Seconds);
        }
    }
}