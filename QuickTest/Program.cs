#pragma warning disable
using System.Text.Json.Nodes;
using IziHardGames.IziAsyncEnumerables;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Builder;


Dictionary<int, object> dic = new Dictionary<int, object>();
dic[100] = new object();
dic[120] = new object();
dic[130] = new object();

Console.WriteLine("Complete");