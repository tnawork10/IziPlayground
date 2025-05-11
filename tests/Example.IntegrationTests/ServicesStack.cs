namespace Example.IntegrationTests
{
    public class ServicesStack : IAsyncLifetime
    {
        public readonly WebFactoryA factoryA;
        public readonly WebFactoryB factoryB;
        public HttpClient clientA;
        public HttpClient clientB;
        private readonly HttpMessageHandler handlerB;

        public ServicesStack()
        {
            factoryB = new WebFactoryB();
            clientB = factoryB.CreateClient();
            this.handlerB = factoryB.Server.CreateHandler();
            factoryA = new WebFactoryA(clientB, handlerB);
            clientA = factoryA.CreateClient();
        }

        public Task InitializeAsync()
        {
            return Task.CompletedTask;
        }

        public Task DisposeAsync()
        {
            return Task.CompletedTask;
        }
    }
}
