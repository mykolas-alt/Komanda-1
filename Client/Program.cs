global using Microsoft.AspNetCore.Components.Authorization;
global using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Projektas.Client;
using Projektas.Client.Services;
using Projektas.Shared.Models;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress=new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<AccountServices>();

builder.Services.AddScoped<AuthenticationStateProvider, AccountAuthStateProvider>();
builder.Services.AddAuthorizationCore();

builder.Services.AddBlazoredLocalStorage();

await builder.Build().RunAsync();
