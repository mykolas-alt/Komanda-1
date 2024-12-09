using Microsoft.AspNetCore.Mvc;
using Projektas.Server.Interfaces;
using Projektas.Shared.Models;
using Projektas.Shared.Enums;

namespace Projektas.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountScoreController : ControllerBase
    {
        private readonly IAccountScoreService _accountScoreService;

        public AccountScoreController(IAccountScoreService accountScoreService)
        {
            _accountScoreService = accountScoreService;
        }

        [HttpGet("scores/math")]
        public async Task<ActionResult<List<UserScoreDto<MathGameData>>>> GetMathScores([FromQuery] string username)
        {
            var user = new User { Username = username };
            var scores = await _accountScoreService.GetUserScores<MathGameData>(user);
            return Ok(scores);
        }

        [HttpGet("scores/aim")]
        public async Task<ActionResult<List<UserScoreDto<AimTrainerData>>>> GetAimScores([FromQuery] string username)
        {
            var user = new User { Username = username };
            var scores = await _accountScoreService.GetUserScores<AimTrainerData>(user);
            return Ok(scores);
        }

        [HttpGet("scores/pairup")]
        public async Task<ActionResult<List<UserScoreDto<PairUpData>>>> GetPairUpScores([FromQuery] string username)
        {
            var user = new User { Username = username };
            var scores = await _accountScoreService.GetUserScores<PairUpData>(user);
            return Ok(scores);
        }

        [HttpGet("scores/sudoku")]
        public async Task<ActionResult<List<UserScoreDto<SudokuData>>>> GetSudokuScores([FromQuery] string username)
        {
            var user = new User { Username = username };
            var scores = await _accountScoreService.GetUserScores<SudokuData>(user);
            return Ok(scores);
        }

        [HttpGet("highscore/math")]
        public async Task<ActionResult<GameScore>> GetMathHighscore([FromQuery] string username)
        {
            var user = new User { Username = username };
            var highscore = await _accountScoreService.GetHighscore<MathGameData>(user);
            return Ok(highscore);
        }

        [HttpGet("highscore/aim")]
        public async Task<ActionResult<GameScore>> GetAimHighscore([FromQuery] string username, [FromQuery] GameDifficulty difficulty)
        {
            var user = new User { Username = username };
            var highscore = await _accountScoreService.GetHighscore<AimTrainerData>(user, difficulty);
            return Ok(highscore);
        }

        [HttpGet("highscore/pairup")]
        public async Task<ActionResult<GameScore>> GetPairUpHighscore([FromQuery] string username, [FromQuery] GameDifficulty difficulty)
        {
            var user = new User { Username = username };
            var highscore = await _accountScoreService.GetHighscore<PairUpData>(user, difficulty);
            return Ok(highscore);
        }

        [HttpGet("highscore/sudoku")]
        public async Task<ActionResult<GameScore>> GetSudokuHighscore([FromQuery] string username, [FromQuery] GameDifficulty difficulty, [FromQuery] GameMode mode)
        {
            var user = new User { Username = username };
            var highscore = await _accountScoreService.GetHighscore<SudokuData>(user, difficulty, mode);
            return Ok(highscore);
        }

        [HttpGet("matches-played/math")]
        public async Task<ActionResult<int>> GetMathMatchesPlayed([FromQuery] string username)
        {
            var user = new User { Username = username };
            var matchesPlayed = await _accountScoreService.GetMatchesPlayed<MathGameData>(user);
            return Ok(matchesPlayed);
        }

        [HttpGet("matches-played/aim")]
        public async Task<ActionResult<int>> GetAimMatchesPlayed([FromQuery] string username, [FromQuery] GameDifficulty difficulty)
        {
            var user = new User { Username = username };
            var matchesPlayed = await _accountScoreService.GetMatchesPlayed<AimTrainerData>(user, difficulty);
            return Ok(matchesPlayed);
        }

        [HttpGet("matches-played/pairup")]
        public async Task<ActionResult<int>> GetPairUpMatchesPlayed([FromQuery] string username, [FromQuery] GameDifficulty difficulty)
        {
            var user = new User { Username = username };
            var matchesPlayed = await _accountScoreService.GetMatchesPlayed<PairUpData>(user, difficulty);
            return Ok(matchesPlayed);
        }

        [HttpGet("matches-played/sudoku")]
        public async Task<ActionResult<int>> GetSudokuMatchesPlayed([FromQuery] string username, [FromQuery] GameDifficulty difficulty, [FromQuery] GameMode mode)
        {
            var user = new User { Username = username };
            var matchesPlayed = await _accountScoreService.GetMatchesPlayed<SudokuData>(user, difficulty, mode);
            return Ok(matchesPlayed);
        }

        [HttpGet("average-score/math")]
        public async Task<ActionResult<GameScore>> GetMathAverageScore([FromQuery] string username)
        {
            var user = new User { Username = username };
            var averageScore = await _accountScoreService.GetAllTimeAverageScore<MathGameData>(user);
            return Ok(averageScore);
        }

        [HttpGet("average-score/aim")]
        public async Task<ActionResult<GameScore>> GetAimAverageScore([FromQuery] string username, [FromQuery] GameDifficulty difficulty)
        {
            var user = new User { Username = username };
            var averageScore = await _accountScoreService.GetAllTimeAverageScore<AimTrainerData>(user, difficulty);
            return Ok(averageScore);
        }

        [HttpGet("average-score/pairup")]
        public async Task<ActionResult<GameScore>> GetPairUpAverageScore([FromQuery] string username, [FromQuery] GameDifficulty difficulty)
        {
            var user = new User { Username = username };
            var averageScore = await _accountScoreService.GetAllTimeAverageScore<PairUpData>(user, difficulty);
            return Ok(averageScore);
        }

        [HttpGet("average-score/sudoku")]
        public async Task<ActionResult<GameScore>> GetSudokuAverageScore([FromQuery] string username, [FromQuery] GameDifficulty difficulty, [FromQuery] GameMode mode)
        {
            var user = new User { Username = username };
            var averageScore = await _accountScoreService.GetAllTimeAverageScore<SudokuData>(user, difficulty, mode);
            return Ok(averageScore);
        }


        [HttpGet("average-score-last-7days/math")]
        public async Task<ActionResult<List<AverageScoreDto>>> GetMathAverageScoreLast7Days([FromQuery] string username)
        {
            var user = new User { Username = username };
            var averageScoreLast7Days = await _accountScoreService.GetAverageScoreLast7Days<MathGameData>(user);
            return Ok(averageScoreLast7Days);
        }

        [HttpGet("average-score-last-7days/aim")]
        public async Task<ActionResult<List<AverageScoreDto>>> GetAimAverageScoreLast7Days([FromQuery] string username, [FromQuery] GameDifficulty difficulty)
        {
            var user = new User { Username = username };
            var averageScoreLast7Days = await _accountScoreService.GetAverageScoreLast7Days<AimTrainerData>(user, difficulty);
            return Ok(averageScoreLast7Days);
        }

        [HttpGet("average-score-last-7days/pairup")]
        public async Task<ActionResult<List<AverageScoreDto>>> GetPairUpAverageScoreLast7Days([FromQuery] string username, [FromQuery] GameDifficulty difficulty)
        {
            var user = new User { Username = username };
            var averageScoreLast7Days = await _accountScoreService.GetAverageScoreLast7Days<PairUpData>(user, difficulty);
            return Ok(averageScoreLast7Days);
        }

        [HttpGet("average-score-last-7days/sudoku")]
        public async Task<ActionResult<List<AverageScoreDto>>> GetSudokuAverageScoreLast7Days([FromQuery] string username, [FromQuery] GameDifficulty difficulty, [FromQuery] GameMode mode)
        {
            var user = new User { Username = username };
            var averageScoreLast7Days = await _accountScoreService.GetAverageScoreLast7Days<SudokuData>(user, difficulty, mode);
            return Ok(averageScoreLast7Days);
        }
    }
}