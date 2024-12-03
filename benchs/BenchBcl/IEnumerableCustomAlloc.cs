using System.Collections;
using BenchmarkDotNet.Attributes;

namespace BenchBcl
{
    [MemoryDiagnoser]
    public class IEnumerableCustomAlloc
    {
        [Benchmark]
        public void Alloc()
        {

        }
    }

    public class CustomEnumerable : IEnumerable<int>
    {
        public IEnumerator<int> GetEnumerator()
        {
            return new Enumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public struct Enumerator : IEnumerator<int>
        {
            public int Current { get; }

            public bool MoveNext()
            {
                throw new NotImplementedException();
            }

            public void Reset()
            {
                throw new NotImplementedException();
            }

            object IEnumerator.Current { get; }

            public void Dispose()
            {
                throw new NotImplementedException();
            }
        }
    }
}
