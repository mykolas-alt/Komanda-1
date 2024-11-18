using Microsoft.AspNetCore.Mvc;
using Projektas.Server.Interfaces.MathGame;
using Projektas.Server.Services.MathGame;
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
        public async Task SaveScore([FromBody] UserScoreDto data) {
            await _scoreboardService.AddScoreToDb(data);
        }

        [HttpGet("highscore")]
        public async Task<ActionResult<int>> GetUserHighscore([FromQuery] string username) {
            return await _scoreboardService.GetUserHighscore(username);
        }

        [HttpGet("top-score")]
        public async Task<ActionResult<List<UserScoreDto>>> GetTopScores([FromQuery] int topCount) {
            return await _scoreboardService.GetTopScores(topCount);
        }

    }
}