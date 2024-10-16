using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Projektas.Client;
using Projektas.Client.Services;
using Blazored.LocalStorage;


var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<MathGameService>();
builder.Services.AddScoped<TimerService>();
builder.Services.AddScoped<DataService>();
builder.Services.AddScoped<ScoreboardService>();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<GameStateService>();

await builder.Build().RunAsync();
