using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Projektas.Server.Repositories;
using Projektas.Server.Services;
using Projektas.Server.Services.MathGame;
using Projektas.Server.Database;
using Microsoft.EntityFrameworkCore;
using Projektas.Server.Interfaces.MathGame;
using Projektas.Server.Interfaces;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
// Add services to the container.

builder.Services.AddHttpClient();

builder.Services.AddDbContext<UserDbContext>(options => options.UseSqlite("Data source=Database/usersdb.db"));

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddControllers();

builder.Services.AddScoped<AimTrainerService>();

builder.Services.AddSingleton<IMathGameService, MathGameService>();
builder.Services.AddSingleton<IMathCalculationService, MathCalculationService>();
builder.Services.AddSingleton<IMathGenerationService, MathGenerationService>();
builder.Services.AddScoped<IMathGameScoreboardService, MathGameScoreboardService>();

builder.Services.AddScoped<PairUpService>();

builder.Services.AddScoped<SudokuService>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped(typeof(IScoreRepository), typeof(ScoreRepository));

builder.Services.AddSingleton<UserTrackingService>();

builder.Services.AddScoped<IUserService, UserService>(provider => new UserService(
	provider.GetRequiredService<IConfiguration>(),
	provider.GetRequiredService<IUserRepository>(),
	provider.GetRequiredService<UserTrackingService>()
));
builder.Services.AddScoped<IAccountScoreService, AccountScoreService>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => {
	options.TokenValidationParameters = new TokenValidationParameters {
		ValidateIssuer = true,
		ValidateAudience = true,
		ValidateLifetime = true,
		ValidateIssuerSigningKey = true,
		ValidIssuer = builder.Configuration["Jwt:Issuer"],
		ValidAudience = builder.Configuration["Jwt:Audience"],
		IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
	};
});

builder.Services.AddAuthorization();

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

app.UseAuthentication();
app.UseAuthorization();


app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();

public partial class Program { }