using DmlEfCoreExplore.Application;
using DmlEfCoreExplore.Infrastructure;
using IziHardGames.Playgrounds.ForEfCore;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Scalar.AspNetCore;

namespace DmlEfCoreExplore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var uid = Environment.GetEnvironmentVariable("IZHG_DB_POSTGRES_USER_DEV");
            var pwd = Environment.GetEnvironmentVariable("IZHG_DB_POSTGRES_PASSWORD_DEV");
            var server = Environment.GetEnvironmentVariable("IZHG_DB_POSTGRES_SERVER_DEV");
            var port = Environment.GetEnvironmentVariable("IZHG_DB_POSTGRES_PORT_DEV");
            var portVal = $";port={port}";

            var cs = $"server={server};uid={uid};pwd={pwd}{(port is null ? string.Empty : portVal)};database=dml_efcore";
            var csPool = $"server={server};uid={uid};pwd={pwd}{(port is null ? string.Empty : portVal)};database=pooled";

            var npsqlCsb = new NpgsqlConnectionStringBuilder(cs);
            // Add services to the container.

            //builder.Services.AddZipkin(new OtlpParams()
            //{
            //    HostName = "localhost",
            //    MainSourceName = "DmlEfCoreExplore.Source",
            //    ServiceName = "DmlEfCoreExplore.Service",
            //});

            builder.Services.AddSingleton<SingletonProxy>();
            builder.Services.AddPooledDbContextFactory<PooledDbContext>(x => x.UseNpgsql(csPool).UseSnakeCaseNamingConvention());
            //builder.Services.AddDbContextPool<PooledDbContext>(x => x.UseNpgsql(csPool).UseSnakeCaseNamingConvention());
            builder.Services.AddDbContext<PooledDbContext>(x => x.UseNpgsql(csPool).UseSnakeCaseNamingConvention());

            builder.Services.AddDbContextPool<DmlResearchContext>(x => x.UseNpgsql(cs).UseSnakeCaseNamingConvention());
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger(options =>
                {
                    options.RouteTemplate = "/openapi/{documentName}.json";
                });
                app.MapScalarApiReference();
            }

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
