namespace QuickTest;

public class DateTimeAndTimeSpan
{
    internal static void Run()
    {
        V3();
    }

    private static void V2()
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

    private static void V1()
    {

        var start = new DateTime(new DateOnly(2024, 08, 01), default, DateTimeKind.Unspecified);
        var end = new DateTime(new DateOnly(2024, 08, 31), default, DateTimeKind.Unspecified);
        var dif = (end - start).TotalDays;
        var difH = (end - start).TotalHours;
        Console.WriteLine();
    }
    private static void V3()
    {
        var start = new DateTime(new DateOnly(2025, 01, 01), default, DateTimeKind.Utc);

        var v1 = new ValueAtDate()
        {
            value = 1,
            // 482135
            timestampMs = (long)(start.AddMinutes(-1) - DateTime.UnixEpoch).TotalMilliseconds,
        };

        var v2 = new ValueAtDate()
        {
            // 482136
            value = 2,
            timestampMs = (long)(start - DateTime.UnixEpoch).TotalMilliseconds,
        };

        var v3 = new ValueAtDate()
        {
            // 482136
            value = 3,
            timestampMs = (long)(start.AddMinutes(1) - DateTime.UnixEpoch).TotalMilliseconds,
        };

        var v4 = new ValueAtDate()
        {
            // 482136
            value = 4,
            timestampMs = (long)(start.AddMinutes(59) - DateTime.UnixEpoch).TotalMilliseconds,
        };
        var v5 = new ValueAtDate()
        {
            value = 5,
            timestampMs = (long)(start.AddMinutes(60) - DateTime.UnixEpoch).TotalMilliseconds,
        };
        var v6 = new ValueAtDate()
        {
            value = 6,
            timestampMs = (long)(start.AddMinutes(61) - DateTime.UnixEpoch).TotalMilliseconds,
        };

        var input = new ValueAtDate[]
        {
            v1,v2, v3, v4, v5, v6,
        };


        var result = Aggregate(TimeSpan.FromMinutes(1), TimeSpan.FromHours(1), input, new Agr()).ToArray();
    }

    public static IEnumerable<ValueAtDate> Aggregate<T>(TimeSpan offset, TimeSpan period, IEnumerable<ValueAtDate> input, T aggregator) where T : IAggregator
    {
        var periodms = period.TotalMilliseconds;
        var indexCurernt = 0;

        foreach (var item in input)
        {
            var index = item.IndexAlt;
            Console.WriteLine(index);
            if (indexCurernt != index)
            {
                // close groupe
            }
            else
            {
                // open groupe
            }
        }
        throw new NotImplementedException();
    }

}

public class Agr : IAggregator
{

}

public interface IAggregator
{

}

public struct ValueAtDate
{
    public double? value;
    public long timestampMs;
    public int Index => (int)(timestampMs / TimeSpan.FromHours(1).TotalMilliseconds);
    public int IndexAlt => (int)((timestampMs - 1) / (TimeSpan.FromHours(1).TotalMilliseconds));

    public DateTime Dt => DateTime.UnixEpoch.Add(TimeSpan.FromMilliseconds(timestampMs));
}