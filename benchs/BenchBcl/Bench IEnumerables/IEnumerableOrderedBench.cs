using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;

namespace BenchBcl.Bench_IEnumerables
{
    /// <summary>
    /*
    With list List<double.Count = 0;
    | Method                              | Mean      | Error     | StdDev    | Gen0   | Allocated |
    |------------------------------------ |----------:|----------:|----------:|-------:|----------:|
    | ItterateWithoutExplicitToCollection | 13.458 ns | 0.2763 ns | 0.2449 ns | 0.0072 |     120 B |
    | ItterateWithExplicitToCollection    |  8.984 ns | 0.0754 ns | 0.0668 ns | 0.0033 |      56 B |

    With Array<dobule>.Length = 1000;
    | Method                              | Mean     | Error    | StdDev   | Gen0   | Gen1   | Allocated |
    |------------------------------------ |---------:|---------:|---------:|-------:|-------:|----------:|
    | ItterateWithoutExplicitToCollection | 19.20 us | 0.079 us | 0.062 us | 1.1902 | 0.0610 |  19.84 KB |
    | ItterateWithExplicitToCollection    | 17.96 us | 0.359 us | 0.352 us | 1.6785 | 0.0916 |  27.61 KB |
     */
    /// </summary>
    [MemoryDiagnoser]
    public class IEnumerableOrderedBench
    {
        private readonly double[] ar;

        public IEnumerableOrderedBench()
        {
            this.ar = new double[1000];
            for (int i = 0; i < ar.Length; i++)
            {
                ar[i] = 100;
                ar[i] = ar[i] + Random.Shared.NextDouble() * ((i % 2) > 0 ? -1 : 1);
            }
        }


        [Benchmark]
        public void ItterateWithoutExplicitToCollection()
        {
            var sorted = ar.OrderBy(x => x);
            var total = 0d;
            foreach (var item in sorted)
            {
                total += item;
            }
        }


        [Benchmark]
        public void ItterateWithExplicitToCollection()
        {
            var sorted = ar.OrderBy(x => x).ToArray();
            var total = 0d;
            foreach (var item in sorted)
            {
                total += item;
            }
        }
    }
}

