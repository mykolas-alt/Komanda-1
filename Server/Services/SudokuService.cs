using Google.OrTools.ConstraintSolver;
using Projektas.Server.Interfaces;
using Projektas.Shared.Models;

namespace Projektas.Server.Services {
    public class SudokuService {
        private readonly IScoreRepository _scoreRepository;
        
        private Solver? _solver = null;
        private IntVar[,]? _possibleGrid = null;
        private DecisionBuilder? _decisionBuilder = null;
        private SolutionCollector? _solutionCollector = null;
      
        private static Random _random = new Random();

        public SudokuService (IScoreRepository scoreRepository) {
            _scoreRepository = scoreRepository;
        }

        public bool HasMultipleSolutions(int[,] grid, int gridSize) {

            SetUpConstrains(grid, gridSize, (int)Math.Sqrt(gridSize));

            _solutionCollector = _solver!.MakeAllSolutionCollector();
            _solutionCollector.Add(_possibleGrid.Flatten());

            _solver.NewSearch(_decisionBuilder, _solutionCollector);

            while(_solver.NextSolution()) {
                if(_solutionCollector.SolutionCount() > 1) {
                    DisposeResources();
                    return true;
                }
            }

            DisposeResources();
            return false;
        }

        private void SetUpConstrains(int[,] grid, int gridSize, int internalGridSize) {
            _solver = new Solver("SudokuSolver");
            _possibleGrid = _solver.MakeIntVarMatrix(gridSize, gridSize, 1, gridSize, "grid");

            for(int i = 0; i < gridSize; i++) {
                _solver!.Add((from j in Enumerable.Range(0, gridSize) select _possibleGrid![i, j]).ToArray().AllDifferent());
                _solver!.Add((from j in Enumerable.Range(0, gridSize) select _possibleGrid![j, i]).ToArray().AllDifferent());
            }

            for(int i = 0; i < internalGridSize; i++) {
                for(int j = 0; j < internalGridSize; j++) {
                    _solver!.Add((from x in Enumerable.Range(i * internalGridSize, internalGridSize) from y in Enumerable.Range(j * internalGridSize, internalGridSize) select _possibleGrid![x, y]).ToArray().AllDifferent());
                }
            }

            for(int i = 0; i < gridSize; i++) {
                for(int j = 0; j < gridSize; j++) {
                    if (grid[i, j] != 0) {
                        _solver!.Add(_possibleGrid![i, j]==grid[i, j]);
                    }
                }
            }

            _decisionBuilder = _solver!.MakePhase(_possibleGrid.Flatten(), Solver.INT_VAR_SIMPLE, Solver.ASSIGN_MIN_VALUE);
        }

        private void DisposeResources() {
            _decisionBuilder!.Dispose();
            _solver!.Dispose();
            if(_solutionCollector != null)
                _solutionCollector!.Dispose();
            _possibleGrid = null;
        }

        public async Task AddScoreToDbAsync(UserScoreDto<SudokuData> data) {
            await _scoreRepository.AddScoreToUserAsync<SudokuData>(data.Username, data.GameData, data.Timestamp);
        }

        public async Task<UserScoreDto<SudokuData>> GetUserHighscoreAsync(string username) {
            var scores = await _scoreRepository.GetHighscoreFromUserAsync<SudokuData>(username);

            return scores.OrderBy(s => s.GameData.TimeInSeconds).First();
        }

        public async Task<List<UserScoreDto<SudokuData>>> GetTopScoresAsync(int topCount) {
            List<UserScoreDto<SudokuData>> userScores = await _scoreRepository.GetAllScoresAsync<SudokuData>();
            List<UserScoreDto<SudokuData>> orderedScores = userScores.Where(s => !s.IsPrivate).OrderByDescending(s => s.GameData.TimeInSeconds).ToList();
            List<UserScoreDto<SudokuData>> topScores = new List<UserScoreDto<SudokuData>>();
            
            for(int i = 0; i < topCount && i < orderedScores.Count; i++) {
                topScores.Add(orderedScores[i]);
            }
            
            return topScores;
        }


        public int[,] HideNumbers(int[,] grid, int gridSize, int numbersToRemove) {
            int attempts = 0;
            Dictionary<string,int> hidenNumbers = new Dictionary<string, int>();
            int[,] tempGrid = (int[,])grid.Clone();

            for(int i = 1; i <= numbersToRemove; i++) {
                int row = _random.Next(0, gridSize);
                int col = _random.Next(0, gridSize);

                string hideNumbKey = row.ToString() + "," + col.ToString();
                int hideNumbTemp;

                while(hidenNumbers.ContainsKey(hideNumbKey)) {
                    row = _random.Next(0, gridSize);
                    col = _random.Next(0, gridSize);

                    hideNumbKey = row.ToString() + "," + col.ToString();
                }

                hideNumbTemp = tempGrid![row, col];
                tempGrid[row, col] = 0;

                if(HasMultipleSolutions(tempGrid, gridSize)) {
                    tempGrid[row, col] = hideNumbTemp;
                    i--;
                    attempts++;

                    if(attempts > 1) {
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
                } else {
                    grid[row, col] = 0;
                    hidenNumbers.Add(hideNumbKey, hideNumbTemp);
                    attempts--;
                }
            }

            return grid;
        }

        public int[,] GenerateSolvedSudoku(int gridSize) {
            int[,] grid = new int[gridSize, gridSize];
            List<int> row = new List<int>();

            row.AddRange(Enumerable.Range(1, gridSize).OrderBy(n => _random.Next()));

            for(int j = 0; j < gridSize; j++) {
                grid[0, j] = row[j];
            }

            SolveSudoku(ref grid, gridSize);

            return grid;
        }

        private bool SolveSudoku(ref int[,] grid, int gridSize) {
            int row = -1;
            int col = -1;

            for(int i = 0; i < gridSize; i++) {
                for(int j = 0; j < gridSize; j++) {
                    if(grid[i, j] == 0) {
                        row = i;
                        col = j;
                        break;
                    }
                }
                if(row != -1) {
                    break;
                }
            }

            if(row == -1) {
                return true;
            }

            return PossibleNumbers(row, col, ref grid, gridSize);
        }

        private bool PossibleNumbers(int row, int col, ref int[,] grid, int gridSize) {
            for(int num = 1; num <= gridSize; num++) {
                if(IsValidMove(row, col, num, ref grid, gridSize)) {
                    grid[row, col] = num;

                    if(SolveSudoku(ref grid, gridSize)) {
                        return true;
                    }
                    grid[row, col] = 0;
                }
            }

            return false;
        }

        private bool IsValidMove(int row, int col, int num, ref int[,] grid, int gridSize) {
            for(int i = 0; i < gridSize; i++) {
                if(grid[row, i] == num || grid[i, col] == num) {
                    return false;
                }
            }
            int internalGridSize = (int)Math.Sqrt(gridSize);
            int startRow=row-row% internalGridSize;
            int startCol=col-col% internalGridSize;

            for(int i = startRow; i < startRow + internalGridSize; i++) {
                for(int j = startCol; j < startCol + internalGridSize; j++) {
                    if(grid[i, j] == num) {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
