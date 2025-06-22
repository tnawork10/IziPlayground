using System.Collections.Concurrent;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using Explore.BCL;
using Explore.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileSystemGlobbing.Internal;
using Microsoft.Extensions.Primitives;
using VisioDiagrams;

namespace QuickTest;

partial class Program
{
    static async Task Main(string[] args)
    {
        ExploreDictionary.Run();
        ExploreLinq.Run();
    }
}
