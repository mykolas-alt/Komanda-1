using Projektas.Shared.Models;
using System.Net.Http.Json;

namespace Projektas.Client.Services
{
    public class AccountServices {
		private readonly HttpClient _httpClient;

		public AccountServices(HttpClient httpClient) {
			_httpClient = httpClient;
		}

		public async Task<bool> LogIn(User user) {
			var response=await _httpClient.PostAsJsonAsync("api/account/log_in",user);

			if(response.IsSuccessStatusCode) {
				return true;
			} else {
				return false;
			}
		}

		public async Task CreateAccount(User newUser) {
			await _httpClient.PostAsJsonAsync("api/account/create_user",newUser);
		}

		public async Task<List<string>> GetUsernames() {
			return await _httpClient.GetFromJsonAsync<List<string>>("api/account/get_usernames");
		}
	}
}