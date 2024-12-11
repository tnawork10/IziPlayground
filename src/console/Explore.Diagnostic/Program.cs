using System.Diagnostics;

namespace Explore.Diagnostic
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            //var v = Task.Run(() => throw new Exception("AAA")); // throwing to events
            Debug.WriteLine("This message will appear in the Output window of Visual Studio");

            await Task.Run(async () => await Task.Delay(TimeSpan.FromDays(1)));

            //Debug.
        }
    }
}
