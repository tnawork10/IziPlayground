using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

namespace СommissioningService.IntegrationTests;

public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram>
    where TProgram : class
{
    private string? connectionString;

    public void SetConnectionString(string connectionString)
    {
        this.connectionString = connectionString;
    }
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("DevelopmentLocal");

        if (string.IsNullOrEmpty(connectionString)) throw new ArgumentException();
        
        builder.UseSetting("ConnectionStrings:DefaultConnection", connectionString);

        // Environment.SetEnvironmentVariable(Constants.ENV_NAME_CONN_STRING);
        //Environment.SetEnvironmentVariable(TemplatesDbContext.COMM_SERVICE_ENSURE_DATABASE_FOR_TEMPLATES, "true");
        Environment.SetEnvironmentVariable("COMM_SERVICE_URI_TRANSPORT", "http://localhost:3333/device");
        Environment.SetEnvironmentVariable("COMM_SERVICE_URI_HARDWARE", "http://localhost:32769/hardware/create");
        Environment.SetEnvironmentVariable("COMM_SERVICE_URI_SIGNALS", "http://localhost:32769/signal/create");

        builder.ConfigureAppConfiguration((hostingContext, config) =>
        {
            config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            config.AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", optional: true, reloadOnChange: true);
            config.AddEnvironmentVariables();
        });

        builder.ConfigureServices(services => { });
    }
}