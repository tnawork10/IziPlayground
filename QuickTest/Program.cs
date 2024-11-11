using System.Collections.Concurrent;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace QuickTest;

class Program
{
    static async Task Main(string[] args)
    {
        var date = new DateTime(2024, 12, 30);
        Console.WriteLine(DateOnly.FromDateTime(date).ToString());
        Console.WriteLine(JsonSerializer.Serialize(DateOnly.FromDateTime(date)));

    }


}