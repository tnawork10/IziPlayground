namespace QuickTest;

public class DateTimeAndTimeSpan
{
    internal static void Run()
    {
        V2();
    }

    public static void V2()
    {
        var start = new DateTime(new DateOnly(2024, 08, 01), default, DateTimeKind.Unspecified);
        var span = (start - DateTime.UnixEpoch);
        var span1m = (start.AddMinutes(1) - DateTime.UnixEpoch);
        var span2m = (start.AddMinutes(2) - DateTime.UnixEpoch);
        var milliseconds = span.TotalMilliseconds;
        var milliseconds1m = span1m.TotalMilliseconds;
        var milliseconds2m = span2m.TotalMilliseconds;
        Console.WriteLine($"TotalMilliseconds: {milliseconds}");
        Console.WriteLine($"TotalMilliseconds +1 min: {milliseconds1m}");
        Console.WriteLine($"TotalMilliseconds +2 min: {milliseconds1m}");
        var totalH = span.TotalHours;
        var h = TimeSpan.FromHours(span2m.TotalHours).TotalHours;
        var ms = DateTime.UnixEpoch + TimeSpan.FromHours(h);
    }

    public static void V1()
    {

        var start = new DateTime(new DateOnly(2024, 08, 01), default, DateTimeKind.Unspecified);
        var end = new DateTime(new DateOnly(2024, 08, 31), default, DateTimeKind.Unspecified);
        var dif = (end - start).TotalDays;
        var difH = (end - start).TotalHours;
        Console.WriteLine();
    }
}
