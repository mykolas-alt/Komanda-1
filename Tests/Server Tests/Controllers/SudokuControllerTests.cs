using Microsoft.AspNetCore.Mvc.Testing;
using Projektas.Tests.Server_Tests;
using System.Net.Http.Json;

namespace Projektas.Tests.Controllers {
    public class SudokuControllerTests : IClassFixture<CustomWebApplicationFactory<Program>> {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Program> _factory;

        internal SudokuControllerTests(CustomWebApplicationFactory<Program> factory) {
            _factory = factory;
            _client = _factory.CreateClient(new WebApplicationFactoryClientOptions {
                AllowAutoRedirect = false
            });
        }

        [Fact]
        public async Task GenerateSolvedSudoku_ReturnsValidGrid() {
            int gridSize = 9;

            var response = await _client.GetAsync($"/api/sudoku/generate-sudoku?gridSize={gridSize}");

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<List<List<int>>>();

            int[,] grid = ConvertListTo2DArray(result, gridSize);

            Assert.Equal(gridSize, grid.GetLength(0));
            Assert.Equal(gridSize, grid.GetLength(1));

            for(int i = 0; i < gridSize; i++) {
                var row = Enumerable.Range(0, gridSize).Select(j => grid[i, j]).ToList();
                Assert.True(IsValidSet(row, gridSize), $"Row {i} is invalid");
            }

            for(int j = 0; j < gridSize; j++) {
                var column = Enumerable.Range(0, gridSize).Select(i => grid[i, j]).ToList();
                Assert.True(IsValidSet(column, gridSize), $"Column {j} is invalid");
            }

            int subGridSize = (int)Math.Sqrt(gridSize);
            for(int row = 0; row < gridSize; row += subGridSize) {
                for(int col = 0; col < gridSize; col += subGridSize) {
                    var subGrid = new List<int>();
                    for(int i = 0; i < subGridSize; i++) {
                        for(int j = 0; j < subGridSize; j++) {
                            subGrid.Add(grid[row + i, col + j]);
                        }
                    }
                    Assert.True(IsValidSet(subGrid, gridSize), $"Sub-grid starting at ({row},{col}) is invalid");
                }
            }
        }

        private bool IsValidSet(List<int> numbers, int gridSize) {
            var validSet = Enumerable.Range(1, gridSize).ToHashSet();
            return numbers.Count == gridSize && validSet.SetEquals(numbers);
        }

        [Fact]
        public async Task HideNumbers_HidesCorrectNumberOfCells() {
            int gridSize = 9;
            int numbersToRemove = 20;
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

            int[] flatGrid = grid.Cast<int>().ToArray();
            string gridParam = string.Join("&grid=", flatGrid);

            var response = await _client.GetAsync(
                $"/api/sudoku/hide-numbers?gridSize={gridSize}&numbersToRemove={numbersToRemove}&grid={gridParam}"
            );

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<List<List<int>>>();

            int[,] resultGrid = ConvertListTo2DArray(result, gridSize);

            int hiddenNumbers = resultGrid.Cast<int>().Count(value => value == 0);
            Assert.Equal(numbersToRemove, hiddenNumbers);

            foreach(var value in resultGrid) {
                Assert.InRange(value, 0, gridSize);
            }
        }

        private static int[,] ConvertListTo2DArray(List<List<int>> gridList, int gridSize) {
            int[,] grid = new int[gridSize, gridSize];
            for(int i = 0; i < gridSize; i++) {
                for(int j = 0; j < gridSize; j++) {
                    grid[i, j] = gridList[i][j];
                }
            }
            return grid;
        }
    }
}
