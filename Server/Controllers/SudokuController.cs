using Microsoft.AspNetCore.Mvc;
using Projektas.Server.Services;
using Projektas.Shared.Models;

namespace Projektas.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SudokuController : ControllerBase
    {
        private readonly SudokuService _sudokuService;

        public SudokuController(SudokuService sudokuService)
        {
            _sudokuService = sudokuService;
        }

        [HttpGet("has-multiple-solutions")]
        public ActionResult<bool> HasMultipleSolutions([FromQuery] int[] grid)
        {
            int gridSize = (int)Math.Sqrt(grid.Length);
            int[,] grid2D = new int[gridSize, gridSize];

            for (int i = 0; i < gridSize; i++)
            {
                for (int j = 0; j < gridSize; j++)
                {
                    grid2D[i, j] = grid[i * gridSize + j];
                }
            }
            return _sudokuService.HasMultipleSolutions(grid2D, gridSize);
        }

        [HttpPost("save-score")]
        public async Task SaveScore([FromBody] UserScoreDto data) {
            await _sudokuService.AddScoreToDb(data);
        }

        [HttpGet("highscore")]
        public async Task<ActionResult<int>> GetUserHighscore([FromQuery] string username) {
            return await _sudokuService.GetUserHighscore(username);
        }

        [HttpGet("top-score")]
        public async Task<ActionResult<List<UserScoreDto>>> GetTopScores([FromQuery] int topCount) {
            return await _sudokuService.GetTopScores(topCount);
        }
    }
}