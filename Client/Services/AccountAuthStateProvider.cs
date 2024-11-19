using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using Projektas.Client.Interfaces;

namespace Projektas.Client.Services {
	public class AccountAuthStateProvider : AuthenticationStateProvider,IAccountAuthStateProvider {
		private readonly ILocalStorageService _localStorage;
		private readonly HttpClient _httpClient;
		public string? Username {get;private set;}=null;

		public AccountAuthStateProvider(ILocalStorageService localStorage,HttpClient httpClient) {
			_localStorage=localStorage;
			_httpClient=httpClient;
		}

		public override async Task<AuthenticationState> GetAuthenticationStateAsync() {
			var token=await _localStorage.GetItemAsync<string>("token");

			if(string.IsNullOrEmpty(token)) {
				return await Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())));
			}

			_httpClient.DefaultRequestHeaders.Authorization=new AuthenticationHeaderValue("Bearer",token);

			await GetUsernameAsync();

			var claims=ParseClaimsFromJWT(token);
			return await Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(claims,"jwt"))));
		}

		public IEnumerable<Claim> ParseClaimsFromJWT(string jwt) {
			if (string.IsNullOrEmpty(jwt)) {
				throw new ArgumentException("JWT token is null or empty.");
			}

			var parts=jwt.Split('.');
			if (parts.Length!=3) {
				throw new ArgumentException("JWT token format is invalid.");
			}

			var payload=parts[1];
			// Pad the payload to be base64-url compatible if needed
			payload=payload.PadRight(payload.Length+(4-payload.Length%4)%4,'=');
			var jsonBytes=Convert.FromBase64String(payload.Replace('-','+').Replace('_','/'));
			var keyValuePairs=JsonSerializer.Deserialize<Dictionary<string,object>>(jsonBytes);
    
			return keyValuePairs.Select(kvp => new Claim(kvp.Key,kvp.Value?.ToString() ?? ""));
		}

		public async Task MarkUserAsAuthenticated(string token) {
			await _localStorage.SetItemAsync("token",token);

			_httpClient.DefaultRequestHeaders.Authorization=new AuthenticationHeaderValue("Bearer",token);

			var claims=ParseClaimsFromJWT(token);
			Username=claims.FirstOrDefault(c => c.Type==ClaimTypes.Name)?.Value;

			var authenticatedUser=new ClaimsPrincipal(new ClaimsIdentity(claims,"jwt"));
			NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(authenticatedUser)));
		}

		public void MarkUserAsLoggedOut() {
			Username=null;
			_localStorage.RemoveItemAsync("token");

			_httpClient.DefaultRequestHeaders.Authorization=null;

			var anonymous=new ClaimsPrincipal(new ClaimsIdentity());
			NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(anonymous)));
		}

		public async Task<string?> GetUsernameAsync() {
			if (Username==null) {
				var token=await _localStorage.GetItemAsync<string>("token");
				if (!string.IsNullOrEmpty(token)) {
					var claims=ParseClaimsFromJWT(token);
					Username=claims.FirstOrDefault(c => c.Type==ClaimTypes.Name)?.Value;
				}
			}
			return Username;
		}
	}
}
