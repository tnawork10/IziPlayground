
namespace QuickTest;

public class ParallelForEach
{
    internal static async Task RunForeachToArrayWrite()
    {
        var ar = new int[10000];
        var arIn = new int[10000];
        for (int i = 0; i < arIn.Length; i++)
        {
            arIn[i] = 1000 + i;
        }

        await Parallel.ForEachAsync(arIn.Select((x, index) => (x, index)), async (x, ct) =>
        {
            //await Task.Delay(1);
            await Task.CompletedTask;
            var index = x.index;
            ar[index] = x.x;
        });

        for (int i = 0; i < ar.Length; i++)
        {
            if (ar[i] != arIn[i]) throw new Exception();
        }

        Console.WriteLine($"Completed {nameof(ParallelForEach)}");
    }
}
