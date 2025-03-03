using DungeonDates.Function.Infrastructure.Databases;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

// if (builder.Environment.IsDevelopment())
// {
//     var databaseContainer = new MsSqlBuilder()
//         .Build();
//
//     await databaseContainer.StartAsync();
//
//     builder.Configuration.AddInMemoryCollection(
//     [
//         new("ConnectionStrings:DungeonDatesDatabase", databaseContainer.GetConnectionString())
//     ]);
// }

// builder.Services.AddDbContext<DungeonDatesDbContext>(options =>
// {
//     options.UseSqlServer(builder.Configuration.GetConnectionString("DungeonDatesDatabase"), options => options.EnableRetryOnFailure());
// });


var app = builder.Build();

// using (var scope = app.Services.CreateScope())
// {
//     var dbContext = scope.ServiceProvider.GetRequiredService<DungeonDatesDbContext>();
//     
//     await dbContext.Database.MigrateAsync();
// }

await app.RunAsync();
