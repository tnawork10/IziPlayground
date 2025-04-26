
using IziHardGames.Observing.Tracing;
using IziHardGames.Playgrounds.ForEfCore;
using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using WebAPI.ActionFilters;
using WebAPI.Middlewares;

namespace WebAPI
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

            var cs = $"server={server};uid={uid};pwd={pwd}{(port is null ? string.Empty : portVal)};database=PlaygroundSelfHierarchy";

            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContextPool<PlaygroundSelfHierarchyDbContext>(x => x.UseNpgsql(cs));

            // Add services to the container.

            builder.Services.AddControllers(x => x.Filters.Add<DisposableActionFilter>());
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddOpenTelemetry()
                .WithLogging(x => x.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("Playground WebApi")).AddOtlpExporter())
                .WithMetrics(x => x.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("Playground WebApi")).AddAspNetCoreInstrumentation().AddOtlpExporter())
                .WithTracing(x => x.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("Playground WebApi")).AddAspNetCoreInstrumentation().AddOtlpExporter());

            //builder.Services.AddZipkin(new OtlpParams()
            //{
            //    HostName = "localhost",
            //    MainSourceName = "IziPlayGround.WebAPI.Source",
            //    ServiceName = "IziPlayGround.WebAPI.Service",
            //});

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseMiddleware<MiddlewareExplorer>();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
