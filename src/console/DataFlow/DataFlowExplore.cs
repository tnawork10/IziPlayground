using System.Threading.Tasks.Dataflow;

namespace DataFlow
{
    public class DataFlowExplore
    {
        internal static async Task ExecuteAsync()
        {
            var actionBlock = new ActionBlock<int>((x) => { });
        }
    }
}
