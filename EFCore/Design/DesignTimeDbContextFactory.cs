using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace IziHardGames.Playgrounds.ForEfCore.Design
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<DesignDbContext>
    {
        public DesignDbContext CreateDbContext(string[] args)
        {
            var uid = Environment.GetEnvironmentVariable("IZHG_DB_POSTGRES_USER_DEV");
            var pwd = Environment.GetEnvironmentVariable("IZHG_DB_POSTGRES_PASSWORD_DEV");
            var server = Environment.GetEnvironmentVariable("IZHG_DB_POSTGRES_SERVER_DEV");
            var port = Environment.GetEnvironmentVariable("IZHG_DB_POSTGRES_PORT_DEV");
            var portVal = $"; port={port}";
            var cs = $"server={server};uid={uid};pwd={pwd}{(port is null ? string.Empty : portVal)};database={nameof(DesignDbContext)}";

            var services = new ServiceCollection();

            services.AddLogging();
            var serviceProvider = services.BuildServiceProvider();

            var logger = serviceProvider.GetRequiredService<ILogger<DesignDbContext>>();

            // Load configuration from appsettings.json or any other source
            var builder = new ConfigurationBuilder() as IConfigurationBuilder;
            IConfiguration configuration = builder
                // <PackageReference Include="Microsoft.Extensions.Configuration.FileExtensions" />
                .SetBasePath(Directory.GetCurrentDirectory())
                // <PackageReference Include="Microsoft.Extensions.Configuration.Json" />
                .AddJsonFile("appsettings.json").Build();

            // Get the connection string
            var connectionString = cs;

            // Configure the DbContext
            var optionsBuilder = new DbContextOptionsBuilder<DesignDbContext>();
            optionsBuilder.UseNpgsql(connectionString);
            return new DesignDbContext(optionsBuilder.Options, logger);
        }
    }
}
