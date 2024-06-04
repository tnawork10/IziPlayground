using System.Text.Json.Nodes;
using IziHardGames.IziAsyncEnumerables;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Builder;

var json = "[\n  {\n    \"hardwareId\": 0,\n    \"signalId\": 0\n  }\n]";

JsonArray obj = JsonObject.Parse(json).AsArray();

foreach (var VARIABLE in obj)
{
    Console.WriteLine(VARIABLE["hardwareId"].GetValue<long>());
    Console.WriteLine(VARIABLE["signalId"].GetValue<long>());
}


Console.WriteLine("Complete");