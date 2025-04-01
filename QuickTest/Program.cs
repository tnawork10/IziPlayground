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

        var v = Task.Run(() =>
        {
            Task.Delay(TimeSpan.FromSeconds(5)).Wait();
            Console.WriteLine("Done sleep");
            return 100;
        });

        Console.WriteLine("Begin wait");
        await v;
        //var v1 = v.Result;
        Console.WriteLine("Finish");
    }
}
