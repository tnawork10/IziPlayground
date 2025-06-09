using Example.UnitTest.Fixtures;

namespace Example.UnitTest
{
    public class UnitTest2(ClassFixtureAsync classFixtureAsync) : IClassFixture<ClassFixtureAsync>
    {
        [Fact]
        public async Task Test1()
        {
            //await Task.Delay(2000);
            //await File.AppendAllLinesAsync("test.log", [classFixtureAsync.GetHashCode().ToString()]);
        }
    }
}