
using IziHardGames.Playgrounds.ForEfCore;
using IziHardGames.Playgrounds.ForEfCore.Scenarios;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

namespace MigratingWithRawSql
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var uid = Environment.GetEnvironmentVariable("IZHG_DB_POSTGRES_USER_DEV");
            var pwd = Environment.GetEnvironmentVariable("IZHG_DB_POSTGRES_PASSWORD_DEV");
            var server = Environment.GetEnvironmentVariable("IZHG_DB_POSTGRES_SERVER_DEV");
            var port = Environment.GetEnvironmentVariable("IZHG_DB_POSTGRES_PORT_DEV");
            var portVal = $"; port={port}";
            var cs = $"server={server};uid={uid};pwd={pwd}{(port is null ? string.Empty : portVal)};database=MigratingWithRawSql;Include Error Detail=true";

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddOpenApi();
            builder.Services.AddControllers();
            builder.Services.AddDbContext<MigratingWithRawSqlDbContext>(x => x.UseNpgsql(cs));
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.MapScalarApiReference();
            }

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
