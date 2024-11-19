using System.Net.Http.Json;

namespace Projektas.Client.Services
{
    public class SudokuService
    {
        private readonly HttpClient _httpClient;
        private static Random _random = new Random();

        public SudokuService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<int[,]> HideNumbers(int[,] grid, int gridSize, int numbersToRemove)
        {
            int attempts = 0;
            Dictionary<string, int> hidenNumbers = new Dictionary<string, int>();
            int[,] tempGrid = (int[,])grid.Clone();

            for (int i = 1; i <= numbersToRemove; i++)
            {
                int row = _random.Next(0, gridSize);
                int col = _random.Next(0, gridSize);

                string hideNumbKey = row.ToString() + "," + col.ToString();
                int hideNumbTemp;

                while (hidenNumbers.ContainsKey(hideNumbKey))
                {
                    row = _random.Next(0, gridSize);
                    col = _random.Next(0, gridSize);

                    hideNumbKey = row.ToString() + "," + col.ToString();
                }

                hideNumbTemp = tempGrid![row, col];
                tempGrid[row, col] = 0;

                if (!await HasMultipleSolutions(tempGrid))
                {
                    tempGrid[row, col] = hideNumbTemp;
                    i--;
                    attempts++;

                    if (attempts > 1)
                    {
                        string lastCords = hidenNumbers.Keys.Last();
                        string[] cords = lastCords.Split(',');
                        row = int.Parse(cords[0]);
                        col = int.Parse(cords[1]);

                        grid[row, col] = hidenNumbers[lastCords];
                        hidenNumbers.Remove(lastCords);

                        tempGrid = (int[,])grid.Clone();

                        attempts = 0;
                        i--;
                    }
                }
                else
                {
                    grid[row, col] = 0;
                    hidenNumbers.Add(hideNumbKey, hideNumbTemp);
                    attempts--;
                }
            }

            return grid;
        }

        public async Task<bool> HasMultipleSolutions(int[,] grid)
        {
            var queryString = ConvertGridToQueryString(grid);

            var response = await _httpClient.GetAsync($"api/sudoku/has-multiple-solutions?{queryString}");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<bool>();
            }

            return false;
        }

        private string ConvertGridToQueryString(int[,] grid)
        {
            var queryString = new List<string>();

            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    queryString.Add($"grid={grid[i, j]}");
                }
            }

            return string.Join("&", queryString);
        }

       
        public static int[,] GenerateSolvedSudoku(int gridSize)
        {
            int[,] grid = new int[gridSize, gridSize];
            List<int> row = new List<int>();

            row.AddRange(Enumerable.Range(1, gridSize).OrderBy(n => _random.Next()));

            for (int j = 0; j < gridSize; j++)
            {

                grid[0, j] = row[j];

            }

            SolveSudoku(ref grid, gridSize);

            return grid;
        }


        public static bool SolveSudoku(ref int [,] grid, int gridSize)
        {
            int row = -1;
            int col = -1;

            for (int i = 0; i < gridSize; i++)
            {
                for (int j = 0; j < gridSize; j++)
                {
                    if (grid[i, j] == 0)
                    {
                        row = i;
                        col = j;
                        break;
                    }
                }
                if (row != -1)
                {
                    break;
                }
            }

            if (row == -1)
            {
                return true;
            }

            return PossibleNumbers(row, col, ref grid, gridSize);
        }

        private static bool PossibleNumbers(int row, int col,ref int[,] grid, int gridSize)
        {
            for (int num = 1; num <= gridSize; num++)
            {
                if (IsValidMove(row, col, num, ref grid, gridSize))
                {
                    grid[row, col] = num;

                    if (SolveSudoku(ref grid, gridSize))
                    {
                        return true;
                    }
                    grid[row, col] = 0;
                }
            }

            return false;
        }

        private static bool IsValidMove(int row, int col, int num, ref int[,] grid, int gridSize)
        {
            for (int i = 0; i < gridSize; i++)
            {
                if (grid[row, i] == num || grid[i, col] == num)
                {
                    return false;
                }
            }

            int startRow = row - row % 3;
            int startCol = col - col % 3;

            for (int i = startRow; i < startRow + 3; i++)
            {
                for (int j = startCol; j < startCol + 3; j++)
                {
                    if (grid[i, j] == num)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
