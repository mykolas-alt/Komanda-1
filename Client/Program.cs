using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Projektas.Client;
using Projektas.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress=new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<AccountServices>();

await builder.Build().RunAsync();
