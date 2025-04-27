
using IziHardGames.Observing.Tracing;
using IziHardGames.Playgrounds.ForEfCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Scalar.AspNetCore;

namespace EfCoreQuery
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

            var cs = $"server={server};uid={uid};pwd={pwd}{(port is null ? string.Empty : portVal)};database=EfCoreQuery;Include Error Detail=true";
            var csIndex = $"server={server};uid={uid};pwd={pwd}{(port is null ? string.Empty : portVal)};database=CompositeIndex;Include Error Detail=true";
            var csTypes = $"server={server};uid={uid};pwd={pwd}{(port is null ? string.Empty : portVal)};database=Types;Include Error Detail=true";

            var npsqlCsb = new NpgsqlConnectionStringBuilder(cs);
            builder.Services.AddSingleton<NpgsqlConnectionStringBuilder>(npsqlCsb);
            //builder.Services.AddZipkin(new OtlpParams() { HostName = "localhost", MainSourceName = "EfCoreQuery.Source", ServiceName = "EfCoreQuery.Service" });
            builder.Services.AddDbContextPool<TypesDbContext>(x => x.UseNpgsql(csTypes).UseSnakeCaseNamingConvention());
            builder.Services.AddDbContextPool<QueryDbContext>(x => x.UseNpgsql(cs).UseSnakeCaseNamingConvention());
            builder.Services.AddPooledDbContextFactory<QueryDbContext>(x => x.UseNpgsql(cs).UseSnakeCaseNamingConvention());
            builder.Services.AddDbContextPool<CompositeIndexScanDbContext>(x => x.UseNpgsql(csIndex).UseSnakeCaseNamingConvention());

            builder.Services.AddControllers();
            builder.Services.AddHostedService<QueryBackgroundService>();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {

            }

            app.UseSwagger(options =>
            {
                options.RouteTemplate = "/openapi/{documentName}.json";
            });
            app.MapScalarApiReference();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
