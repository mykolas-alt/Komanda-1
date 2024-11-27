global using Microsoft.AspNetCore.Components.Authorization;
global using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Projektas.Client;
using Projektas.Client.Services;
using Projektas.Client.Interfaces;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<IAccountAuthStateProvider,AccountAuthStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider,AccountAuthStateProvider>();

builder.Services.AddBlazoredLocalStorage();

builder.Services.AddScoped(sp => new HttpClient {BaseAddress=new Uri(builder.HostEnvironment.BaseAddress)});

builder.Services.AddScoped<AccountService>();
builder.Services.AddScoped<IAccountService,AccountService>();

builder.Services.AddScoped<IAimTrainerService,AimTrainerService>();

builder.Services.AddScoped<IMathGameService,MathGameService>();

builder.Services.AddScoped<IPairUpService,PairUpService>();

builder.Services.AddScoped<ISudokuService,SudokuService>();

builder.Services.AddScoped<ITimerService,TimerService>();
builder.Services.AddSingleton<Random>();

await builder.Build().RunAsync();
