using Moq;
using Projektas.Server.Interfaces;
using Projektas.Server.Services.MathGame;
using Projektas.Shared.Models;

namespace Projektas.Tests.Services.MathGameTests
{
    public class MathGameScoreboardServiceTests
    {
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly MathGameScoreboardService _mathGameScoreBoardService;

        public MathGameScoreboardServiceTests()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _mathGameScoreBoardService = new MathGameScoreboardService(_mockUserRepository.Object);
        }

		[Fact]
		public async Task AddScoreToDb_ShouldCallAddMathGameScoreToUserAsync()
		{
			// Arrange
			var userScore = new UserScoreDto { Username = "testuser", Score = 100 };

			// Act
			await _mathGameScoreBoardService.AddScoreToDb(userScore);

			// Assert
			_mockUserRepository.Verify(repo => repo.AddMathGameScoreToUserAsync(userScore.Username, userScore.Score), Times.Once);
		}

		[Fact]
		public async Task GetUserHighscore_ShouldReturnHighscore()
		{
			// Arrange
			var username = "testuser";
			var expectedHighscore = 200;
			_mockUserRepository.Setup(repo => repo.GetMathGameHighscoreFromUserAsync(username)).ReturnsAsync(expectedHighscore);

			// Act
			var result = await _mathGameScoreBoardService.GetUserHighscore(username);

			// Assert
			Assert.Equal(expectedHighscore, result);
		}

		[Fact]
		public async Task GetTopScores_ShouldReturnTopScores()
		{
			// Arrange
			var topCount = 3;
			var userScores = new List<UserScoreDto>
			{
				new UserScoreDto { Username = "user1", Score = 300 },
				new UserScoreDto { Username = "user2", Score = 250 },
				new UserScoreDto { Username = "user3", Score = 200 },
				new UserScoreDto { Username = "user4", Score = 150 }
			};
			_mockUserRepository.Setup(repo => repo.GetAllMathGameScoresAsync()).ReturnsAsync(userScores);

			// Act
			var result = await _mathGameScoreBoardService.GetTopScores(topCount);

			// Assert
			Assert.Equal(topCount, result.Count);
			Assert.Equal(userScores[0], result[0]);
			Assert.Equal(userScores[1], result[1]);
			Assert.Equal(userScores[2], result[2]);
		}
	}
}
