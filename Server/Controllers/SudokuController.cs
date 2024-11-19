using Microsoft.AspNetCore.Mvc;
using Projektas.Server.Services;

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

    }
}