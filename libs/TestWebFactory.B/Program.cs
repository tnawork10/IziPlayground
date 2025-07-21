using Microsoft.EntityFrameworkCore;

namespace TestWebFactory.B
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var uid = Environment.GetEnvironmentVariable("IZHG_DB_POSTGRES_USER_DEV");
            var pwd = Environment.GetEnvironmentVariable("IZHG_DB_POSTGRES_PASSWORD_DEV");
            var server = Environment.GetEnvironmentVariable("IZHG_DB_POSTGRES_SERVER_DEV");
            var port = Environment.GetEnvironmentVariable("IZHG_DB_POSTGRES_PORT_DEV");
            var portVal = $";port={port}";

            var cs = $"server={server};uid={uid};pwd={pwd}{(port is null ? string.Empty : portVal)};database=dml_efcore";

            var csPool = $"server={server};uid={uid};pwd={pwd}{(port is null ? string.Empty : portVal)};database=pooled";
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();

            using var loggerFactory = LoggerFactory.Create(logging =>
            {
                logging.AddConfiguration(builder.Configuration.GetSection("Logging"));
                logging.AddConsole();
            });

            var logger = loggerFactory.CreateLogger("Startup");
            var types0 = builder.Services.Select(x => x.ServiceType).ToArray();
            //var servicesAtStart = builder.Services.Select(x => x.ServiceType.AssemblyQualifiedName).Aggregate((x, y) => x + Environment.NewLine + y);
            //logger.LogInformation("At start:" + Environment.NewLine + servicesAtStart);
            builder.Services.AddHttpClient();
            var typesAfterHttpClient = builder.Services.Select(x => x.ServiceType).ToArray();
            var diff = typesAfterHttpClient.Except(types0).ToArray();
            var servicesAfterHttpClient = diff.Select(x => x.AssemblyQualifiedName).Aggregate((x, y) => x + Environment.NewLine + y);
            //var servicesAfterHttpClient = builder.Services.Select(x => x.ServiceType.AssemblyQualifiedName).Aggregate((x, y) => x + Environment.NewLine + y);
            logger.LogInformation("After AddHttpClient():" + Environment.NewLine + servicesAfterHttpClient);

            //builder.Services.AddPooledDbContextFactory<PlaygroundDbContext>(x => x.UseNpgsql(cs));
            var typesAfterDbContext = builder.Services.Select(x => x.ServiceType).ToArray();
            var diff2 = typesAfterDbContext.Except(typesAfterHttpClient).ToArray();
            var servicesAfterDbContext = diff2.Select(x => x.AssemblyQualifiedName).Aggregate((x, y) => x + Environment.NewLine + y);
            logger.LogInformation("After AddPooledDbContextFactory():" + Environment.NewLine + servicesAfterDbContext);

            var app = builder.Build();
            app.MapControllers();
            app.Run();
        }
    }
}
