namespace DataFlow
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            await DataFlowExplore.ExecuteAsync();
            Console.WriteLine("Hello, World!");
        }
    }
}
