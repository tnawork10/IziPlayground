using System.Collections.Concurrent;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace QuickTest;

class Program
{
    static async Task Main(string[] args)
    {
        var list = new int[] { 1 };
        var order = list.OrderBy(x => x).ToArray();

    }

    private static async IAsyncEnumerable<int> AsyncStream([EnumeratorCancellation] CancellationToken ct = default)
    {
        yield return 0;
        throw new NotImplementedException();
    }


    public static IEnumerable<(string, string)> Parse(QueryString queryString)
    {
        var pairs = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(queryString.Value);
        foreach (var item in pairs)
        {
            yield return (item.Key, item.Value);
        }
    }
}