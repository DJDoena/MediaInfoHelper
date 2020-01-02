namespace DoenaSoft.MediaInfoHelper
{
    using System;

    public static class DateTimeConformer
    {
        public static DateTime Conform(this DateTime value)
            => value.AddTicks(-(value.Ticks % TimeSpan.TicksPerSecond)).ToUniversalTime();
    }
}