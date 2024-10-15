using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Projektas.Server.Services;
using Projektas.Shared;

namespace Projektas.Server.Controllers {
	[ApiController]
	[Route("api/[controller]")]
	public class AccountController:ControllerBase {
		private readonly AccountService _accountService;

		public AccountController(AccountService accountService) {
			_accountService=accountService;	
		}


		[HttpPost("create_account")]
		public IActionResult CreateAccount([FromBody] AccountInfo newAccount) {
			_accountService.CreateAccount(newAccount);
			return Ok();
		}
	}
}