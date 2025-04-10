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
        Console.WriteLine(double.IsNormal(double.Epsilon));
        Console.WriteLine(double.IsNormal(0));
        Console.WriteLine(double.IsRealNumber(0));
        Console.WriteLine(double.IsRealNumber(double.NaN));
        Console.WriteLine(double.IsRealNumber(double.NegativeInfinity));
        Console.WriteLine(double.IsRealNumber(.5f));
        //var utcTime = time.ToUniversalTime();
        //var deltaT = time - DateTime.UnixEpoch;
    }
}
