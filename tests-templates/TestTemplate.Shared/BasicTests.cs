using System.Drawing.Text;
using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;
using WebAPI;
using IntegrationTests;

namespace СommissioningService.IntegrationTests;

[Collection(nameof(AspCollection))]
public class BasicTests : IClassFixture<SharedFixture>
{
    private readonly CustomWebApplicationFactory<Program> factory;
    private readonly SharedFixture fixture;
    private readonly ITestOutputHelper console;
    private readonly HttpClient client;

    public BasicTests(SharedFixture fixture, CustomWebApplicationFactory<Program> factory, ITestOutputHelper console)
    {
        this.console = console;
        this.fixture = fixture;
        this.factory = factory;
        this.client = factory.CreateClient();
    }


    private void Action0()
    {
        console.WriteLine(fixture.databaseFixture.ConnectionString);
    }

    private async Task Action1()
    {
        var builder = new DbContextOptionsBuilder<TemplatesDbContext>();
        var opt = builder.UseNpgsql(fixture.databaseFixture.ConnectionString).Options;
        //using var context = new TemplatesDbContext(opt);

        //var e = await context.Signals.AddAsync(new { SignalType = 100, SignalId = 200 });
        //Assert.Equal(default, e.Entity.Id);
        //await context.SaveChangesAsync();
        //Assert.NotEqual(default, e.Entity.Id);

        //var response0 = await client.GetAsync($"/api/SignalTemplates/{e.Entity.Id}");
        //response0.EnsureSuccessStatusCode();
        //var fact0 = await response0.Content.ReadFromJsonAsync<ModelForSignalTemplate>();
        //Assert.NotNull(fact0);
        //Assert.Equal(e.Entity.Id, fact0.Id);
        //Assert.Equal(e.Entity.SignalId, fact0.SignalId);
        //Assert.Equal(e.Entity.SignalType, fact0.SignalType);

        //var response1 = await client.GetAsync($"/api/SignalTemplates/{0}");
        //Assert.Equal(HttpStatusCode.NotFound, response1.StatusCode);
    }

    [Fact]
    private async Task Action2()
    {
        await AspNetCoreTestingUtils.Test(factory);
    }

    [Theory]
    [InlineData("/")]
    private async Task Get_EndpointsReturnSuccessAndCorrectContentType(string url)
    {
        await Task.CompletedTask;
        // Arrange
        var baseAddress = client.BaseAddress!;
        var port = baseAddress.Port;
        // Act
        var response = await client.GetAsync(url);
        // Assert
        response.EnsureSuccessStatusCode(); // Status Code 200-299
        // Assert.Equal("text/html; charset=utf-8", response.Content.Headers.ContentType.ToString());
        Assert.Equal("this is service", await response.Content.ReadAsStringAsync());
    }
}

[CollectionDefinition(nameof(AspCollection))]
public class AspCollection : ICollectionFixture<DatabaseFixture>, ICollectionFixture<CustomWebApplicationFactory<Program>>;

// FixA
public class SharedFixture
{
    public readonly CustomWebApplicationFactory<Program> factory;
    public readonly DatabaseFixture databaseFixture;
    // public readonly ITestOutputHelper console;

    public SharedFixture(DatabaseFixture databaseFixture, CustomWebApplicationFactory<Program> factory)
    {
        this.databaseFixture = databaseFixture;
        this.factory = factory;
        // this.console = databaseFixture.console;
        factory.SetConnectionString(databaseFixture.ConnectionString);
    }
}
