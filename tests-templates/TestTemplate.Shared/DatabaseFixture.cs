using Npgsql;
using Testcontainers.PostgreSql;
using Xunit.Abstractions;

namespace СommissioningService.IntegrationTests;

public class DatabaseFixture : IAsyncLifetime
{
    // private readonly ITestOutputHelper console;
    public readonly PostgreSqlContainer container;
    public const string ENV_NAME_CONN_STRING = "";
    // public readonly ITestOutputHelper console;
    public string ConnectionString => GetConnectionString();

    public DatabaseFixture()
    {
        var connString = Environment.GetEnvironmentVariable(ENV_NAME_CONN_STRING) ?? throw new NullReferenceException();
        NpgsqlConnectionStringBuilder builder = new NpgsqlConnectionStringBuilder(connString);
        container = new PostgreSqlBuilder()
            .WithDatabase(builder.Database)
            .WithPassword(builder.Password)
            .WithUsername(builder.Username)
            .WithPortBinding(builder.Port, 5432)
            .Build();
    }

    private string GetConnectionString()
    {
        if (string.IsNullOrEmpty(container.GetConnectionString()))
        {
            throw new ArgumentNullException();
        }

        return container.GetConnectionString();
    }

    public async Task InitializeAsync()
    {
        await container.StartAsync();
        // console.WriteLine(ConnectionString);
        // console.WriteLine(container.Id);
        //Environment.SetEnvironmentVariable(Constants.COMM_SERVICE_ENSURE_DATABASE_FOR_TEMPLATES, "true");
        //await TemplatesDbContext.EnsureCreatedWithNpgsql(container.GetConnectionString());
    }

    public Task DisposeAsync()
    {
        return container.StopAsync();
    }
}