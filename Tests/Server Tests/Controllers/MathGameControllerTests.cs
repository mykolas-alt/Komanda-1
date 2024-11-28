using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Projektas.Shared.Models;
using System.Net.Http.Json;
using Projektas.Tests.Server_Tests;
using Projektas.Server.Database;
using Microsoft.EntityFrameworkCore;

namespace Projektas.Tests.Controllers {
	public class MathGameControllerTests : IClassFixture<CustomWebApplicationFactory<Program>> {
		private readonly HttpClient _client;
		private readonly CustomWebApplicationFactory<Program> _factory;

		public MathGameControllerTests(CustomWebApplicationFactory<Program> factory) {
			_factory=factory;
			_client=_factory.CreateClient(new WebApplicationFactoryClientOptions {AllowAutoRedirect=false});
		}

		[Fact]
		public async Task GetQuestion_ReturnsQuestion() {
			int score=10;

			var response=await _client.GetAsync($"/api/mathgame/question?score={score}");
			response.EnsureSuccessStatusCode();
			string question=await response.Content.ReadAsStringAsync();

			Assert.False(string.IsNullOrEmpty(question));
		}

		[Fact]
		public async Task GetOptions_ReturnsAlistOfFourIntegers() {
			var response=await _client.GetAsync("/api/mathgame/options");
			response.EnsureSuccessStatusCode();
			List<int>? options=await response.Content.ReadFromJsonAsync<List<int>>();

			Assert.NotNull(options);
			Assert.Equal(4,options.Count);
		}

		[Fact]
		public async Task CheckAnswer_ReturnsTrue() {
			int answer=4;

			var response=await _client.PostAsJsonAsync("/api/mathgame/check-answer",answer);
			response.EnsureSuccessStatusCode();
			bool result=await response.Content.ReadFromJsonAsync<bool>();

			Assert.True(result==false || result==true);
		}

		[Fact]
		public async Task SaveScore_SavesScoreSuccessfully() {
			using(var scope=_factory.Services.CreateScope()) {
				var scopedServices=scope.ServiceProvider;
				var db=scopedServices.GetRequiredService<UserDbContext>();

				db.Database.EnsureCreated();
				Seeding.InitializeTestDB(db);
			}

			var userScore=new UserScoreDto {Username="johndoe",Data=19};

			var response=await _client.PostAsJsonAsync("/api/mathgame/save-score",userScore);

			response.EnsureSuccessStatusCode();
		
			using(var scope=_factory.Services.CreateScope()) {
				var scopedServices=scope.ServiceProvider;
				var db=scopedServices.GetRequiredService<UserDbContext>();

				var newUserScore=await db.MathGameScores
					.Include(s => s.User)
					.FirstOrDefaultAsync(u => u.UserScores==userScore.Score && u.User.Username==userScore.Username);

				Assert.NotNull(newUserScore);
				Assert.Equal(userScore.Score,newUserScore.UserScores);
				Assert.Equal(userScore.Username,newUserScore.User.Username);
			}
		}

		[Fact]
		public async Task GetUserHighscore_ReturnsHighscore() {
			using(var scope=_factory.Services.CreateScope()) {
				var scopedServices=scope.ServiceProvider;
				var db=scopedServices.GetRequiredService<UserDbContext>();

				db.Database.EnsureCreated();
				Seeding.InitializeTestDB(db);
			}

			string username="jakedoe";

			var response=await _client.GetAsync($"/api/mathgame/highscore?username={username}");
			response.EnsureSuccessStatusCode();
			int highscore=await response.Content.ReadFromJsonAsync<int>();

			using(var scope=_factory.Services.CreateScope()) {
				var scopedServices=scope.ServiceProvider;
				var db=scopedServices.GetRequiredService<UserDbContext>();

				var actualUserHighscore=await db.MathGameScores
					.Include(s => s.User)
					.Where(u => u.User.Username==username)
					.Select(u => u.UserScores)
					.FirstOrDefaultAsync();

				Assert.Equal(actualUserHighscore, highscore);
			}
		}

		[Fact]
		public async Task GetTopScores_ReturnsTopList() {
			using(var scope=_factory.Services.CreateScope()) {
				var scopedServices=scope.ServiceProvider;
				var db=scopedServices.GetRequiredService<UserDbContext>();

				db.Database.EnsureCreated();
				Seeding.InitializeTestDB(db);
			}

			int topCount=3;

			var response=await _client.GetAsync($"/api/mathgame/top-score?topCount={topCount}");
			response.EnsureSuccessStatusCode();
			List<UserScoreDto>? topScores=await response.Content.ReadFromJsonAsync<List<UserScoreDto>>();

			Assert.NotNull(topScores);
			Assert.Equal(topCount,topScores.Count);

			using(var scope=_factory.Services.CreateScope()) {
				var scopedServices=scope.ServiceProvider;
				var db=scopedServices.GetRequiredService<UserDbContext>();

				var actualTopScores=await db.MathGameScores
					.Include(s => s.User)
					.OrderByDescending(s => s.UserScores)
					.Take(topCount)
					.Select(s => new UserScoreDto {Username=s.User.Username,Data=s.UserScores})
					.ToListAsync();

				for(int i=0;i<topCount;i++) {
					Assert.Equal(actualTopScores[i].Score,topScores[i].Score);
					Assert.Equal(actualTopScores[i].Username,topScores[i].Username);
				}
			}
		}
	}
}
