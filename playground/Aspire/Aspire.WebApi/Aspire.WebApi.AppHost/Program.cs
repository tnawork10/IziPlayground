using Aspire.Hosting;
using Microsoft.Extensions.Hosting;
using Projects;

var builder = DistributedApplication.CreateBuilder(args);
builder.AddProject<IziHardGames_Domain1>("domain1");
builder.AddProject<IziHardGames_Domain2>("domain2");
var app = builder.Build();
app.Run();
