using Moq;
using Projektas.Server.Interfaces;
using Projektas.Server.Services;
using Projektas.Shared.Models;
using Projektas.Shared.Enums;

namespace Projektas.Tests.Services
{
    public class AimTrainerServiceTests
    {
        private readonly Mock<IScoreRepository> _mockScoreRepository;
        private readonly AimTrainerService _aimTrainerService;

        public AimTrainerServiceTests()
        {
            _mockScoreRepository = new Mock<IScoreRepository>();
            _aimTrainerService = new AimTrainerService(_mockScoreRepository.Object);
        }

        [Fact]
        public async Task AddScoreToDbAsync_ShouldCallAddScoreToUserAsync()
        {
            var userScore = new UserScoreDto<AimTrainerData>
            {
                Username = "testuser",
                GameData = new AimTrainerData { Scores = 100 },
                Timestamp = DateTime.UtcNow
            };

            await _aimTrainerService.AddScoreToDbAsync(userScore);

            _mockScoreRepository.Verify(repo => repo.AddScoreToUserAsync(userScore.Username, userScore.GameData, userScore.Timestamp), Times.Once);
        }

        [Fact]
        public async Task GetUserHighscoreAsync_ShouldReturnHighestScore()
        {
            var username = "testuser";
            var scores = new List<UserScoreDto<AimTrainerData>>
            {
                new UserScoreDto<AimTrainerData> { Username = username, GameData = new AimTrainerData { Scores = 50, Difficulty = GameDifficulty.Normal }, Timestamp = DateTime.UtcNow },
                new UserScoreDto<AimTrainerData> { Username = username, GameData = new AimTrainerData { Scores = 100, Difficulty = GameDifficulty.Normal }, Timestamp = DateTime.UtcNow }
            };

            _mockScoreRepository.Setup(repo => repo.GetHighscoreFromUserAsync<AimTrainerData>(username)).ReturnsAsync(scores);

            var result = await _aimTrainerService.GetUserHighscoreAsync(username, GameDifficulty.Normal);

            Assert.Equal(100, result.GameData.Scores);
        }

        [Fact]
        public async Task GetTopScoresAsync_ShouldReturnTopScores()
        {
            var topCount = 2;
            var scores = new List<UserScoreDto<AimTrainerData>>
            {
                new UserScoreDto<AimTrainerData> { Username = "user1", GameData = new AimTrainerData { Scores = 50, Difficulty = GameDifficulty.Easy }, Timestamp = DateTime.UtcNow },
                new UserScoreDto<AimTrainerData> { Username = "user2", GameData = new AimTrainerData { Scores = 100, Difficulty = GameDifficulty.Normal }, Timestamp = DateTime.UtcNow },
                new UserScoreDto<AimTrainerData> { Username = "user3", GameData = new AimTrainerData { Scores = 75, Difficulty = GameDifficulty.Normal }, Timestamp = DateTime.UtcNow }
            };

            _mockScoreRepository.Setup(repo => repo.GetAllScoresAsync<AimTrainerData>()).ReturnsAsync(scores);

            var result = await _aimTrainerService.GetTopScoresAsync(topCount, GameDifficulty.Normal);

            Assert.Equal(2, result.Count);
            Assert.Equal(100, result[0].GameData.Scores);
            Assert.Equal(75, result[1].GameData.Scores);
        }
    }
}