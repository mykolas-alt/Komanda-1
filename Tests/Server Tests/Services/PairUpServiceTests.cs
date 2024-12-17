using Moq;
using Projektas.Server.Interfaces;
using Projektas.Server.Services;
using Projektas.Shared.Enums;
using Projektas.Shared.Models;

namespace Projektas.Tests.Services
{
    public class PairUpServiceTests
    {
        private readonly Mock<IScoreRepository> _mockScoreRepository;
        private readonly PairUpService _pairUpService;

        public PairUpServiceTests()
        {
            _mockScoreRepository = new Mock<IScoreRepository>();
            _pairUpService = new PairUpService(_mockScoreRepository.Object);
        }

        [Fact]
        public async Task AddScoreToDbAsync_ShouldCallAddScoreToUserAsync()
        {
            var data = new UserScoreDto<PairUpData>
            {
                Username = "testuser",
                GameData = new PairUpData { TimeInSeconds = 100, Fails = 2 },
                Timestamp = DateTime.UtcNow
            };

            await _pairUpService.AddScoreToDbAsync(data);

            _mockScoreRepository.Verify(repo => repo.AddScoreToUserAsync(data.Username, data.GameData, data.Timestamp), Times.Once);
        }

        [Fact]
        public async Task GetUserHighscoreAsync_ShouldReturnHighestScore()
        {
            var username = "testuser";
            var scores = new List<UserScoreDto<PairUpData>>
            {
                new UserScoreDto<PairUpData> { Username = username, GameData = new PairUpData { TimeInSeconds = 100, Fails = 2, Difficulty = GameDifficulty.Normal }, Timestamp = DateTime.UtcNow },
                new UserScoreDto<PairUpData> { Username = username, GameData = new PairUpData { TimeInSeconds = 90, Fails = 1, Difficulty = GameDifficulty.Normal }, Timestamp = DateTime.UtcNow }
            };

            _mockScoreRepository.Setup(repo => repo.GetHighscoreFromUserAsync<PairUpData>(username)).ReturnsAsync(scores);

            var result = await _pairUpService.GetUserHighscoreAsync(username, GameDifficulty.Normal);

            Assert.Equal(90, result.GameData.TimeInSeconds);
            Assert.Equal(1, result.GameData.Fails);
        }

        [Fact]
        public async Task GetTopScoresAsync_ShouldReturnTopScores()
        {
            var topCount = 2;
            var scores = new List<UserScoreDto<PairUpData>>
            {
                new UserScoreDto<PairUpData> { Username = "user1", GameData = new PairUpData { TimeInSeconds = 100, Fails = 2, Difficulty = GameDifficulty.Easy }, Timestamp = DateTime.UtcNow },
                new UserScoreDto<PairUpData> { Username = "user2", GameData = new PairUpData { TimeInSeconds = 90, Fails = 1, Difficulty = GameDifficulty.Normal }, Timestamp = DateTime.UtcNow },
                new UserScoreDto<PairUpData> { Username = "user3", GameData = new PairUpData { TimeInSeconds = 80, Fails = 0, Difficulty = GameDifficulty.Normal }, Timestamp = DateTime.UtcNow }
            };

            _mockScoreRepository.Setup(repo => repo.GetAllScoresAsync<PairUpData>()).ReturnsAsync(scores);

            var result = await _pairUpService.GetTopScoresAsync(topCount, GameDifficulty.Normal);

            Assert.Equal(2, result.Count);
            Assert.Equal(80, result[0].GameData.TimeInSeconds);
            Assert.Equal(90, result[1].GameData.TimeInSeconds);
        }
    }
}