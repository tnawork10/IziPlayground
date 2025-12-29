using System.Runtime.CompilerServices;

namespace IziAsyncEnumerables.Tests
{
    public class UnitTest1
    {
        [Fact]
        public async Task Should_Timeou_With_None()
        {
            var target = new AsyncEnumeratorTest();
            var eble = target.EnumerateAsync();
            var etor = eble.GetAsyncEnumerator();
            await etor.MoveNextAsync();
            var f1 = etor.Current;
            Assert.NotNull(f1.Some);
            await etor.MoveNextAsync();
            await etor.MoveNextAsync();

            await etor.MoveNextAsync();
            var f4 = etor.Current;
            Assert.Equal(10, f4.None);
            await etor.MoveNextAsync();
            await Task.Delay(TimeSpan.FromSeconds(300));
        }
    }

    internal class AsyncEnumeratorTest
    {
        //public async IAsyncEnumerable<Option> EnumerateTimeoutAsync([EnumeratorCancellation] CancellationToken ct = default)
        //{
        //    for (int i = 0; i < 10; i++)
        //    {
        //        var delay = Task.Delay(TimeSpan.FromSeconds(1));
        //    }
        //}

        public async IAsyncEnumerable<Option> EnumerateAsync([EnumeratorCancellation] CancellationToken ct = default)
        {
            yield return new Option() { Some = 1 };
            yield return new Option() { Error = 1 };
            yield return new Option() { None = 1 };

            for (int i = 0; i < 10; i++)
            {
                await Task.Delay(TimeSpan.FromSeconds(1));
                yield return new Option() { None = 10 };
                var v = i;
            }
        }

        public class Option
        {
            public int? Some { get; set; }
            public int? None { get; set; }
            public int? Error { get; set; }
        }
    }
}