using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Projektas.Server.Services;

namespace Projektas.Server.Controllers {
	[ApiController]
	[Route("api/[controller]")]
	public class AccountController:ControllerBase {
		private readonly AccountService _accountService;

		public AccountController(AccountService accountService) {
			_accountService=accountService;	
		}


		[HttpGet("test")]
		public ActionResult<string> GetTest() {
			return _accountService.GetTestServ();
		}
	}
}