using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using DungeonDates.Client;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddMudServices();

var backendBaseAddress = builder.HostEnvironment.BaseAddress + "api/";
if (builder.HostEnvironment.IsDevelopment())
{
    backendBaseAddress = "http://localhost:7057/api/";
}

builder.Services.AddHttpClient(nameof(DungeonDates), client => client.BaseAddress = new(backendBaseAddress))
    .AddStandardResilienceHandler();

builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient(nameof(DungeonDates)));

await builder.Build().RunAsync();