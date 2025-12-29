using System.Collections.Concurrent;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using IziHardGames.Microsoft.Word;
using IziHardGames.SVG;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileSystemGlobbing.Internal;
using Microsoft.Extensions.Primitives;
using VisioDiagrams;

namespace QuickTest;

partial class Program
{
    static unsafe async Task Main(string[] args)
    {
        Console.WriteLine(JsonSerializer.Serialize(TimeSpan.FromDays(35)));

    }
}
