using Projektas.Shared.Models;
using System.Net.Http.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Projektas.Client.Services
{
    public class AccountService {
		private readonly HttpClient _httpClient;
		private readonly ILocalStorageService _localStorage;

		public AccountService(HttpClient httpClient, ILocalStorageService localStorage) {
			_httpClient = httpClient;
			_localStorage = localStorage;
		}
		
		public async Task<HttpResponseMessage> CreateAccountAsync(User user) {
			return await _httpClient.PostAsJsonAsync("api/database/create_user",user);
		}

		public async Task<string?> LogIn(User user) {
			var response=await _httpClient.PostAsJsonAsync("api/user/login",user);

			if(response.IsSuccessStatusCode) {
				var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
				return result?.Token;
			}

			return "";
		}

		public async Task<List<string>> GetUsernames() {
			return await _httpClient.GetFromJsonAsync<List<string>>("api/user/get_usernames");
		}
	}
}