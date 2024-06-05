using System;
using System.Threading.Tasks;
using System.Threading.Tasks.Sources;

namespace IziHardGames.IziAsyncEnumerables;

public  struct ManualResetValueTaskSource<T> : IValueTaskSource<T>
{
    private readonly  ManualResetValueTaskSourceCore<T> _taskSource = new();

    public short Version => _taskSource.Version;
    public ManualResetValueTaskSource()
    {
        _taskSource.RunContinuationsAsynchronously = true;
    }
    internal ValueTask<T> GetTask() => new(this, _taskSource.Version);

    internal void SetResult(T result) => _taskSource.SetResult(result);

    internal void Reset() => _taskSource.Reset();

    public T GetResult(short token) => _taskSource.GetResult(token);

    public ValueTaskSourceStatus GetStatus(short token) => _taskSource.GetStatus(token);

    public void OnCompleted(Action<object?> continuation, object? state, short token, ValueTaskSourceOnCompletedFlags flags)
        => _taskSource.OnCompleted(continuation, state, token, flags);
}