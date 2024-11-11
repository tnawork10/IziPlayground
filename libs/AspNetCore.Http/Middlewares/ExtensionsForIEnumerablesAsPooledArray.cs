using System.Buffers;
using System.Runtime.InteropServices;

namespace Common.Extensions.Collections
{
    public static class ExtensionsForIEnumerablesAsPooledArray
    {
        public static PooledArray<T> ToArrayPooled<T>(this IEnumerable<T> values)
        {
            return PooledArray<T>.FromIEnumerable(values);
        }
    }


    public readonly struct PooledArray<T> : IDisposable
    {
        private readonly T[] array;
        private readonly int start;
        private readonly int length;
        public Memory<T> AsMemory => new Memory<T>(array, start, length);
        public Span<T> AsSpan => new Span<T>(array, start, length);

        public PooledArray(T[] array, int start, int length)
        {
            this.array = array;
            this.start = start;
            this.length = length;
        }
        public PooledArray(T[] array, int length)
        {
            this.array = array;
            this.start = 0;
            this.length = length;
        }
        public void Dispose()
        {
            ArrayPool<T>.Shared.Return(array);
        }

        public static PooledArray<T> FromIEnumerable(IEnumerable<T> items)
        {
            if (items.Any())
            {
                int count = items.Count();
                var rent = ArrayPool<T>.Shared.Rent(count);
                if (items is Array t)
                {
                    t.CopyTo(rent, 0);
                }
                if (items is List<T> list)
                {
                    var span = CollectionsMarshal.AsSpan(list);
                    span.CopyTo(rent);
                }
                else
                {
                    var etor = items.GetEnumerator();
                    for (int i = 0; i < count; i++)
                    {
                        etor.MoveNext();
                        rent[i] = etor.Current;
                    }
                }
                return new PooledArray<T>(rent, count);
            }
            return new PooledArray<T>(Array.Empty<T>(), 0, 0);
        }
    }
}
