using Microsoft.AspNetCore.Mvc;
using Projektas.Server.Interfaces;
using Projektas.Shared.Models;

namespace Projektas.Server.Controllers {
    [ApiController]
	[Route("api/[controller]")]
	public class UserController:ControllerBase {
		private readonly IUserService _userService;

		public UserController(IUserService userService) {
			_userService = userService;
		}
		
		[HttpPost("create_user")]
		public async Task<IActionResult> CreateUserAsync([FromBody] User newUser) {
			await _userService.CreateUserAsync(newUser);
			return Ok();
		}

		[HttpPost("login")]
		public async Task<IActionResult> LogInAsync([FromBody]User user) {
			var response = await _userService.LogInToUserAsync(user);

			if(response) {
				var token = _userService.GenerateJwtToken(user);
				return Ok(new {Token = token});
			}

			return Unauthorized();
		}

		[HttpDelete("logoff")]
		public void LogOff([FromQuery]string username) {
			_userService.LogOffFromUser(username);
		}

		[HttpGet("usernames")]
		public async Task<ActionResult<List<string>>> GetUsernamesAsync() {
			return await _userService.GetUsernamesAsync();
		}
	}
}