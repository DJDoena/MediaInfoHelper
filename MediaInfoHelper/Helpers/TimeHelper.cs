using System;
using System.Collections.Generic;
using System.Text;
using DoenaSoft.MediaInfoHelper.DataObjects;

namespace DoenaSoft.MediaInfoHelper.Helpers
{
    /// <summary />
    public static class TimeHelper
    {
        /// <summary>
        /// Returns days, hours and minutes of a given time in seconds
        /// </summary>
        /// <remarks>when the remaining seconds are more or equal to 30, the  part gets rounded to the next whole number</remarks>
        public static TimeParts GetTimeParts(uint totalSeconds)
        {
            var timeSpan = TimeSpan.FromSeconds(totalSeconds);

            var days = (ulong)timeSpan.Days;

            var hours = (ushort)timeSpan.Hours;

            var minutes = (ushort)timeSpan.Minutes;

            var seconds = (ushort)timeSpan.Seconds;

            if (timeSpan.Milliseconds >= 500)
            {
                seconds++;
            }

            return new TimeParts(days, hours, minutes, seconds);
        }

        /// <summary>
        /// Formats a time in seconds into a HH:mm:ss format.
        /// </summary>
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

        /// <summary>
        /// Adds up a number of <see cref="uint"/> values.
        /// </summary>
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
    }
}