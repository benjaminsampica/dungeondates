using DotNet.Testcontainers.Builders;
using DungeonDates.Function.Infrastructure.Databases;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

if (builder.Environment.IsDevelopment())
{
    AppContext.SetSwitch("Azure.Core.DisableSslCertificateValidation", true);
    
    var databaseContainer = new ContainerBuilder()
        .WithImage("mcr.microsoft.com/cosmosdb/linux/azure-cosmos-emulator:vnext-preview")
        .WithPortBinding(1234, 1234)
        .WithPortBinding(8081, 8081)
        .WithWaitStrategy(Wait.ForUnixContainer().UntilHttpRequestIsSucceeded(r => r.ForPort(1234)))
        .WithCommand("--protocol", "https")
        .WithReuse(true)
        .Build();

    await databaseContainer.StartAsync();
    
    var hostPort = databaseContainer.GetMappedPublicPort(8081);
    var connectionString = $"AccountEndpoint=https://localhost:{hostPort}/;AccountKey=C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==;";

    builder.Configuration.AddInMemoryCollection(
    [
        new("ConnectionStrings:DungeonDatesDatabase", connectionString)
    ]);
}

builder.Services.AddDbContext<DungeonDatesDbContext>(options =>
{
    options.UseCosmos(builder.Configuration.GetConnectionString("DungeonDatesDatabase")!, "DungeonDates", options =>
    {
        if (builder.Environment.IsDevelopment())
        {
            options.ConnectionMode(ConnectionMode.Gateway);
            options.HttpClientFactory(() => new(new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            }));
        }
    });
});


var app = builder.Build();

if (builder.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    
    var dbContext = scope.ServiceProvider.GetRequiredService<DungeonDatesDbContext>();

    await dbContext.Database.EnsureCreatedAsync();
}

await app.RunAsync();
