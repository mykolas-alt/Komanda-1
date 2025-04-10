using Microsoft.AspNetCore.Mvc;
using Projektas.Server.Interfaces.MathGame;
using Projektas.Shared.Enums;
using Projektas.Shared.Models;

namespace Projektas.Server.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class MathGameController : ControllerBase {
        private readonly IMathGameService _mathGameService;
        private readonly IMathGameScoreboardService _scoreboardService;

        public MathGameController(IMathGameService mathGameService,IMathGameScoreboardService mathGameScoreboardService) {
            _mathGameService = mathGameService;
            _scoreboardService = mathGameScoreboardService;
        }

        [HttpGet("question")]
        public ActionResult<string> GetQuestion([FromQuery] int score) {
            return _mathGameService.GenerateQuestion(score);
        }

        [HttpGet("options")]
        public ActionResult<List<int>> GetOptions() {
            return _mathGameService.GenerateOptions();
        }

        [HttpPost("check-answer")]
        public ActionResult<bool> CheckAnswer([FromBody] int answer) {
            return _mathGameService.CheckAnswer(answer);
        }

        [HttpPost("save-score")]
        public async Task SaveScoreAsync([FromBody] UserScoreDto<MathGameData> data) {
            await _scoreboardService.AddScoreToDbAsync(data);
        }

        [HttpGet("highscore")]
        public async Task<ActionResult<UserScoreDto<MathGameData>?>> GetUserHighscoreAsync([FromQuery] string username, [FromQuery] GameDifficulty difficulty) {
            return await _scoreboardService.GetUserHighscoreAsync(username, difficulty);
        }

        [HttpGet("top-score")]
        public async Task<ActionResult<List<UserScoreDto<MathGameData>>>> GetTopScoresAsync([FromQuery] int topCount, [FromQuery] GameDifficulty difficulty) {
            return await _scoreboardService.GetTopScoresAsync(topCount, difficulty);
        }

    }
}