using Microsoft.AspNetCore.Mvc;
using Projektas.Server.Services;
using Projektas.Shared.Models;

namespace Projektas.Server.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class SudokuController : ControllerBase {
        private readonly SudokuService _sudokuService;

        public SudokuController(SudokuService sudokuService) {
            _sudokuService=sudokuService;
        }


        [HttpGet("generate-sudoku")]
        public ActionResult<List<List<int>>> GenerateSolvedSudoku(int gridSize) {
            var grid=_sudokuService.GenerateSolvedSudoku(gridSize);

            var gridList=new List<List<int>>();

            for(int i=0;i<gridSize;i++) {
                var row=new List<int>();
                for(int j=0;j<gridSize;j++) {
                    row.Add(grid[i,j]);
                }
                gridList.Add(row);
            }

            return gridList;
        }

        [HttpGet("hide-numbers")]
        public async Task<ActionResult<List<List<int>>>> HideNumbers([FromQuery] int gridSize,[FromQuery] int numbersToRemove,[FromQuery] int[] grid) {
            int[,] grid2D=new int[gridSize,gridSize];
            for(int i=0;i<gridSize;i++) {
                for(int j=0;j<gridSize;j++) {
                    grid2D[i,j]=grid[i*gridSize+j];
                }
            }

            var updatedGrid=_sudokuService.HideNumbers(grid2D,gridSize,numbersToRemove);

            var result=new List<List<int>>();
            for(int i=0;i<updatedGrid.GetLength(0);i++) {
                var row = new List<int>();
                for(int j=0;j<updatedGrid.GetLength(1);j++) {
                    row.Add(updatedGrid[i,j]);
                }
                result.Add(row);
            }

            return result;
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