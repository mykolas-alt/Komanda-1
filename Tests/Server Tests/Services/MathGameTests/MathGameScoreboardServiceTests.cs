using Moq;
using Projektas.Server.Services;
using Projektas.Server.Services.MathGame;
using Projektas.Shared.Models;

namespace Projektas.Tests.Services.MathGameTests {
    public class MathGameScoreboardServiceTests {
		private readonly Mock<IScoreRepository<MathGameM>> _mockScoreRepository;
		private readonly MathGameScoreboardService _mathGameScoreboardService;

        public MathGameScoreboardServiceTests() {
			_mockScoreRepository=new Mock<IScoreRepository<MathGameM>>();
			_mathGameScoreboardService=new MathGameScoreboardService(_mockScoreRepository.Object);
        }

		[Fact]
		public async Task AddScoreToDb_ShouldCallAddMathGameScoreToUserAsync() {
			var userScore=new UserScoreDto {Username="testuser",Score=100};

			await _mathGameScoreboardService.AddScoreToDb(userScore);

			_mockScoreRepository.Verify(repo => repo.AddScoreToUserAsync(userScore.Username,userScore.Score),Times.Once);
		}

		[Fact]
		public async Task GetUserHighscore_ShouldReturnHighscore() {
			var username="testuser";
			var expectedHighscore=200;
			_mockScoreRepository.Setup(repo => repo.GetHighscoreFromUserAsync(username)).ReturnsAsync(expectedHighscore);

			var result=await _mathGameScoreboardService.GetUserHighscore(username);

			Assert.Equal(expectedHighscore,result);
		}

		[Fact]
		public async Task GetTopScores_ShouldReturnTopScores() {
			var topCount=3;
			var userScores=new List<UserScoreDto> {
				new UserScoreDto {Username="user1",Score=300},
				new UserScoreDto {Username="user2",Score=250},
				new UserScoreDto {Username="user3",Score=200},
				new UserScoreDto {Username="user4",Score=150}
			};
			_mockScoreRepository.Setup(repo => repo.GetAllScoresAsync()).ReturnsAsync(userScores);

			var result=await _mathGameScoreboardService.GetTopScores(topCount);

			Assert.Equal(topCount,result.Count);
			Assert.Equal(userScores[0],result[0]);
			Assert.Equal(userScores[1],result[1]);
			Assert.Equal(userScores[2],result[2]);
		}
	}
}
