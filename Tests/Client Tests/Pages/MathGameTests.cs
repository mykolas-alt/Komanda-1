using Bunit;
using Moq;
using Projektas.Client.Pages;
using Projektas.Client.Interfaces;
using Projektas.Shared.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Projektas.Tests.Client_Tests.Pages 
{
    public class MathGameTests : TestContext
    {
        private readonly Mock<IMathGameStateService> _mockMathGameStateService;
        private readonly Mock<IMathGameService> _mockMathGameService;
        private readonly Mock<ITimerService> _mockTimerService;

        public MathGameTests()
        {
            _mockMathGameStateService = new Mock<IMathGameStateService>();
            _mockMathGameService = new Mock<IMathGameService>();
            _mockTimerService = new Mock<ITimerService>();

            Services.AddSingleton(_mockMathGameStateService.Object);
            Services.AddSingleton(_mockMathGameService.Object);
            Services.AddSingleton(_mockTimerService.Object);

            // setup
            _mockMathGameStateService.Setup(s => s.GetGameState()).ReturnsAsync(new GameState());
            _mockMathGameService.Setup(s => s.CheckAnswerAsync(It.IsAny<int>())).ReturnsAsync(true);
            _mockMathGameStateService.Setup(s => s.IncrementScore(It.IsAny<GameState>())).Returns(Task.CompletedTask);
            _mockMathGameService.Setup(s => s.GetQuestionAsync(It.IsAny<int>())).ReturnsAsync("Question");
            _mockMathGameService.Setup(s => s.GetOptionsAsync()).ReturnsAsync(new List<int> { 1, 2, 3, 4 });
            _mockMathGameService.Setup(s => s.SaveDataAsync(It.IsAny<int>())).Returns(Task.CompletedTask);
            _mockMathGameService.Setup(s => s.GetTopScoresAsync(It.IsAny<int>())).ReturnsAsync(new List<int> { 100, 90, 80 });
        }

        [Fact]
        public void MathGame_RendersCorrectlyOnInitialized()
        {
            var cut = RenderComponent<MathGame>();

            cut.MarkupMatches(@"
                <div class=""container"">
                    <div class=""math-game"">
                        <div class=""app-title"">
                            Math Game!
                        </div>
                        <div class=""math-game-header"">
                            <p>Test your math skills!</p>
                        </div>
                        <div class=""math-game-footer"">
                            <button class=""play-btn"">Start game</button>
                        </div>
                    </div>
                </div>
            ");
        }

        [Fact]
        public async Task MathGame_RendersCorrectlyWhenQuestionDisplayed()
        {
            _mockTimerService.SetupProperty(t => t.RemainingTime, 10);
            var cut = RenderComponent<MathGame>();

            await cut.Instance.StartGame();
            await cut.InvokeAsync(() => cut.Instance.OnTimerTick());

            cut.MarkupMatches(@"
            <div class=""container"" >
              <div class=""math-game"" >
                <div class=""app-title"" >
                  Math Game!
                </div>
                <div class=""math-game-header"" >
                  <p >What is Question = ?</p>
                </div>
                <div class=""math-game-body"" >
                  <div class=""option""  >1</div>
                  <div class=""option""  >2</div>
                  <div class=""option""  >3</div>
                  <div class=""option""  >4</div>
                  <div class=""score-time-container"" >
                    <div class=""score"" >
                      <p >Score: 0</p>
                    </div>
                    <div class=""time"" >
                      <p >Time left: 10 seconds</p>
                    </div>
                  </div>
                </div>
              </div>
            </div>
        ");
        }

        [Fact]
        public async Task MathGame_RendersCorrectlyWhenGameOver()
        {
            var cut = RenderComponent<MathGame>();

            await cut.Instance.StartGame();
            _mockTimerService.SetupProperty(t => t.RemainingTime, 0);
            await cut.InvokeAsync(() => cut.Instance.OnTimerTick());

            cut.MarkupMatches(@"
                <div class=""container"">
                    <div class=""math-game"">
                        <div class=""app-title"">
                            Game over!
                        </div>
                        <div class=""score-highscore-after-game-container"">
                            <div class=""score-after-game"">
                                <b>You scored 0 points!</b>
                            </div>
                            <div class=""highscore-after-game"">
                                <b>Your highscore: 0</b>
                            </div>
                        </div>
                        <div class=""math-game-footer"">
                            <button class=""play-btn"">Play again</button>
                        </div>
                        <div class=""scoreboard"">
                            <div class=""scoreboard-title"">
                                <b>Scoreboard</b>
                            </div>
                            <p>1. 100</p>
                            <p>2. 90</p>
                            <p>3. 80</p>
                        </div>
                    </div>
                </div>
            ");
        }

        [Fact]
        public async Task StartGame_ShouldInitializeGameState()
        {
            var cut = RenderComponent<MathGame>();

            await cut.Instance.StartGame();

            Assert.False(cut.Instance.isTimesUp);
            Assert.Equal(0, cut.Instance.gameState.Score);
            Assert.NotNull(cut.Instance.question);
            Assert.NotNull(cut.Instance.options);
        }

        [Fact]
        public async Task CheckAnswer_CorrectAnswer_IncrementsScore()
        {
            var cut = RenderComponent<MathGame>();

            await cut.Instance.GenerateQuestion();
            await cut.Instance.CheckAnswer(1);

            _mockMathGameStateService.Verify(s => s.IncrementScore(It.IsAny<GameState>()), Times.Once);
            Assert.NotNull(cut.Instance.question);
            Assert.NotNull(cut.Instance.options);
        }

        [Fact]
        public async Task CheckAnswer_WrongAnswer_DecrementsTime()
        {
            _mockMathGameService.Setup(s => s.CheckAnswerAsync(It.IsAny<int>())).ReturnsAsync(false);
            _mockTimerService.SetupProperty(t => t.RemainingTime, 10);

            var cut = RenderComponent<MathGame>();

            await cut.Instance.GenerateQuestion();
            await cut.Instance.CheckAnswer(1);

            Assert.Equal(5, _mockTimerService.Object.RemainingTime);
        }

        [Fact]
        public async Task CheckAnswer_WrongAnswer_FinishesGameWhenTimeIsLowerThan5Secs()
        {
            _mockMathGameService.Setup(s => s.CheckAnswerAsync(It.IsAny<int>())).ReturnsAsync(false);
            _mockTimerService.SetupProperty(t => t.RemainingTime, 4);

            var cut = RenderComponent<MathGame>();

            await cut.Instance.GenerateQuestion();
            await cut.Instance.CheckAnswer(1);

            Assert.Equal(0, _mockTimerService.Object.RemainingTime);
            Assert.True(cut.Instance.isTimesUp);
            _mockTimerService.Verify(t => t.Stop(), Times.Once);
            _mockMathGameService.Verify(s => s.SaveDataAsync(It.IsAny<int>()), Times.Once);
            Assert.NotNull(cut.Instance.topScores);
        }

        [Fact]
        public async Task OnTimerTick_TimeUp_StopsTimerAndSavesData()
        {
            _mockTimerService.SetupProperty(t => t.RemainingTime, 0);

            var cut = RenderComponent<MathGame>();

            await cut.InvokeAsync(() => cut.Instance.OnTimerTick());

            Assert.True(cut.Instance.isTimesUp);
            _mockTimerService.Verify(t => t.Stop(), Times.Once);
            _mockMathGameService.Verify(s => s.SaveDataAsync(It.IsAny<int>()), Times.Once);
            Assert.NotNull(cut.Instance.topScores);
        }
    }
}

