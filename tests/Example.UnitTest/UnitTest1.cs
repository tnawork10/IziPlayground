using System.Threading.Tasks;
using Example.UnitTest.Fixtures;

namespace Example.UnitTest
{
    /// <summary>
    ///  Each <see cref="ClassFixtureAsync"/> is new instance
    /// </summary>
    /// <param name="classFixtureAsync"></param>
    public class UnitTest1(ClassFixtureAsync classFixtureAsync) : IClassFixture<ClassFixtureAsync>
    {
        [Fact]
        public async Task Test1()
        {
            //await File.AppendAllLinesAsync("test.log", [classFixtureAsync.GetHashCode().ToString()]);
        }
    }
}