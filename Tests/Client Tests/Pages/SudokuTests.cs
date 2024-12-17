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

			_mockAuthStateProvider.Setup(s => s.GetUsernameAsync()).ReturnsAsync("TestUser");
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

            Assert.Equal(0, cut.Instance.ElapsedTime);

            _mockTimerService.Verify(t => t.Start(It.IsAny<int>()), Times.AtLeastOnce);
        }

        [Fact]
        public void SudokuDifficulty_ShouldReturnCorrectRange()
        {
            var cut = RenderComponent<Sudoku>();

            cut.Instance.GridSize = 4;

            cut.Instance.ChangeDifficulty("Easy");
            var difficulty = cut.Instance.SudokuDifficulty();
            Assert.InRange(difficulty, 7, 8);

            cut.Instance.ChangeDifficulty("Normal");
            difficulty = cut.Instance.SudokuDifficulty();
            Assert.InRange(difficulty, 9, 10);

            cut.Instance.ChangeDifficulty("Hard");
            difficulty = cut.Instance.SudokuDifficulty();
            Assert.InRange(difficulty, 11, 12);

            cut.Instance.GridSize = 9;

            cut.Instance.ChangeDifficulty("Easy");
            difficulty = cut.Instance.SudokuDifficulty();
            Assert.InRange(difficulty, 30, 35);

            cut.Instance.ChangeDifficulty("Normal");
            difficulty = cut.Instance.SudokuDifficulty();
            Assert.InRange(difficulty, 45, 48);

            cut.Instance.ChangeDifficulty("Hard");
            difficulty = cut.Instance.SudokuDifficulty();
            Assert.InRange(difficulty, 53, 57);

            cut.Instance.GridSize = 16;

            cut.Instance.ChangeDifficulty("Easy");
            difficulty = cut.Instance.SudokuDifficulty();
            Assert.InRange(difficulty, 30, 50);

            cut.Instance.ChangeDifficulty("Normal");
            difficulty = cut.Instance.SudokuDifficulty();
            Assert.InRange(difficulty, 100, 130);

            cut.Instance.ChangeDifficulty("Hard");
            difficulty = cut.Instance.SudokuDifficulty();
            Assert.InRange(difficulty, 140, 150);
        }

        [Fact]
        public async Task IsCorrect_ShouldEndGame_WhenSolutionIsCorrect() {
            var mockGrid = new int[9, 9];
            _mockSudokuService.Setup(x => x.GenerateSolvedSudokuAsync(It.IsAny<int>())).ReturnsAsync(mockGrid);
            _mockSudokuService.Setup(x => x.HideNumbersAsync(It.IsAny<int[,]>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(mockGrid); var cut = RenderComponent<Sudoku>();
            
            await cut.Instance.StartGameAsync();
            cut.Instance.GridValues = mockGrid;
            cut.Instance.Solution = mockGrid;

            cut.Instance.IsCorrect();

            Assert.Equal("ended", cut.Instance.gameScreen);
        }

        [Fact]
        public async Task HandleCellClicked_ShouldSetSelectedRowAndCol()
        {
            var mockGrid = new int[9, 9];
            _mockSudokuService.Setup(x => x.GenerateSolvedSudokuAsync(It.IsAny<int>())).ReturnsAsync(mockGrid);
            _mockSudokuService.Setup(x => x.HideNumbersAsync(It.IsAny<int[,]>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(mockGrid);

            var cut = RenderComponent<Sudoku>();

            await cut.Instance.StartGameAsync();
            cut.Instance.GridValues = mockGrid;
            cut.Instance.HandleCellClicked(2, 3);

            Assert.Equal(2, cut.Instance.SelectedRow);
            Assert.Equal(3, cut.Instance.SelectedCol);
        }

        [Fact]
        public async Task HandleValueSelected_ShouldUpdateGridValues()
        {
            var mockGrid = new int[9, 9];
            _mockSudokuService.Setup(x => x.GenerateSolvedSudokuAsync(It.IsAny<int>())).ReturnsAsync(mockGrid);
            _mockSudokuService.Setup(x => x.HideNumbersAsync(It.IsAny<int[,]>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(mockGrid);
            var cut = RenderComponent<Sudoku>();
            await cut.Instance.StartGameAsync();
            cut.Instance.GridValues = mockGrid;

            cut.Instance.HandleValueSelected(new ChangeEventArgs { Value = "5" }, 1, 1);

            Assert.Equal(5, cut.Instance.GridValues[1, 1]);
        }
    }
}
