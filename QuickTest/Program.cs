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


public class Program
{
    static async Task Main(params string[] arg)
    {

        //await PlaygroundDbContext.RecreateAsync();
        await CheckDbCommands.Check();
    }
}
