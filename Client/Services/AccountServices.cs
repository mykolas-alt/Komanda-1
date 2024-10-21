using Projektas.Shared;
using System.Net.Http.Json;

namespace Projektas.Client.Services {
	public class AccountServices {
		private readonly HttpClient _httpClient;

		public AccountServices(HttpClient httpClient) {
			_httpClient = httpClient;
		}

		public async Task<bool> LogIn(AccountInfo account) {
			var response=await _httpClient.PostAsJsonAsync("api/account/log_in",account);

			if(response.IsSuccessStatusCode) {
				return true;
			} else {
				return false;
			}
		}

		public async Task CreateAccount(AccountInfo newAccount) {
			await _httpClient.PostAsJsonAsync("api/account/create_account",newAccount);
		}

		public async Task<List<string>> GetNicknames() {
			return await _httpClient.GetFromJsonAsync<List<string>>("api/account/get_nicknames");
		}
	}
}