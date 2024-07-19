using Microsoft.Extensions.Logging;

namespace IziHardGames.Playgrounds.ResearchBuildPaths;

public static class SomeStructExtensions
{
    public static void DoSome(this ref SomeStruct str)
    {
        str.value0 = Int32.MaxValue;
        str.value1 = Int32.MinValue;
        str.value2 = Int32.MaxValue;
        str.value3 = Int32.MinValue;
    }
}

public struct SomeStruct
{
    public int value0;
    public int value1;
    public int value2;
    public int value3;
}

class Program
{
    static void Main(string[] args)
    {
        SomeStruct s = new();
        s.DoSome();
    }

    private static void PrintDirContent(string path)
    {
        var info = new DirectoryInfo(path);
        if (!info.Exists) throw new DirectoryNotFoundException(path);
        Console.WriteLine($"Dirs: {info.GetDirectories().Length}");
        foreach (var dir in info.GetDirectories())
        {
            Console.WriteLine(dir.Name);
        }

        Console.WriteLine($"Files:{info.GetFiles().Length}");
        foreach (var file in info.GetFiles())
        {
            Console.WriteLine(file.Name);
        }
    }
}

internal class TestLog : ILogger<TestLog>
{
    public IDisposable? BeginScope<TState>(TState state) where TState : notnull
    {
        return new Scope(state);
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return true;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        Console.WriteLine($"{logLevel}\t{eventId.Id}.{eventId.Name}\t{formatter(state, exception)}");
    }

    private class Scope : IDisposable
    {
        private readonly object state;

        public Scope(object state)
        {
            this.state = state;
            Console.WriteLine($"New Scope. State type: {state.GetType()}");
        }

        public void Dispose()
        {
        }
    }
}

internal class MyLoggerProvider : ILoggerProvider
{
    public void Dispose()
    {
    }

    public ILogger CreateLogger(string categoryName)
    {
        return new TestLog();
    }
}