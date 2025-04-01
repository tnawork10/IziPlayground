using System.Collections.Concurrent;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileSystemGlobbing.Internal;
using Microsoft.Extensions.Primitives;

namespace QuickTest;

partial class Program
{
    static async Task Main(string[] args)
    {
        var date = new DateOnly(2024, 02, 03);
        var delta = (new DateTime(2024, 02, 02) - new DateTime(2024, 02, 01)).Ticks;
        var half = TimeSpan.TicksPerMinute * 30;
        var halfH = delta / (half);
        Console.WriteLine(delta);
        Console.WriteLine(halfH);
        Console.WriteLine((new DateTime(date, default) - DateTime.UnixEpoch).Ticks / half);
        Console.WriteLine((new DateTime(date, new TimeOnly(0, 30)) - DateTime.UnixEpoch).Ticks / half);
        Console.WriteLine((new DateTime(date, new TimeOnly(1, 0)) - DateTime.UnixEpoch).Ticks / half);
        Console.WriteLine((new DateTime(date, new TimeOnly(1, 15)) - DateTime.UnixEpoch).Ticks / half);
        Console.WriteLine((new DateTime(date, new TimeOnly(1, 30)) - DateTime.UnixEpoch).Ticks / half);
        Console.WriteLine((new DateTime(date, new TimeOnly(1, 45)) - DateTime.UnixEpoch).Ticks / half);
        Console.WriteLine((new DateTime(date, new TimeOnly(2, 0)) - DateTime.UnixEpoch).Ticks / half);


        var time0 = new DateTime(DateOnly.FromDateTime(DateTime.UnixEpoch.Date), new TimeOnly(0, 0), DateTimeKind.Utc);
        var time01 = new DateTime(DateOnly.FromDateTime(DateTime.UnixEpoch.Date), new TimeOnly(0, 30), DateTimeKind.Utc);
        var time1 = new DateTime(DateOnly.FromDateTime(DateTime.UnixEpoch.Date), new TimeOnly(1, 0), DateTimeKind.Utc);
        var time2 = new DateTime(DateOnly.FromDateTime(DateTime.UnixEpoch.Date), new TimeOnly(1, 30), DateTimeKind.Utc);
        var time3 = new DateTime(DateOnly.FromDateTime(DateTime.UnixEpoch.Date), new TimeOnly(2, 0), DateTimeKind.Utc);
        var time4 = new DateTime(DateOnly.FromDateTime(DateTime.UnixEpoch.Date), new TimeOnly(2, 30), DateTimeKind.Utc);
        var time5 = new DateTime(DateOnly.FromDateTime(DateTime.UnixEpoch.Date), new TimeOnly(3, 0), DateTimeKind.Utc);

        var delta0 = time0 - DateTime.UnixEpoch;
        var delta01 = time01 - DateTime.UnixEpoch;
        var delta1 = time1 - DateTime.UnixEpoch;
        var delta2 = time2 - DateTime.UnixEpoch;
        var delta3 = time3 - DateTime.UnixEpoch;
        var delta4 = time4 - DateTime.UnixEpoch;
        var delta5 = time5 - DateTime.UnixEpoch;

        Console.WriteLine(delta0.Ticks / half);
        Console.WriteLine(delta01.Ticks / half);
        Console.WriteLine(delta1.Ticks / half);
        Console.WriteLine(delta2.Ticks / half);
        Console.WriteLine(delta3.Ticks / half);
        Console.WriteLine(delta4.Ticks / half);
        Console.WriteLine(delta5.Ticks / half);
        //var utcTime = time.ToUniversalTime();
        //var deltaT = time - DateTime.UnixEpoch;
    }
}
