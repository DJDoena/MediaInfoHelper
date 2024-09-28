namespace DoenaSoft.MediaInfoHelper.Helpers;

/// <summary>
/// Extension class for <see cref="DateTime"/>.
/// </summary>
public static class DateTimeConformer
{
    /// <summary>
    /// Rounds <paramref name="value"/> down to a whole second.
    /// </summary>
    public static DateTime Conform(this DateTime value)
        => value.AddTicks(-(value.Ticks % TimeSpan.TicksPerSecond)).ToUniversalTime();
}