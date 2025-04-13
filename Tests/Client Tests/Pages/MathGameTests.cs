using Bunit;
using Moq;
using Projektas.Client.Pages;
using Projektas.Shared.Enums;
using Projektas.Client.Interfaces;
using Projektas.Shared.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Projektas.Tests.Client_Tests.Pages {
	public class MathGameTests : TestContext {
		private readonly Mock<IMathGameService> _mockMathGameService;
		private readonly Mock<ITimerService> _mockTimerService;
		private readonly Mock<IAccountAuthStateProvider> _mockAuthStateProvider;

		public MathGameTests() {
			_mockMathGameService = new Mock<IMathGameService>();
			_mockTimerService = new Mock<ITimerService>();
			_mockAuthStateProvider = new Mock<IAccountAuthStateProvider>();

			Services.AddSingleton(_mockMathGameService.Object);
			Services.AddSingleton(_mockTimerService.Object);
			Services.AddSingleton(_mockAuthStateProvider.Object);

			// setup
			_mockMathGameService.Setup(s => s.GetQuestionAsync(It.IsAny<int>(), It.IsAny<GameDifficulty>())).ReturnsAsync("Question");
			_mockMathGameService.Setup(s => s.GetOptionsAsync()).ReturnsAsync(new List<int> {1, 2, 3, 4});
			_mockMathGameService.Setup(s => s.CheckAnswerAsync(It.IsAny<int>())).ReturnsAsync(true);
			_mockMathGameService.Setup(s => s.SaveScoreAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<GameDifficulty>())).Returns(Task.CompletedTask);
			_mockMathGameService.Setup(s => s.GetUserHighscoreAsync(It.IsAny<string>(), It.IsAny<GameDifficulty>())).ReturnsAsync(new UserScoreDto<MathGameData> {
				Username = "User",
				GameData = new MathGameData {
					Scores = 10
				}
			});
			_mockMathGameService.Setup(s => s.GetTopScoresAsync(It.IsAny<GameDifficulty>(), It.IsAny<int>())).ReturnsAsync(new List<UserScoreDto<MathGameData>> {
				new UserScoreDto<MathGameData> {
					Username = "User1",
					GameData = new MathGameData {
						Scores = 100
					}
				},
				new UserScoreDto<MathGameData> {
					Username = "User2",
					GameData = new MathGameData {
						Scores = 90
					}
				},
				new UserScoreDto<MathGameData> {
					Username = "User3",
					GameData = new MathGameData {
						Scores = 80
					}
				}
			});

			_mockAuthStateProvider.Setup(s => s.GetUsernameAsync()).ReturnsAsync("TestUser");
		}

		[Fact]
		public async Task StartGame_ShouldInitializeGameState() {
			var cut = RenderComponent<MathGame>();

			await cut.Instance.StartGameAsync();

			Assert.False(cut.Instance.isTimesUp);
			Assert.Equal(0, cut.Instance.score);
			Assert.NotNull(cut.Instance.question);
			Assert.NotNull(cut.Instance.options);
		}

		[Fact]
		public async Task CheckAnswer_CorrectAnswer_IncrementsScore() {
			var cut = RenderComponent<MathGame>();

			await cut.Instance.GenerateQuestionAsync();
			await cut.Instance.CheckAnswer(1);

			Assert.Equal(1, cut.Instance.score);
			Assert.NotNull(cut.Instance.question);
			Assert.NotNull(cut.Instance.options);
		}

		[Fact]
		public async Task CheckAnswer_WrongAnswer_DecrementsTime() {
			_mockMathGameService.Setup(s => s.CheckAnswerAsync(It.IsAny<int>())).ReturnsAsync(false);
			_mockTimerService.SetupProperty(t => t.RemainingTime, 10);

			var cut = RenderComponent<MathGame>();

			await cut.Instance.GenerateQuestionAsync();
			await cut.Instance.CheckAnswer(1);

			Assert.Equal(5, _mockTimerService.Object.RemainingTime);
		}

		[Fact]
		public async Task CheckAnswer_WrongAnswer_FinishesGameWhenTimeIsLowerThan5Secs() {
			_mockMathGameService.Setup(s => s.CheckAnswerAsync(It.IsAny<int>())).ReturnsAsync(false);
			_mockTimerService.SetupProperty(t => t.RemainingTime, 4);

			var cut = RenderComponent<MathGame>();

			await cut.Instance.GenerateQuestionAsync();
			await cut.Instance.CheckAnswer(1);

			Assert.Equal(0, _mockTimerService.Object.RemainingTime);
			Assert.True(cut.Instance.isTimesUp);
			_mockTimerService.Verify(t => t.Stop(), Times.Once);
			_mockMathGameService.Verify(s => s.SaveScoreAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<GameDifficulty>()), Times.Once);
			Assert.NotNull(cut.Instance.topScores);
		}

		[Fact]
		public async Task OnTimerTick_TimeUp_StopsTimerAndSavesData() {
			_mockTimerService.SetupProperty(t => t.RemainingTime, 0);

			var cut = RenderComponent<MathGame>();

			await cut.InvokeAsync(() => cut.Instance.OnTimerTick());

			Assert.True(cut.Instance.isTimesUp);
			_mockTimerService.Verify(t => t.Stop(), Times.Once);
			_mockMathGameService.Verify(s => s.SaveScoreAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<GameDifficulty>()), Times.Once);
			Assert.NotNull(cut.Instance.topScores);
		}
	}
}