namespace Example.UnitTest.Fixtures
{
    public class ClassFixtureAsync : IAsyncLifetime
    {
        public async Task InitializeAsync()
        {
            await Task.CompletedTask;
        }

        public async Task DisposeAsync()
        {
            await Task.CompletedTask;
        }
    }
}
