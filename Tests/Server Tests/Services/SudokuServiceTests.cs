using Moq;
using Projektas.Server.Services;
using Projektas.Shared.Models;

namespace Projektas.Tests.Server_Tests.Services
{
    public class SudokuServiceTests
    {
        private readonly Mock<IScoreRepository<SudokuM>> _mockScoreRepository;
        private readonly SudokuService _sudokuService;

        public SudokuServiceTests()
        {
            
            _mockScoreRepository=new Mock<IScoreRepository<SudokuM>>();
            _sudokuService = new SudokuService(_mockScoreRepository.Object);
        }

        [Fact]
        public void HasMultipleSolutions_ValidGrid_ReturnsTrueForMultipleSolutions()
        {
            int[,] grid = new int[9, 9]
            {
                {5, 3, 4, 6, 7, 8, 9, 1, 2},
                {6, 7, 2, 1, 9, 5, 3, 4, 8},
                {1, 9, 8, 3, 4, 2, 5, 6, 7},
                {8, 5, 9, 7, 6, 1, 4, 2, 3},
                {4, 2, 6, 8, 5, 3, 7, 9, 1},
                {7, 1, 3, 9, 2, 4, 8, 5, 6},
                {9, 6, 1, 5, 3, 7, 2, 8, 4},
                {2, 8, 7, 4, 1, 9, 6, 3, 5},
                {3, 4, 5, 2, 8, 6, 1, 7, 9}
            };
            int gridSize = 9;

            var result = _sudokuService.HasMultipleSolutions(grid, gridSize);

            Assert.False(result);
        }

        [Fact]
        public void HideNumbers_ValidGrid_ReturnsGridWithHiddenNumbers()
        {
            int gridSize = 9;
            int[,] grid = new int[9, 9]
            {
                {5, 3, 4, 6, 7, 8, 9, 1, 2},
                {6, 7, 2, 1, 9, 5, 3, 4, 8},
                {1, 9, 8, 3, 4, 2, 5, 6, 7},
                {8, 5, 9, 7, 6, 1, 4, 2, 3},
                {4, 2, 6, 8, 5, 3, 7, 9, 1},
                {7, 1, 3, 9, 2, 4, 8, 5, 6},
                {9, 6, 1, 5, 3, 7, 2, 8, 4},
                {2, 8, 7, 4, 1, 9, 6, 3, 5},
                {3, 4, 5, 2, 8, 6, 1, 7, 9}
            };

            int numbersToRemove = 5;

            var result = _sudokuService.HideNumbers(grid, gridSize, numbersToRemove);

            int hiddenNumbers = result.Cast<int>().Count(value => value == 0);
            Assert.Equal(numbersToRemove, hiddenNumbers);

            for (int row = 0; row < gridSize; row++)
            {
                for (int col = 0; col < gridSize; col++)
                {
                    int val = result[row, col];
                    Assert.InRange(val, 0, gridSize);
                }
            }
        }

        [Fact]
        public void GenerateSolvedSudoku_ValidGridSize_ReturnsSolvedGrid()
        {
            int gridSize = 9;

            var result = _sudokuService.GenerateSolvedSudoku(gridSize);

            Assert.Equal(gridSize, result.GetLength(0));
            Assert.Equal(gridSize, result.GetLength(1));

            Assert.All(result.Cast<int>(), val => Assert.InRange(val, 1, gridSize));
        }

        [Fact]
        public void SolveSudoku_ValidGrid_ReturnsSolvedGrid()
        {
            int[,] grid = new int[9, 9]
            {
                {5, 3, 4, 6, 7, 8, 9, 1, 2},
                {6, 7, 2, 1, 9, 5, 3, 4, 8},
                {1, 9, 8, 3, 4, 2, 5, 6, 7},
                {8, 5, 9, 7, 6, 1, 4, 2, 3},
                {4, 2, 6, 8, 5, 3, 7, 9, 1},
                {7, 1, 3, 9, 2, 4, 8, 5, 6},
                {9, 6, 1, 5, 3, 7, 2, 8, 4},
                {2, 8, 7, 4, 1, 9, 6, 3, 5},
                {3, 4, 5, 2, 8, 6, 1, 7, 9}
            };

            int gridSize = 9;

            var result = _sudokuService.GenerateSolvedSudoku(gridSize);

            Assert.True(result.Cast<int>().All(value => value != 0));
        }
    }
}
