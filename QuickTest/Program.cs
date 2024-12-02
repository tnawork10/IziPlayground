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

        var etor = GetEnumerator();
        var etor2 = GetEnumerator();
        var type = etor.GetType();
        Console.WriteLine(type.AssemblyQualifiedName);
        Console.WriteLine(type.IsValueType);
        Console.WriteLine(type.IsClass);
        Console.WriteLine(etor == etor2);


        var source = new List<int>();
        var etorList = source.GetEnumerator();
    }

    public static IEnumerable<int> GetEnumerator()
    {
        yield return 0;
    }

}