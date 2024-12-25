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
        //var dt = new DateTime(new DateOnly(2024, 12, 31), new TimeOnly(23, 59, 59), DateTimeKind.Unspecified);
        //var ts = (dt - DateTimeOffset.UnixEpoch).TotalMilliseconds;
        //var span = TimeSpan.FromMilliseconds(ts);
        //var tHours = (long)span.TotalHours;
        //var spanHours = TimeSpan.FromHours(tHours);
        //var dtFinal = DateTime.MinValue + spanHours;        

        var start = new DateTime(new DateOnly(2024, 08, 01), default, DateTimeKind.Unspecified);
        var end = new DateTime(new DateOnly(2024, 08, 31), default, DateTimeKind.Unspecified);
        var dif = (end - start).TotalDays;
        var difH = (end - start).TotalHours;
        Console.WriteLine();
    }
}

public class HttpExceptionHandling
{
    internal static async Task Run()
    {
        throw new NotImplementedException();
    }
}