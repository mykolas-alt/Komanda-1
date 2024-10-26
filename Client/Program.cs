global using Microsoft.AspNetCore.Components.Authorization;
global using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Projektas.Client;
using Projektas.Client.Services;
using Projektas.Shared.Models;
using Blazored.LocalStorage;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress=new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<AccountService>();
builder.Services.AddScoped<MathGameService>();
builder.Services.AddScoped<GameStateService>();
builder.Services.AddScoped<TimerService>();
builder.Services.AddScoped<DataService>();
builder.Services.AddScoped<ScoreboardService>();
builder.Services.AddScoped<AuthenticationStateProvider, AccountAuthStateProvider>();
builder.Services.AddAuthorizationCore();
builder.Services.AddBlazoredLocalStorage();

await builder.Build().RunAsync();
