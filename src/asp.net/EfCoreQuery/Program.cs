
using IziHardGames.Observing.Tracing;
using IziHardGames.Playgrounds.ForEfCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

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

            var cs = $"server={server};uid={uid};pwd={pwd}{(port is null ? string.Empty : portVal)};database=EfCoreQuery";

            var npsqlCsb = new NpgsqlConnectionStringBuilder(cs);
            builder.Services.AddSingleton<NpgsqlConnectionStringBuilder>(npsqlCsb);
            builder.Services.AddZipkin(new OtlpParams() { HostName = "localhost", MainSourceName = "EfCoreQuery.Source", ServiceName = "EfCoreQuery.Service" });
            builder.Services.AddDbContextPool<QueryDbContext>(x => x.UseNpgsql(cs).UseSnakeCaseNamingConvention());

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
