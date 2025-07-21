#pragma warning disable
using BenchBcl.Bench_IEnumerables;
using BenchmarkDotNet.Running;

namespace BenchBcl
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            if (false)
            {
                BenchmarkRunner.Run<IEnumerableOrderedBench>();
            }
            BenchmarkRunner.Run<RegExBenchmarks>();
            //BenchmarkRunner.Run<IEnumerableAlloc>();
            //var summary = BenchmarkRunner.Run<StringConcatenationBenchmarks>();
        }
    }
}
