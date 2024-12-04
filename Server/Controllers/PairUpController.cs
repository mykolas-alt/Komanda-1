﻿using Microsoft.AspNetCore.Mvc;
using Projektas.Server.Services;
using Projektas.Shared.Models;

namespace Projektas.Server.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class PairUpController : Controller {
        private readonly PairUpService _pairUpService;

        public PairUpController (PairUpService pairUpService) {
            _pairUpService=pairUpService;
        }

        [HttpPost("save-score")]
        public async Task SaveScoreAsync([FromBody] UserScoreDto<PairUpData> data) {
            await _pairUpService.AddScoreToDbAsync(data);
        }

        [HttpGet("highscore")]
        public async Task<ActionResult<UserScoreDto<PairUpData>?>> GetUserHighscoreAsync([FromQuery] string username) {
            return await _pairUpService.GetUserHighscoreAsync(username);
        }

        [HttpGet("top-score")]
        public async Task<ActionResult<List<UserScoreDto<PairUpData>>>> GetTopScoresAsync([FromQuery] int topCount) {
            return await _pairUpService.GetTopScoresAsync(topCount);
        }
    }
}
