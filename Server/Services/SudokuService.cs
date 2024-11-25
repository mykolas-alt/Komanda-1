namespace Projektas.Server.Services
{
    using Google.OrTools.ConstraintSolver;
    using Projektas.Shared.Models;

    public class SudokuService
    {
        private readonly IScoreRepository<SudokuM> _scoreRepository;
        
        private Solver? _solver = null;
        private IntVar[,]? _possibleGrid = null;
        private DecisionBuilder? _decisionBuilder = null;
        private SolutionCollector? _solutionCollector = null;

        public SudokuService (IScoreRepository<SudokuM> scoreRepository) {
            _scoreRepository=scoreRepository;
        }

        public bool HasMultipleSolutions(int[,] grid,int gridSize)
        {
            SetUpConstrains(grid, gridSize, gridSize / 3);

            _solutionCollector = _solver!.MakeAllSolutionCollector();
            _solutionCollector.Add(_possibleGrid.Flatten());

            _solver.NewSearch(_decisionBuilder, _solutionCollector);

            while (_solver.NextSolution())
            {
                if (_solutionCollector.SolutionCount() > 1)
                {
                    DisposeResources();

                    return false;
                }
            }

            DisposeResources();

            return true;
        }

        private void SetUpConstrains(int[,] grid, int gridSize, int internalGridSize)
        {
            _solver = new Solver("SudokuSolver");
            _possibleGrid = _solver.MakeIntVarMatrix(gridSize, gridSize, 1, gridSize, "grid");

            for (int i = 0; i < gridSize; i++)
            {
                _solver!.Add((from j in Enumerable.Range(0, gridSize) select _possibleGrid![i, j]).ToArray().AllDifferent());
                _solver!.Add((from j in Enumerable.Range(0, gridSize) select _possibleGrid![j, i]).ToArray().AllDifferent());
            }

            for (int i = 0; i < internalGridSize; i++)
            {
                for (int j = 0; j < internalGridSize; j++)
                {
                    _solver!.Add((from x in Enumerable.Range(i * internalGridSize, internalGridSize) from y in Enumerable.Range(j * internalGridSize, internalGridSize) select _possibleGrid![x, y]).ToArray().AllDifferent());
                }
            }

            for (int i = 0; i < gridSize; i++)
            {
                for (int j = 0; j < gridSize; j++)
                {
                    if (grid[i, j] != 0)
                    {
                        _solver!.Add(_possibleGrid![i, j] == grid[i, j]);
                    }
                }
            }

            _decisionBuilder = _solver!.MakePhase(_possibleGrid.Flatten(), Solver.INT_VAR_SIMPLE, Solver.ASSIGN_MIN_VALUE);
        }



        private void DisposeResources()
        {
            _decisionBuilder!.Dispose();
            _solver!.Dispose();
            if (_solutionCollector != null) _solutionCollector!.Dispose();
            _possibleGrid = null;
        }

        public async Task AddScoreToDb(UserScoreDto data) {
            await _scoreRepository.AddScoreToUserAsync(data.Username,data.Score);
        }

        public async Task<int?> GetUserHighscore(string username) {
            return await _scoreRepository.GetHighscoreFromUserAsync(username);
        }

        public async Task<List<UserScoreDto>> GetTopScores(int topCount) {
            List<UserScoreDto> userScores=await _scoreRepository.GetAllScoresAsync();
            List<UserScoreDto> topScores=new List<UserScoreDto>();
            
            for(int i=0;i<topCount && i<userScores.Count;i++) {
                topScores.Add(userScores[i]);
            }
            
            return topScores;
        }
    }
}
