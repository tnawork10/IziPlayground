using JetBrains.dotMemoryUnit;

namespace Explore.Diagnostic
{
    public class JetBraindsDotMemory
    {
        public void Do()
        {
            while (true)
            {
                Console.ReadLine();
                Console.WriteLine("dotMemory check");
                dotMemory.Check(memory =>
                {
                    //memory.("snapshot.dm");
                    Console.WriteLine("Snapshot saved as snapshot.dm");
                });
            }
        }
    }
}
