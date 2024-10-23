using Microsoft.AspNetCore.Mvc;
using Projektas.Server.Services.MathGame;

namespace Projektas.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MathGameController : ControllerBase
    {
        private readonly MathGameAPIService _mathGameService;

        public MathGameController(MathGameAPIService mathGameService)
        {
            _mathGameService = mathGameService;
        }

        [HttpGet("question")]
        public ActionResult<string> GetQuestion([FromQuery] int score)
        {
            return _mathGameService.GenerateQuestion(score);
        }

        [HttpGet("options")]
        public ActionResult<List<int>> GetOptions()
        {
            return _mathGameService.GenerateOptions();
        }

        [HttpPost("check-answer")]
        public ActionResult<bool> CheckAnswer([FromBody] int answer)
        {
            return _mathGameService.CheckAnswer(answer);
        }
    }
}