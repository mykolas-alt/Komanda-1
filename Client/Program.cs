global using Microsoft.AspNetCore.Components.Authorization;
global using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Projektas.Client;
using Projektas.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, AccountAuthStateProvider>();
builder.Services.AddScoped<AccountAuthStateProvider>();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped(sp => new HttpClient { BaseAddress=new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<AccountService>();
builder.Services.AddScoped<MathGameService>();
builder.Services.AddScoped<MathGameStateService>();
builder.Services.AddScoped<TimerService>();

await builder.Build().RunAsync();
