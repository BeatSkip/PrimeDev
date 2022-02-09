using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using PrimeDev;
using Blazm.Hid;
using MudBlazor.Services;
using PrimeWeb;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<HidNavigator>();
builder.Services.AddScoped<PrimeManager>();
builder.Services.AddMudServices();

await builder.Build().RunAsync();
