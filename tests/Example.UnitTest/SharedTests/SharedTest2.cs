using Example.UnitTest.FixturesShared;

namespace Example.UnitTest
{
    [Collection(nameof(CollectionDef))]
    public class SharedTest2(AsyncLifetimeShared shared)
    {
        [Fact]
        public async Task Test1()
        {
            await Task.Delay(TimeSpan.FromSeconds(1));
            await File.AppendAllLinesAsync("test.log", [shared.GetHashCode().ToString()]);
        }
    }
}