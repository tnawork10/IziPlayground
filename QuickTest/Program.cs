using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.Json.Nodes;
using IziHardGames.IziAsyncEnumerables;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Builder;


await TestAsync();


partial class Program
{
    public static async Task TestAsync()
    {
        var v = ValueTask.FromResult(100f);
        Console.WriteLine(v.GetType().GenericTypeArguments.First().FullName);
    }
}