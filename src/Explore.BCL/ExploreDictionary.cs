using System.Runtime.InteropServices;

namespace Explore.BCL
{
    public class ExploreDictionary
    {
        public static void Run()
        {
            var dic = new Dictionary<int, string>(2);
            dic.Add(0, "0");
            dic.Add(1, "1");
            ref var str0 = ref CollectionsMarshal.GetValueRefOrAddDefault(dic, 0, out var exists);
        }
    }
}
