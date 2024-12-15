using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Projektas.Server.Database;
using Projektas.Shared.Enums;
using Projektas.Shared.Models;
using System.Net.Http.Json;

namespace Projektas.Tests.Server_Tests.Controllers
{
    public class AccountScoreControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Program> _factory;

        public AccountScoreControllerTests(CustomWebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = _factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });

            using (var scope = _factory.Services.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<UserDbContext>();

                db.Database.EnsureCreated();
                Seeding.InitializeTestDB(db);
            }
        }

        [Theory]
        [InlineData("math", "testuser")]
        [InlineData("aim", "testuser")]
        [InlineData("pairup", "testuser")]
        [InlineData("sudoku", "testuser")]
        public async Task GetScores_ReturnsOk(string gameType, string username)
        {
            var response = await _client.GetAsync($"/api/accountscore/scores/{gameType}?username={username}");
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            object scores = gameType switch
            {
                "math" => JsonConvert.DeserializeObject<List<UserScoreDto<MathGameData>>>(responseString)!,
                "aim" => JsonConvert.DeserializeObject<List<UserScoreDto<AimTrainerData>>>(responseString)!,
                "pairup" => JsonConvert.DeserializeObject<List<UserScoreDto<PairUpData>>>(responseString)!,
                "sudoku" => JsonConvert.DeserializeObject<List<UserScoreDto<SudokuData>>>(responseString)!,
                _ => throw new ArgumentException("Invalid game type", nameof(gameType))
            };

            Assert.NotNull(scores);
        }

        [Theory]
        [InlineData("math", "testuser")]
        [InlineData("aim", "testuser", GameDifficulty.Easy)]
        [InlineData("pairup", "testuser", GameDifficulty.Normal)]
        [InlineData("sudoku", "testuser", GameDifficulty.Hard, GameMode.FourByFour)]
        public async Task GetHighscore_ReturnsOk(string gameType, string username, GameDifficulty? difficulty = null, GameMode? mode = null)
        {
            var url = $"/api/accountscore/highscore/{gameType}?username={username}";
            if (difficulty.HasValue)
            {
                url += $"&difficulty={difficulty.Value}";
            }
            if (mode.HasValue)
            {
                url += $"&mode={mode.Value}";
            }

            var response = await _client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            var highscore = JsonConvert.DeserializeObject<GameScore>(responseString);

            Assert.NotNull(highscore);
        }

        [Theory]
        [InlineData("math", "testuser")]
        [InlineData("aim", "testuser")]
        [InlineData("pairup", "testuser")]
        [InlineData("sudoku", "testuser")]
        public async Task GetMatchesPlayed_ReturnsOk(string gameType, string username)
        {
            var response = await _client.GetAsync($"/api/accountscore/matches-played/{gameType}?username={username}");
            response.EnsureSuccessStatusCode();
            var matchesPlayed = await response.Content.ReadFromJsonAsync<int>();
            Assert.True(matchesPlayed >= 0);
        }

        [Theory]
        [InlineData("math", "testuser")]
        [InlineData("aim", "testuser")]
        [InlineData("pairup", "testuser")]
        [InlineData("sudoku", "testuser")]
        public async Task GetAverageScore_ReturnsOk(string gameType, string username)
        {
            var response = await _client.GetAsync($"/api/accountscore/average-score/{gameType}?username={username}");
            response.EnsureSuccessStatusCode();
            var averageScore = await response.Content.ReadFromJsonAsync<GameScore>();
            Assert.NotNull(averageScore);
        }

        [Theory]
        [InlineData("math", "testuser")]
        [InlineData("aim", "testuser")]
        [InlineData("pairup", "testuser")]
        [InlineData("sudoku", "testuser")]
        public async Task GetAverageScoreLast7Days_ReturnsOk(string gameType, string username)
        {
            var response = await _client.GetAsync($"/api/accountscore/average-score-last-7days/{gameType}?username={username}");
            response.EnsureSuccessStatusCode();
            var averageScores = await response.Content.ReadFromJsonAsync<List<AverageScoreDto>>();
            Assert.NotNull(averageScores);
            Assert.All(averageScores, score => Assert.NotNull(score));
        }
    }
}