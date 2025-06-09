using System.Threading.Tasks;
using Example.UnitTest.Fixtures;
using Example.UnitTest.FixturesShared;

namespace Example.UnitTest
{
    [Collection(nameof(CollectionDef))]
    public class SharedTest1(AsyncLifetimeShared shared)
    {
        [Fact]
        public async Task Test1()
        {
            await File.AppendAllLinesAsync("test.log", [shared.GetHashCode().ToString()]);
        }
    }
}