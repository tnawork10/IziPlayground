using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace MetricProcessing.Processing;

/// <summary>
/// Все входящие сообщения передает дальше по потоку сообщений.
/// Внутри собственного блока выполнения в зависимости от булева значения отправляет сообщение либо в левый (filter=false) либо в правый (filter=true) асинхронный обработчик.
/// Всегда дожидается выполнение обработчика   
/// </summary>
/// <typeparam name="T"></typeparam>
public class MessageBranchedAction<T>
{
    private Func<T, bool>? filter;
    private Func<T, Task>? actionLeft;
    private Func<T, Task>? actionRight;

    public void Initilize(Func<T, bool> filter, Func<T, Task> actionLeft, Func<T, Task> actionRight)
    {
        this.filter = filter;
        this.actionLeft = actionLeft;
        this.actionRight = actionRight;
    }

    public async IAsyncEnumerable<T> HandleStreamAsync(IAsyncEnumerable<T> stream, [EnumeratorCancellation] CancellationToken ct = default)
    {
        await foreach (var message in stream.ConfigureAwait(false).WithCancellation(ct))
        {
            if (filter!(message))
            {
                await actionRight!(message).ConfigureAwait(false);
            }
            else
            {
                await actionLeft!(message).ConfigureAwait(false);
            }

            yield return message;
        }
    }
}