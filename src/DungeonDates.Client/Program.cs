using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using DungeonDates.Client;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var backendBaseAddress = builder.HostEnvironment.BaseAddress + "api/";
builder.Services.AddScoped(_ => new HttpClient { BaseAddress = new (backendBaseAddress) });

await builder.Build().RunAsync();