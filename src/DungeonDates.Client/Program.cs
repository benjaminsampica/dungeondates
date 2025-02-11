using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using DungeonDates.Client;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddRadzenComponents();

var backendBaseAddress = builder.HostEnvironment.BaseAddress + "api/";
if (builder.HostEnvironment.IsDevelopment())
{
    backendBaseAddress = "http://localhost:7057/api/";
}

builder.Services.AddScoped(_ => new HttpClient { BaseAddress = new (backendBaseAddress) });

await builder.Build().RunAsync();