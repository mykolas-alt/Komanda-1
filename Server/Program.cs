using Microsoft.AspNetCore.ResponseCompression;
using Projektas.Server.Services;
using Projektas.Server.Services.MathGame;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddHttpClient();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddControllers();
builder.Services.AddSingleton<MathGameAPIService>();
builder.Services.AddSingleton<MathCalculationService>();
builder.Services.AddSingleton<MathGenerationService>();
builder.Services.AddSingleton<DataAPIService>(provider => new DataAPIService(Path.Combine("Data", "MathGameData.txt")));
builder.Services.AddSingleton<UserService>(provider => new UserService(Path.Combine("Data","AccountData.txt")));
builder.Services.AddSingleton<ScoreboardAPIService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if(app.Environment.IsDevelopment()) {
	app.UseWebAssemblyDebugging();
} else {
	app.UseExceptionHandler("/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();


app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
