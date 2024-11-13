using Microsoft.AspNetCore.Mvc;
using Projektas.Server.Services.MathGame;
using Projektas.Shared.Models;

namespace Projektas.Server.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class MathGameController : ControllerBase {
        private readonly MathGameService _mathGameService;
        private readonly MathGameScoreboardService _scoreboardService;

        public MathGameController(MathGameService mathGameService,MathGameScoreboardService mathGameScoreboardService) {
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

        [HttpGet("top")]
        public async Task<ActionResult<List<UserScoreDto>>> GetTopScores([FromQuery] int topCount) {
            return await _scoreboardService.GetTopScores(topCount);
        }

    }
}