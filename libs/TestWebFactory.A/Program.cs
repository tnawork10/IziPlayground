
using TestWebFactory.A.Services;

namespace TestWebFactory.A
{
    public class Program
    {
        public const string CLIENT_NAME = "CLIENT_B";

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddHttpClient();
            builder.Services.AddHttpClient(CLIENT_NAME);
            builder.Services.AddHttpClient<ITypedHttpClient, TypedHttpClient>();

            var app = builder.Build();



            if (app.Environment.IsDevelopment())
            {

            }
            app.UseDeveloperExceptionPage();
            app.MapControllers();
            app.Run();
        }
    }
}
