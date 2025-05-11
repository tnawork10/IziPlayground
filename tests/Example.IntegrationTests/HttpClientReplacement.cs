namespace Example.IntegrationTests
{
    public class HttpClientReplacement : IClassFixture<ServicesStack>
    {
        public HttpClientReplacement(ServicesStack pair)
        {
            this.pair = pair;
        }
        public ServicesStack pair { get; }

        [Fact]
        public async Task Named_Http_Client()
        {
            var req = await pair.clientA.GetAsync("FactoryA/RequestFactoryA");
            try
            {
                var text = await req.Content.ReadAsStringAsync();
                Assert.True(req.IsSuccessStatusCode);
                Assert.Equal("BaseAddress: http://localhost:1111/. This is factory B", text);
            }
            catch (Exception ex)
            {
                throw new Exception($"Status: {req.StatusCode} {await req.Content.ReadAsStringAsync()}{Environment.NewLine}{ex.Message}", ex);
            }
        }

        [Fact]
        public async Task Default_Http_Client()
        {
            var req = await pair.clientA.GetAsync("FactoryA/RequestFactoryADefault");
            try
            {
                var text = await req.Content.ReadAsStringAsync();
                Assert.True(req.IsSuccessStatusCode);
                Assert.Equal("BaseAddress: http://localhost:2222/. This is factory B", text);
            }
            catch (Exception ex)
            {
                throw new Exception($"Status: {req.StatusCode} {await req.Content.ReadAsStringAsync()}{Environment.NewLine}{ex.Message}", ex);
            }
        }

        [Fact]
        public async Task Typed_Http_Client()
        {
            var req = await pair.clientA.GetAsync("FactoryA/RequestFactoryATyped");
            try
            {
                var text = await req.Content.ReadAsStringAsync();
                Assert.True(req.IsSuccessStatusCode);
                Assert.Equal("BaseAddress: http://localhost:3333/. This is factory B", text);
            }
            catch (Exception ex)
            {
                throw new Exception($"Status: {req.StatusCode} {await req.Content.ReadAsStringAsync()}{Environment.NewLine}{ex.Message}", ex);
            }
        }
    }
}
