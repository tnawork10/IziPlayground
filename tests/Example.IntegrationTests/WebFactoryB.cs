using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Example.IntegrationTests
{
    public class WebFactoryB : WebApplicationFactory<TestWebFactory.B.Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var servicesStr = services.Select(x => x.ServiceType.AssemblyQualifiedName).Aggregate((x, y) => x + Environment.NewLine + y);
                //throw new Exception(servicesStr);
                //var descriptor1 = services.SingleOrDefault(d => d.ServiceType == typeof(IDbContextFactory<TagDataDbContext>));

                //var dbOptionsDescriptor = services.FirstOrDefault(d => d.ServiceType == typeof(DbContextOptions<TagDataDbContext>));
                //if (dbOptionsDescriptor != null)
                //    services.Remove(dbOptionsDescriptor);

                //if (descriptor1 != null)
                //{
                //    services.Remove(descriptor1);
                //}
                //services.AddPooledDbContextFactory<TagDataDbContext>((options) =>
                //{
                //    options.UseInMemoryDatabase("TestDb");
                //});
            });
        }
    }
}
