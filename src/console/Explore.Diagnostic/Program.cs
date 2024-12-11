using System.Diagnostics;
using Microsoft.Diagnostics.Tracing.Session;

namespace Explore.Diagnostic
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            //var v = Task.Run(() => throw new Exception("AAA")); // throwing to events
            Debug.WriteLine("This message will appear in the Output window of Visual Studio");
            var list = new List<object>();
            var v1 = Task.Run(async () =>
            {
                while (true)
                {
                    await Task.Delay(100);
                    var boxing = Tuple.Create(100, 200, 300);
                    list.Add(boxing);
                }
            });

            await Task.Run(async () => await Task.Delay(TimeSpan.FromDays(1)));



            //using var session = new TraceEventSession("MyPerformanceSession");
            //Debug.
        }
    }
}
