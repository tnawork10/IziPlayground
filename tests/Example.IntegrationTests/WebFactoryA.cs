using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using TestWebFactory.A.Services;

namespace Example.IntegrationTests
{
    public class WebFactoryA : WebApplicationFactory<TestWebFactory.A.Program>
    {
        private HttpClient clientB;
        private HttpMessageHandler handlerB;

        public WebFactoryA(HttpClient clientB, HttpMessageHandler handlerB)
        {
            this.clientB = clientB;
            this.handlerB = handlerB;
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.AddHttpClient(TestWebFactory.A.Program.CLIENT_NAME, x =>
                {
                    // required but not actually used
                    x.BaseAddress = new Uri("http://localhost:1111");
                }).ConfigurePrimaryHttpMessageHandler(x => handlerB);

                services.AddHttpClient().ConfigureHttpClientDefaults(x =>
                {
                    x.ConfigureHttpClient(client =>
                    {
                        client.BaseAddress = new Uri("http://localhost:2222");
                    });
                    x.ConfigurePrimaryHttpMessageHandler(x => handlerB);
                });

                services.AddHttpClient<ITypedHttpClient, TypedHttpClient>(x =>
                {
                    x.BaseAddress = new Uri("http://localhost:3333");
                }).ConfigurePrimaryHttpMessageHandler(x => handlerB);
            });
        }
    }
}
