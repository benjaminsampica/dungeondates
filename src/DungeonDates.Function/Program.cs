using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Configuration;
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

builder.Build().Run();
