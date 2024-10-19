using BenchmarkDotNet.Attributes;

namespace BenchBcl
{
    [MemoryDiagnoser]
    public class IEnumerableAlloc
    {
        /*
         * .NET8
    | Method         | Mean             | Error          | StdDev         | Gen0   | Allocated |
    |--------------- |-----------------:|---------------:|---------------:|-------:|----------:|
    | Alloc1         |         8.029 ns |      0.1832 ns |      0.4098 ns | 0.0024 |      40 B |
    | Alloc10        |        15.468 ns |      0.1647 ns |      0.1375 ns | 0.0024 |      40 B |
    | Alloc100       |        99.765 ns |      0.5505 ns |      0.5150 ns | 0.0024 |      40 B |
    | Alloc10000     |     8,853.053 ns |     88.6697 ns |     82.9417 ns |      - |      40 B |
    | AllocTimes1000 | 8,769,300.000 ns | 79,371.3116 ns | 74,243.9741 ns |      - |   40006 B |
         */
        [Benchmark]
        public void Alloc1()
        {
            var v = GetInts(1);

            foreach (var item in v)
            {
                var c = item;
                c += item;
            }
        }
        [Benchmark]
        public void Alloc10()
        {
            var v = GetInts(10);

            foreach (var item in v)
            {
                var c = item;
                c += item;
            }
        }
        [Benchmark]
        public void Alloc100()
        {
            var v = GetInts(100);

            foreach (var item in v)
            {
                var c = item;
                c += item;
            }
        }

        [Benchmark]
        public void Alloc10000()
        {
            var v = GetInts(10000);

            foreach (var item in v)
            {
                var c = item;
                c += item;
            }
        }

        [Benchmark]
        public void AllocTimes1000()
        {
            for (int i = 0; i < 1000; i++)
            {
                Alloc10000();
            }
        }

        private IEnumerable<int> GetInts(int count)
        {
            for (int i = 0; i < count; i++)
            {
                yield return i;
            }
        }
    }
}
