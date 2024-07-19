using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.Json.Nodes;
using IziHardGames.IziAsyncEnumerables;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using IziHardGames.Playgrounds.ForEfCore;
using IziHardGames.ForUnsafe;

namespace QuickTest;

if(Enumerable.Range(0, 2).Contains(2))
 {
        Console.WriteLine("Fire");
 }
Console.WriteLine("Hello, World!");
JsonArray obj = JsonObject.Parse(json).AsArray();

foreach (var VARIABLE in obj)
{
    Console.WriteLine(VARIABLE["hardwareId"].GetValue<long>());
    Console.WriteLine(VARIABLE["signalId"].GetValue<long>());
}


Console.WriteLine("Complete");