using Microsoft.AspNetCore.Mvc;
using Projektas.Server.Services;

namespace Projektas.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MathGameController : ControllerBase
    {
        private readonly MathGameService _mathGameService;

        public MathGameController(MathGameService mathGameService)
        {
            _mathGameService = mathGameService;
        }

        [HttpGet("question")]
        public ActionResult<string> GetQuestion()
        {
            return _mathGameService.GenerateQuestion();
        }

        [HttpGet("options")]
        public ActionResult<List<int>> GetOptions()
        {
            return _mathGameService.GenerateOptions();
        }

        [HttpPost("check-answer")]
        public IActionResult CheckAnswer([FromBody] int answer)
        {
            _mathGameService.CheckAnswer(answer);
            return Ok();
        }

        [HttpGet("score")]
        public ActionResult<int> GetScore()
        {
            return _mathGameService.Score;
        }

        [HttpGet("lives")]
        public ActionResult<int> GetLives()
        {
            return _mathGameService.Lives;
        }

        [HttpGet("highscore")]
        public ActionResult<int> GetHighscore()
        {
            return _mathGameService.Highscore;
        }

        [HttpPost("reset-score")]
        public ActionResult<int> ResetScore()
        {
            _mathGameService.Score = 0;
            return _mathGameService.Score;
        }

        [HttpPost("reset-lives")]
        public ActionResult<int> ResetLives()
        {
            _mathGameService.Lives = 3;
            return _mathGameService.Lives;
        }
    }
}