using Projektas.Shared;
using System.Net.Http.Json;

namespace Projektas.Client.Services {
	public class AccountServices {
		private readonly HttpClient _httpClient;

		public AccountServices(HttpClient httpClient) {
			_httpClient = httpClient;
		}

		public async Task CreateAccount(AccountInfo newAccount) {
			await _httpClient.PostAsJsonAsync("api/account/create_account",newAccount);
		}
	}
}