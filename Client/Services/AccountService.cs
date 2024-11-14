using Projektas.Shared.Models;
using System.Net.Http.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Projektas.Client.Interfaces;

namespace Projektas.Client.Services
{
    public class AccountService : IAccountService {
		private readonly HttpClient _httpClient;
		private readonly ILocalStorageService _localStorage;

		public AccountService(HttpClient httpClient, ILocalStorageService localStorage) {
			_httpClient = httpClient;
			_localStorage = localStorage;
		}

		public async Task<string?> LogIn(User user) {
			var response=await _httpClient.PostAsJsonAsync("api/account/login",user);

			if(response.IsSuccessStatusCode) {
				var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
				return result?.Token;
			}

			return "";
		}

		public async Task CreateAccount(User newUser) {
			await _httpClient.PostAsJsonAsync("api/account/create_user",newUser);
		}

		public async Task<List<string>> GetUsernames() {
			return await _httpClient.GetFromJsonAsync<List<string>>("api/account/get_usernames");
		}
	}
}