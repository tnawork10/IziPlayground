namespace Common.Extensions;

using System;

/// <summary>
/// Все кейсы, в которых нулевая минута это конец прошлого периода именуются с постфиксом '*Alt'
/// </summary>
public static class ExtensionsForDateTime
{
    public const long TICKS_PER_MINUTES_30 = TimeSpan.TicksPerMinute * 30;

    public static long ToHalfHourlyGroup(this DateTime dt, TimeSpan offset = default)
    {
        return ((dt).Ticks - offset.Ticks) / TICKS_PER_MINUTES_30;
    }

    public static long ToHalfHourlyGroupAlt(this DateTime dt)
    {
        return dt.ToHalfHourlyGroup(TimeSpan.FromMinutes(1));
    }

    public static long ToHourlyGroup(this DateTime dt, TimeSpan offset = default)
    {
        return ((dt).Ticks - offset.Ticks) / TimeSpan.TicksPerHour;
    }

    public static long ToHourlyGroupAlt(this DateTime dt)
    {
        return dt.ToHourlyGroup(TimeSpan.FromMinutes(1));
    }

    public static long ToDailyGroup(this DateTime dt)
    {
        return dt.Ticks / TimeSpan.TicksPerDay;
    }

    public static long ToDailyGroupAlt(this DateTime dt)
    {
        return (dt.Ticks - TimeSpan.FromMinutes(1).Ticks) / TimeSpan.TicksPerDay;
    }
}
