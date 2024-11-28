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

        public async Task<int?> GetMathGameHighscoreAsync(string username)
        {
            var url = $"api/accountscore/math-game-highscore?username={username}";
            return await _httpClient.GetFromJsonAsync<int?>(url);
        }
        public async Task<int?> GetAimTrainerHighscoreAsync(string username)
        {
            var url = $"api/accountscore/aim-trainer-highscore?username={username}";
            return await _httpClient.GetFromJsonAsync<int?>(url);
        }
        public async Task<int?> GetPairUpHighscoreAsync(string username)
        {
            var url = $"api/accountscore/pair-up-highscore?username={username}";
            return await _httpClient.GetFromJsonAsync<int?>(url);
        }
        public async Task<int?> GetMathGameAllTimeAverageAsync(string username)
        {
            var url = $"api/accountscore/math-game-average-score?username={username}";
            return await _httpClient.GetFromJsonAsync<int?>(url);
        }
        public async Task<int?> GetAimTrainerAllTimeAverageAsync(string username)
        {
            var url = $"api/accountscore/aim-trainer-average-score?username={username}";
            return await _httpClient.GetFromJsonAsync<int?>(url);
        }
        public async Task<int?> GetPairUpAllTimeAverageAsync(string username)
        {
            var url = $"api/accountscore/pair-up-average-score?username={username}";
            return await _httpClient.GetFromJsonAsync<int?>(url);
        }
    }
}