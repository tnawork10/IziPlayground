
using IziHardGames.DevExtremeAPI.DAL;
using Microsoft.EntityFrameworkCore;

namespace DevextremeAPI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {

            var uid = Environment.GetEnvironmentVariable("IZHG_DB_POSTGRES_USER_DEV");
            var pwd = Environment.GetEnvironmentVariable("IZHG_DB_POSTGRES_PASSWORD_DEV");
            var server = Environment.GetEnvironmentVariable("IZHG_DB_POSTGRES_SERVER_DEV");
            var port = Environment.GetEnvironmentVariable("IZHG_DB_POSTGRES_PORT_DEV");
            var portVal = $";port={port}";

            var cs = $"server={server};uid={uid};pwd={pwd}{(port is null ? string.Empty : portVal)};database=DevExtreme";

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers().AddJsonOptions(opt =>
            {
                // pascal case
                opt.JsonSerializerOptions.PropertyNamingPolicy = null;
            });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //builder.Services.AddDbContext<DevExtremeDbContext>(x => x.UseInMemoryDatabase("DevExtreme"));
            builder.Services.AddDbContext<DevExtremeDbContext>(x => x.UseNpgsql(cs));

            var app = builder.Build();
            using (var scope = app.Services.CreateScope())
            {
                using var db = scope.ServiceProvider.GetRequiredService<DevExtremeDbContext>();
                await db.Database.EnsureCreatedAsync();
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();

            app.UseCors(x =>
            {
                x.AllowAnyHeader();
                x.AllowAnyMethod();
                x.AllowAnyOrigin();
            });

            app.MapControllers();

            app.Run();
        }
    }
}
