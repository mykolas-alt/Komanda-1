using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Projektas.Server.Database;
using Projektas.Shared.Models;
using Projektas.Shared.Enums;
using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;

namespace Projektas.Tests.Server_Tests.Controllers
{
    public class PairUpControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Program> _factory;

        internal PairUpControllerTests(CustomWebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = _factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });
        }

        [Fact]
        public async Task SaveScore_SavesScoreSuccessfully()
        {
            using (var scope = _factory.Services.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<UserDbContext>();

                db.Database.EnsureCreated();
                Seeding.InitializeTestDB(db);
            }

            var userScore = new UserScoreDto<PairUpData>
            {
                Username = "johndoe",
                GameData = new PairUpData
                {
                    TimeInSeconds = 30,
                    Fails = 5,
                    Difficulty = GameDifficulty.Normal
                }
            };

            var response = await _client.PostAsJsonAsync("/api/pairup/save-score", userScore);

            response.EnsureSuccessStatusCode();

            using (var scope = _factory.Services.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<UserDbContext>();

                var newUserScore = await db.PairUpScores
                    .Include(s => s.User)
                    .FirstOrDefaultAsync(u => u.GameData.TimeInSeconds == userScore.GameData.TimeInSeconds
                                              && u.GameData.Fails == userScore.GameData.Fails
                                              && u.GameData.Difficulty == userScore.GameData.Difficulty
                                              && u.User.Username == userScore.Username);

                Assert.NotNull(newUserScore);
                Assert.Equal(userScore.GameData.TimeInSeconds, newUserScore.GameData.TimeInSeconds);
                Assert.Equal(userScore.GameData.Fails, newUserScore.GameData.Fails);
                Assert.Equal(userScore.GameData.Difficulty, newUserScore.GameData.Difficulty);
                Assert.Equal(userScore.Username, newUserScore.User.Username);
            }
        }

        [Fact]
        public async Task GetUserHighscore_ReturnsHighscore()
        {
            using (var scope = _factory.Services.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<UserDbContext>();

                db.Database.EnsureCreated();
                Seeding.InitializeTestDB(db);
            }

            string username = "johndoe";
            GameDifficulty difficulty = GameDifficulty.Normal;

            var response = await _client.GetAsync($"/api/pairup/highscore?username={username}&difficulty={difficulty}");
            response.EnsureSuccessStatusCode();

            UserScoreDto<PairUpData>? highscore = await response.Content.ReadFromJsonAsync<UserScoreDto<PairUpData>>();

            Assert.NotNull(highscore);

            using (var scope = _factory.Services.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<UserDbContext>();

                var actualUserHighscore = await db.PairUpScores
                    .AsNoTracking()
                    .Include(s => s.User)
                    .Where(u => u.User.Username == username && u.GameData.Difficulty == difficulty)
                    .Select(u => new UserScoreDto<PairUpData>
                    {
                        Username = u.User.Username,
                        GameData = u.GameData
                    })
                    .OrderBy(s => s.GameData.TimeInSeconds)
                    .ThenBy(s => s.GameData.Fails)
                    .FirstAsync();

                Assert.NotNull(actualUserHighscore);
                Assert.Equal(actualUserHighscore.Username, highscore.Username);
                Assert.Equal(actualUserHighscore.GameData.TimeInSeconds, highscore.GameData.TimeInSeconds);
                Assert.Equal(actualUserHighscore.GameData.Fails, highscore.GameData.Fails);
                Assert.Equal(actualUserHighscore.GameData.Difficulty, highscore.GameData.Difficulty);
            }
        }

        [Fact]
        public async Task GetTopScores_ReturnsTopList()
        {
            using (var scope = _factory.Services.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<UserDbContext>();

                db.Database.EnsureCreated();
                Seeding.InitializeTestDB(db);
            }

            int topCount = 2;
            GameDifficulty difficulty = GameDifficulty.Normal;

            var response = await _client.GetAsync($"/api/pairup/top-score?topCount={topCount}&difficulty={difficulty}");
            response.EnsureSuccessStatusCode();
            List<UserScoreDto<PairUpData>>? topScores = await response.Content.ReadFromJsonAsync<List<UserScoreDto<PairUpData>>>();

            Assert.NotNull(topScores);
            Assert.Equal(topCount, topScores.Count);

            using (var scope = _factory.Services.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<UserDbContext>();

                var actualTopScores = await db.PairUpScores
                    .Include(s => s.User)
                    .Where(s => s.GameData.Difficulty == difficulty)
                    .OrderBy(s => s.GameData.TimeInSeconds)
                    .ThenBy(s => s.GameData.Fails)
                    .Take(topCount)
                    .Select(s => new UserScoreDto<PairUpData>
                    {
                        Username = s.User.Username,
                        GameData = new PairUpData
                        {
                            TimeInSeconds = s.GameData.TimeInSeconds,
                            Fails = s.GameData.Fails,
                            Difficulty = s.GameData.Difficulty
                        }
                    })
                    .ToListAsync();

                for (int i = 0; i < topCount; i++)
                {
                    Assert.Equal(actualTopScores[i].GameData.TimeInSeconds, topScores[i].GameData.TimeInSeconds);
                    Assert.Equal(actualTopScores[i].GameData.Fails, topScores[i].GameData.Fails);
                    Assert.Equal(actualTopScores[i].GameData.Difficulty, topScores[i].GameData.Difficulty);
                    Assert.Equal(actualTopScores[i].Username, topScores[i].Username);
                }
            }
        }
    }
}