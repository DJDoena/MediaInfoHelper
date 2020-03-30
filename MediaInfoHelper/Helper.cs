namespace DoenaSoft.MediaInfoHelper
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public static class Helper
    {
        public static string FormatTime(uint totalSeconds)
        {
            var (days, hours, minutes) = GetTimeParts(totalSeconds);

            var text = new StringBuilder();

            if (days > 0)
            {
                text.Append(days);
                text.Append("d ");
            }

            if ((days > 0) || (hours > 0))
            {
                text.Append(hours);
                text.Append("h ");
            }

            text.Append(minutes);
            text.Append("m");

            return text.ToString();
        }

        public static uint Sum(this IEnumerable<uint> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            uint result = 0;

            foreach (var item in source)
            {
                result += item;
            }

            return result;
        }

        public static (uint days, uint hours, uint minutes) GetTimeParts(uint totalSeconds)
        {
            var timeSpan = TimeSpan.FromSeconds(totalSeconds);

            var days = (uint)timeSpan.Days;

            var hours = (uint)timeSpan.Hours;

            var minutes = (uint)timeSpan.Minutes;

            var seonds = (uint)timeSpan.Seconds;

            if (timeSpan.Milliseconds >= 500)
            {
                seonds++;
            }

            if (seonds >= 30)
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

            return (days, hours, minutes);
        }
    }
}