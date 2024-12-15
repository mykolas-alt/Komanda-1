using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Projektas.Client.Interfaces;
using Projektas.Client.Pages;
using Projektas.Shared.Enums;
using Projektas.Shared.Models;

namespace Projektas.Tests.Client_Tests.Pages {
    public class SudokuTests : TestContext {
        private readonly Mock<ITimerService> _mockTimerService;
        private readonly Mock<ISudokuService> _mockSudokuService;
		private readonly Mock<IAccountAuthStateProvider> _mockAuthStateProvider;

        public SudokuTests() {
            _mockSudokuService = new Mock<ISudokuService>();
            _mockTimerService = new Mock<ITimerService>();
			_mockAuthStateProvider = new Mock<IAccountAuthStateProvider>();
            _mockSudokuService
                .Setup(service => service.GenerateSolvedSudokuAsync(It.IsAny<int>()))
                .ReturnsAsync(new int[9, 9]);

            Services.AddSingleton(_mockSudokuService.Object);
            Services.AddSingleton(_mockTimerService.Object);
			Services.AddSingleton(_mockAuthStateProvider.Object);

			_mockSudokuService.Setup(s => s.SaveScoreAsync(It.IsAny<string>(),It.IsAny<int>(), It.IsAny<GameDifficulty>(), It.IsAny<GameMode>())).Returns(Task.CompletedTask);
			_mockSudokuService.Setup(s => s.GetUserHighscoreAsync(It.IsAny<string>())).ReturnsAsync(new UserScoreDto<SudokuData> {
                Username = "User",
                GameData = new SudokuData {
                    TimeInSeconds = 30
                }
            });
			_mockSudokuService.Setup(s => s.GetTopScoresAsync(It.IsAny<int>())).ReturnsAsync(new List<UserScoreDto<SudokuData>> {
				new UserScoreDto<SudokuData> {
                    Username = "User1",
                    GameData = new SudokuData {
                        TimeInSeconds = 30
                    }
                },
				new UserScoreDto<SudokuData> {
                    Username = "User2",
                    GameData = new SudokuData {
                        TimeInSeconds = 50
                    }
                },
				new UserScoreDto<SudokuData> {
                    Username = "User3",
                    GameData = new SudokuData {
                        TimeInSeconds = 80
                    }
                }
			});

			_mockAuthStateProvider.Setup(s => s.GetUsernameAsync()).ReturnsAsync("TestUser");
        }

        [Fact]
        public void OnInitialized_ShouldSetInitialState() {
            var cut = RenderComponent<Sudoku>();

            Assert.False(cut.Instance.IsGameActive);
            Assert.NotNull(cut.Instance.PossibleValues);
            Assert.Equal(9, cut.Instance.GridSize);
            _mockTimerService.VerifyAdd(t => t.OnTick += It.IsAny<Action>(), Times.Once);
        }

        [Fact]
        public async Task GenerateSudokuGame_ShouldInitializeGridAndStartTimer() {
            var mockGrid = new int[9, 9];
            _mockSudokuService
                .Setup(s => s.GenerateSolvedSudokuAsync(It.IsAny<int>()))
                .ReturnsAsync(mockGrid);
            _mockSudokuService
                .Setup(s => s.HideNumbersAsync(It.IsAny<int[,]>(), It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(mockGrid);

            var cut = RenderComponent<Sudoku>();

            await cut.InvokeAsync(() => cut.Instance.GenerateSudokuGameAsync());

            Assert.True(cut.Instance.IsGameActive);
            Assert.Equal(0, cut.Instance.ElapsedTime);

            _mockTimerService.Verify(t => t.Start(It.IsAny<int>()), Times.AtLeastOnce);
        }

        [Fact]
        public void SudokuDifficulty_ShouldReturnCorrectRange() {
            var cut = RenderComponent<Sudoku>();

            cut.Instance.GridSize = 4;

            cut.Instance.OnDifficultyChanged(new ChangeEventArgs {Value = "Easy"});
            var difficulty = cut.Instance.SudokuDifficulty();
            Assert.InRange(difficulty, 7, 8);

            cut.Instance.OnDifficultyChanged(new ChangeEventArgs { Value = "Normal" });
            difficulty = cut.Instance.SudokuDifficulty();
            Assert.InRange(difficulty, 9, 10);

            cut.Instance.OnDifficultyChanged(new ChangeEventArgs { Value = "Hard" });
            difficulty = cut.Instance.SudokuDifficulty();
            Assert.InRange(difficulty, 11, 12);

            cut.Instance.GridSize = 9;

            cut.Instance.OnDifficultyChanged(new ChangeEventArgs { Value = "Easy" });
            difficulty = cut.Instance.SudokuDifficulty();
            Assert.InRange(difficulty, 30, 35);

            cut.Instance.OnDifficultyChanged(new ChangeEventArgs {Value = "Normal"});
            difficulty = cut.Instance.SudokuDifficulty();
            Assert.InRange(difficulty, 45, 48);

            cut.Instance.OnDifficultyChanged(new ChangeEventArgs {Value = "Hard"});
            difficulty = cut.Instance.SudokuDifficulty();
            Assert.InRange(difficulty, 53, 57);

            cut.Instance.GridSize = 16;

            cut.Instance.OnDifficultyChanged(new ChangeEventArgs { Value = "Easy" });
            difficulty = cut.Instance.SudokuDifficulty();
            Assert.InRange(difficulty, 30, 50);

            cut.Instance.OnDifficultyChanged(new ChangeEventArgs { Value = "Normal" });
            difficulty = cut.Instance.SudokuDifficulty();
            Assert.InRange(difficulty, 100, 130);

            cut.Instance.OnDifficultyChanged(new ChangeEventArgs { Value = "Hard" });
            difficulty = cut.Instance.SudokuDifficulty();
            Assert.InRange(difficulty, 140, 150);
        }


        [Fact]
        public void TimerTick_ShouldEndGame_WhenTimeRunsOut() {
            _mockTimerService.Setup(t => t.RemainingTime).Returns(0);
            var cut = RenderComponent<Sudoku>();
            cut.Instance.IsGameActive = true;

            cut.Instance.TimerTick();

            Assert.False(cut.Instance.IsGameActive);
            Assert.Equal("Ran out of time", cut.Instance.Message);

            _mockTimerService.Verify(t => t.Stop(), Times.Exactly(2));
        }

        [Fact]
        public void IsCorrect_ShouldEndGame_WhenSolutionIsCorrect() {
            var mockGrid = new int[9, 9];
            var cut = RenderComponent<Sudoku>();
            cut.Instance.GridValues = mockGrid;
            cut.Instance.Solution = mockGrid;
            cut.Instance.IsGameActive = true;
            cut.Instance.IsLoading = false;

            cut.Instance.IsCorrect();

            Assert.False(cut.Instance.IsGameActive);
            Assert.Equal($"Correct solution. Solved in {Sudoku.FormatTime(0)}", cut.Instance.Message);
        }

        [Fact]
        public void HandleCellClicked_ShouldSetSelectedRowAndCol() {
            var cut = RenderComponent<Sudoku>();
            cut.Instance.HandleCellClicked(2, 3);

            Assert.Equal(2, cut.Instance.SelectedRow);
            Assert.Equal(3, cut.Instance.SelectedCol);
        }

        [Fact]
        public void HandleValueSelected_ShouldUpdateGridValues() {
            var cut = RenderComponent<Sudoku>();
            cut.Instance.GridValues = new int[9, 9];

            cut.Instance.HandleValueSelected(new ChangeEventArgs {Value = "5"}, 1, 1);

            Assert.Equal(5, cut.Instance.GridValues[1, 1]);
        }

        [Fact]
        public void OnSizeChanged_ShouldUpdateNextGridSize()
        {
            var cut = RenderComponent<Sudoku>();

            var changeArgs = new ChangeEventArgs { Value = "9" };
            cut.Instance.OnSizeChanged(changeArgs);
            Assert.Equal(9, (int)cut.Instance.mode);

            changeArgs = new ChangeEventArgs { Value = "16" };
            cut.Instance.OnSizeChanged(changeArgs);
            Assert.Equal(16, (int)cut.Instance.mode);


            changeArgs = new ChangeEventArgs { Value = "invalid" };
            cut.Instance.OnSizeChanged(changeArgs);
            Assert.Equal(16, (int)cut.Instance.mode);

        }



    }
}
