using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Projektas.Server.Services;
using Projektas.Shared.Models;

namespace Projektas.Server.Controllers
{
    [ApiController]
	[Route("api/[controller]")]
	public class AccountController:ControllerBase {
		private readonly UserService _userService;
		private readonly IConfiguration _configuration;

		public AccountController(UserService userService, IConfiguration configuration) {
			_userService=userService;
			_configuration=configuration;
		}

		[HttpPost("login")]
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