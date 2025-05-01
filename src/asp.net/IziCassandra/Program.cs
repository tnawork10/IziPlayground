using IziCassandra.Application;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Scalar.AspNetCore;

namespace IziCassandra
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddSingleton<CassandraClient>();
            builder.Services.AddSwaggerGen();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddOpenTelemetry()
                            .WithTracing(x => x.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("IziCassandra"))
                                               .AddAspNetCoreInstrumentation()
                                               .AddSource(ActivityProj.SOURCE_NAME)
                                               .AddOtlpExporter()
                                               .AddZipkinExporter());
            //builder.Services.AddZipkin(new OtlpParams()
            //{
            //    HostName = "localhost",
            //    MainSourceName = ZnModuleActivity.SOURCE_NAME,
            //    ServiceName = ZnModuleActivity.SERVICE_NAME,
            //    SubSourcesNames = new string[] { ZnModuleActivity.SOURCE_NAME}
            //});

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

            app.MapGet("/", context =>
            {
                context.Response.Redirect("/scalar/v1");
                return Task.CompletedTask;
            });
            app.Run();
        }
    }
}
