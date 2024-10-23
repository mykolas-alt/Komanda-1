using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Projektas.Server.Services;
using Projektas.Shared.Models;

namespace Projektas.Server.Controllers
{
    [ApiController]
	[Route("api/[controller]")]
	public class AccountController:ControllerBase {
		private readonly UserService _userService;

		public AccountController(UserService userService) {
			_userService=userService;	
		}

		[HttpPost("log_in")]
		public IActionResult LogIn([FromBody]User user) {
			var response=_userService.LogInToUser(user);
			return Ok(response);
		}

		[HttpPost("create_user")]
		public IActionResult CreateUser([FromBody] User newUser) {
			_userService.CreateUser(newUser);
			return Ok();
		}

		[HttpGet("get_usernames")]
		public ActionResult <List<string>> GetUsernames() {
			return _userService.GetUsernames();
		}
	}
}