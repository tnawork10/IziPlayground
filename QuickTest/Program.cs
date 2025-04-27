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
        var json = JsonSerializer.Serialize(new { Span = TimeSpan.MaxValue });
        var node = JsonObject.Parse(json);
        var span = node["Span"];
        var val = span.Deserialize<TimeSpan>();
    }
}
