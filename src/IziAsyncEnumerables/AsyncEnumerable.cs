using System.Collections.Generic;
using System.Threading;

namespace IziHardGames.IziAsyncEnumerables;

public class AsyncEnumerable<T> : IAsyncEnumerable<T>
{
    private readonly ManualResetValueTaskSource<T> taskSource = new ManualResetValueTaskSource<T>();

    public async IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken ct = default)
    {
        while (!ct.IsCancellationRequested)
        {
            var result = await taskSource.GetTask().ConfigureAwait(false);
            taskSource.Reset();
            yield return result;
        }
    }

    public void SetValue(T value)
    {
        taskSource.SetResult(value);
    }
}