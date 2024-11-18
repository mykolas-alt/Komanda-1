using System.Security.Claims;

namespace Projektas.Client.Interfaces
{
    public interface IAccountAuthStateProvider
    {
        string? Username { get; }
        Task<AuthenticationState> GetAuthenticationStateAsync();
        IEnumerable<Claim> ParseClaimsFromJWT(string jwt);
        Task MarkUserAsAuthenticated(string token);
        void MarkUserAsLoggedOut();
        Task<string?> GetUsernameAsync();
		event AuthenticationStateChangedHandler? AuthenticationStateChanged;
	}
}
