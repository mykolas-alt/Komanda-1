using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Projektas.Server.Database;
using Projektas.Shared.Models;
using Projektas.Shared.Enums;
using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;

namespace Projektas.Tests.Server_Tests.Controllers
{
    public class AimTrainerControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Program> _factory;

        public AimTrainerControllerTests(CustomWebApplicationFactory<Program> factory)
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

            var userScore = new UserScoreDto<AimTrainerData>
            {
                Username = "johndoe",
                GameData = new AimTrainerData
                {
                    Scores = 19,
                    Difficulty = GameDifficulty.Normal
                }
            };

            var response = await _client.PostAsJsonAsync("/api/aimtrainer/save-score", userScore);

            response.EnsureSuccessStatusCode();

            using (var scope = _factory.Services.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<UserDbContext>();

                var newUserScore = await db.AimTrainerScores
                    .Include(s => s.User)
                    .FirstOrDefaultAsync(u => u.GameData.Scores == userScore.GameData.Scores
                                              && u.GameData.Difficulty == userScore.GameData.Difficulty
                                              && u.User.Username == userScore.Username);

                Assert.NotNull(newUserScore);
                Assert.Equal(userScore.GameData.Scores, newUserScore.GameData.Scores);
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

            string username = "jakedoe";

            var response = await _client.GetAsync($"/api/aimtrainer/highscore?username={username}");
            response.EnsureSuccessStatusCode();
            UserScoreDto<AimTrainerData> highscore = await response.Content.ReadFromJsonAsync<UserScoreDto<AimTrainerData>>();

            using (var scope = _factory.Services.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<UserDbContext>();

                var actualUserHighscore = await db.AimTrainerScores
                    .AsNoTracking()
                    .Include(s => s.User)
                    .Where(u => u.User.Username == username)
                    .Select(u => new UserScoreDto<AimTrainerData>
                    {
                        Username = u.User.Username,
                        GameData = u.GameData
                    })
                    .OrderByDescending(s => s.GameData.Scores)
                    .FirstOrDefaultAsync();

                Assert.NotNull(actualUserHighscore);
                Assert.Equal(actualUserHighscore.Username, highscore.Username);
                Assert.Equal(actualUserHighscore.GameData.Scores, highscore.GameData.Scores);
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

            int topCount = 3;

            var response = await _client.GetAsync($"/api/aimtrainer/top-score?topCount={topCount}");
            response.EnsureSuccessStatusCode();
            List<UserScoreDto<AimTrainerData>>? topScores = await response.Content.ReadFromJsonAsync<List<UserScoreDto<AimTrainerData>>>();

            Assert.NotNull(topScores);
            Assert.Equal(topCount, topScores.Count);

            using (var scope = _factory.Services.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<UserDbContext>();

                var actualTopScores = await db.AimTrainerScores
                    .Include(s => s.User)
                    .OrderByDescending(s => s.GameData.Scores)
                    .Take(topCount)
                    .Select(s => new UserScoreDto<AimTrainerData>
                    {
                        Username = s.User.Username,
                        GameData = new AimTrainerData
                        {
                            Scores = s.GameData.Scores,
                            Difficulty = s.GameData.Difficulty
                        }
                    })
                    .ToListAsync();

                for (int i = 0; i < topCount; i++)
                {
                    Assert.Equal(actualTopScores[i].GameData.Scores, topScores[i].GameData.Scores);
                    Assert.Equal(actualTopScores[i].GameData.Difficulty, topScores[i].GameData.Difficulty);
                    Assert.Equal(actualTopScores[i].Username, topScores[i].Username);
                }
            }
        }
    }
}