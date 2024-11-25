using Projektas.Client.Interfaces;
using System.Net.Http.Json;

namespace Projektas.Client.Services
{
    public class SudokuService : ISudokuService
    {
        private readonly HttpClient _httpClient;

        public SudokuService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        public async Task<int[,]> HideNumbersAsync(int[,] grid, int gridSize, int numbersToRemove)
        {
            var queryString = string.Join("&",
                Enumerable.Range(0, grid.GetLength(0))
                          .SelectMany(i => Enumerable.Range(0, grid.GetLength(1))
                                                     .Select(j => $"grid={grid[i, j]}"))
            );

            var response = await _httpClient.GetAsync($"api/sudoku/hide-numbers?{queryString}&gridSize={gridSize}&numbersToRemove={numbersToRemove}");
            var updatedGridList = await response.Content.ReadFromJsonAsync<List<List<int>>>();

            int[,] updatedGrid = new int[gridSize, gridSize];

            for (int i = 0; i < gridSize; i++)
            {
                for (int j = 0; j < gridSize; j++)
                {
                    updatedGrid[i, j] = updatedGridList[i][j];
                }
            }

            return updatedGrid;
        }


        public async Task<int[,]> GenerateSolvedSudokuAsync(int gridSize)
        {
            var response = await _httpClient.GetFromJsonAsync<List<List<int>>>($"api/sudoku/generate-sudoku?gridsize={gridSize}");

            int[,] grid = new int[gridSize, gridSize];

            for (int i = 0; i < gridSize; i++)
            {
                for (int j = 0; j < gridSize; j++)
                {
                    grid[i, j] = response![i][j];
                }
            }

            return grid;
        }

    }
}
