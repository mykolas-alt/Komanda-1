using Moq;
using Projektas.Server.Interfaces;
using Projektas.Server.Services;
using Projektas.Server.Services.MathGame;
using Projektas.Shared.Models;

namespace Projektas.Tests.Services.MathGameTests {
    public class MathGameScoreboardServiceTests {
		private readonly Mock<IScoreRepository> _mockScoreRepository;
		private readonly MathGameScoreboardService _mathGameScoreboardService;

        public MathGameScoreboardServiceTests() {
			_mockScoreRepository = new Mock<IScoreRepository>();
			_mathGameScoreboardService = new MathGameScoreboardService(_mockScoreRepository.Object);
        }

		[Fact]
		public async Task AddScoreToDb_ShouldCallAddMathGameScoreToUserAsync() {
			var userScore = new UserScoreDto<MathGameData> {
				Username = "testuser",
				GameData = new MathGameData {
					Scores = 100
				}
			};

			await _mathGameScoreboardService.AddScoreToDbAsync(userScore);

			_mockScoreRepository.Verify(
				repo => repo.AddScoreToUserAsync(
					userScore.Username,
					It.Is<MathGameData>(m => m.Scores == userScore.GameData.Scores)
					),
				Times.Once
				);
		}

		[Fact]
		public async Task GetUserHighscore_ShouldReturnHighscore() {
			var username = "testuser";
			var expectedHighscore = new List<UserScoreDto<MathGameData>> {
				new UserScoreDto<MathGameData> {
					Username = "testuser",
					GameData = new MathGameData {
						Scores = 200
					}
				}
			};
			_mockScoreRepository.Setup(repo => repo.GetHighscoreFromUserAsync<MathGameData>(username)).ReturnsAsync(expectedHighscore);

			var result = await _mathGameScoreboardService.GetUserHighscoreAsync(username);

			Assert.Equal(expectedHighscore.OrderByDescending(s => s.GameData.Scores).First(), result);
		}

		[Fact]
		public async Task GetTopScores_ShouldReturnTopScores() {
			var topCount = 3;
			var userScores = new List<UserScoreDto<MathGameData>> {
				new UserScoreDto<MathGameData> {Username = "user1", GameData = new MathGameData {Scores = 300}},
				new UserScoreDto<MathGameData> {Username = "user2", GameData = new MathGameData {Scores = 250}},
				new UserScoreDto<MathGameData> {Username = "user3", GameData = new MathGameData {Scores = 200}},
				new UserScoreDto<MathGameData> {Username = "user4", GameData = new MathGameData {Scores = 150}}
			};
			_mockScoreRepository.Setup(repo => repo.GetAllScoresAsync<MathGameData>()).ReturnsAsync(userScores);

			var result=await _mathGameScoreboardService.GetTopScoresAsync(topCount);

			Assert.Equal(topCount,result.Count);
			Assert.Equal(userScores[0],result[0]);
			Assert.Equal(userScores[1],result[1]);
			Assert.Equal(userScores[2],result[2]);
		}
	}
}
