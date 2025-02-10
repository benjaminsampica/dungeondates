using Azure.Data.Tables;
using DungeonDates.Function;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Testcontainers.Azurite;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

if (builder.Environment.IsDevelopment())
{
    var databaseContainer = new AzuriteBuilder()
        .Build();

    builder.Configuration.AddInMemoryCollection(
    [
        new("ConnectionStrings:StorageAccount", databaseContainer.GetConnectionString())
    ]);
}

builder.Services.AddAzureClients(clientBuilder =>
{
    clientBuilder.AddTableServiceClient(builder.Configuration.GetConnectionString("StorageAccount")).WithName("table");
});

var app = builder.Build();

if (builder.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();

    var tableServiceClient = scope.ServiceProvider.GetRequiredService<IAzureClientFactory<TableServiceClient>>();

    await tableServiceClient.CreateClient("table").CreateTableIfNotExistsAsync(nameof(DungeonDate));
}

await app.RunAsync();
