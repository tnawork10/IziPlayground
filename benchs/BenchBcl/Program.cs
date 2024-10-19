using BenchmarkDotNet.Running;

namespace BenchBcl
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            BenchmarkRunner.Run<IEnumerableAlloc>();
            //var summary = BenchmarkRunner.Run<StringConcatenationBenchmarks>();
        }
    }
}
