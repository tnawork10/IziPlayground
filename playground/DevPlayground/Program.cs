
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Scalar.AspNetCore;

namespace DevPlayground
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            var rb = ResourceBuilder.CreateDefault().AddService("DevPlayground");
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();
            builder.Services.AddOpenTelemetry()
                .WithTracing(x => x.SetResourceBuilder(rb).AddAspNetCoreInstrumentation().AddOtlpExporter())
                .WithMetrics(x => x.SetResourceBuilder(rb).AddAspNetCoreInstrumentation().AddProcessInstrumentation().AddRuntimeInstrumentation().AddOtlpExporter())
                .WithLogging(x => x.SetResourceBuilder(rb).AddOtlpExporter());

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {

            }

            app.MapOpenApi();
            app.MapScalarApiReference();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
