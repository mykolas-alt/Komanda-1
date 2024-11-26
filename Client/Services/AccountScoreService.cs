using Projektas.Client.Pages;
using Projektas.Shared.Models;
using System.Net.Http.Json;

namespace Projektas.Client.Services
{
    public class AccountScoreService
    {
        private readonly HttpClient _httpClient;

        public AccountScoreService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<UserScoreDto>> GetUsersMathGameScoreAsync(string username)
        {
            var url = $"api/accountscore/math-game-scores?username={username}";
            return await _httpClient.GetFromJsonAsync<List<UserScoreDto>>(url);
        }

        public async Task<List<UserScoreDto>> GetUsersAimTrainerScoreAsync(string username)
        {
            var url = $"api/accountscore/aim-trainer-scores?username={username}";
            return await _httpClient.GetFromJsonAsync<List<UserScoreDto>>(url);
        }

        public async Task<List<UserScoreDto>> GetUsersPairUpScoreAsync(string username)
        {
            var url = $"api/accountscore/pair-up-scores?username={username}";
            return await _httpClient.GetFromJsonAsync<List<UserScoreDto>>(url);
        }
    }
}