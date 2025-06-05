using IziHardGames.TestAppDomain;

namespace IziHardGames.Domain2
{
    public class Class2
    {
        public static string Run()
        {
            var v = new DebugRunMe();
            var version = v.ReturnVersion();
            Console.WriteLine(version);
            return version;
        }
    }
}
