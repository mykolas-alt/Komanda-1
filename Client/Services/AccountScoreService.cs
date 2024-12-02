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
        public async Task<int?> GetAimTrainerHighscoreNormalModeAsync(string username)
        {
            var url = $"api/accountscore/aim-trainer-highscore-normal-mode?username={username}";
            return await _httpClient.GetFromJsonAsync<int?>(url);
        }
        public async Task<int?> GetAimTrainerHighscoreHardModeAsync(string username)
        {
            var url = $"api/accountscore/aim-trainer-highscore-hard-mode?username={username}";
            return await _httpClient.GetFromJsonAsync<int?>(url);
        }
        public async Task<int?> GetPairUpHighscoreNormalModeAsync(string username)
        {
            var url = $"api/accountscore/pair-up-highscore-normal-mode?username={username}";
            return await _httpClient.GetFromJsonAsync<int?>(url);
        }
        public async Task<int?> GetPairUpHighscoreHardModeAsync(string username)
        {
            var url = $"api/accountscore/pair-up-highscore-hard-mode?username={username}";
            return await _httpClient.GetFromJsonAsync<int?>(url);
        }
        public async Task<int?> GetMathGameAllTimeAverageAsync(string username)
        {
            var url = $"api/accountscore/math-game-average-score?username={username}";
            return await _httpClient.GetFromJsonAsync<int?>(url);
        }
        public async Task<int?> GetAimTrainerAllTimeAverageNormalModeAsync(string username)
        {
            var url = $"api/accountscore/aim-trainer-average-score-normal-mode?username={username}";
            return await _httpClient.GetFromJsonAsync<int?>(url);
        }
        public async Task<int?> GetAimTrainerAllTimeAverageHardModeAsync(string username)
        {
            var url = $"api/accountscore/aim-trainer-average-score-hard-mode?username={username}";
            return await _httpClient.GetFromJsonAsync<int?>(url);
        }
        public async Task<int?> GetPairUpAllTimeAverageNormalModeAsync(string username)
        {
            var url = $"api/accountscore/pair-up-average-score-normal-mode?username={username}";
            return await _httpClient.GetFromJsonAsync<int?>(url);
        }
        public async Task<int?> GetPairUpAllTimeAverageHardModeAsync(string username)
        {
            var url = $"api/accountscore/pair-up-average-score-hard-mode?username={username}";
            return await _httpClient.GetFromJsonAsync<int?>(url);
        }
        public async Task<int> GetMathGameMatchesPlayedAsync(string username)
        {
            var url = $"api/accountscore/math-game-matches-played?username={username}";
            return await _httpClient.GetFromJsonAsync<int>(url);
        }

        public async Task<int> GetTotalAimTrainerMatchesPlayedAsync(string username)
        {
            var url = $"api/accountscore/aim-trainer-matches-played?username={username}";
            return await _httpClient.GetFromJsonAsync<int>(url);
        }

        public async Task<int> GetAimTrainerMatchesPlayedNormalModeAsync(string username)
        {
            var url = $"api/accountscore/aim-trainer-matches-played-normal-mode?username={username}";
            return await _httpClient.GetFromJsonAsync<int>(url);
        }

        public async Task<int> GetAimTrainerMatchesPlayedHardModeAsync(string username)
        {
            var url = $"api/accountscore/aim-trainer-matches-played-hard-mode?username={username}";
            return await _httpClient.GetFromJsonAsync<int>(url);
        }

        public async Task<int> GetTotalPairUpMatchesPlayedAsync(string username)
        {
            var url = $"api/accountscore/pair-up-matches-played?username={username}";
            return await _httpClient.GetFromJsonAsync<int>(url);
        }

        public async Task<int> GetPairUpMatchesPlayedNormalModeAsync(string username)
        {
            var url = $"api/accountscore/pair-up-matches-played-normal-mode?username={username}";
            return await _httpClient.GetFromJsonAsync<int>(url);
        }

        public async Task<int> GetPairUpMatchesPlayedHardModeAsync(string username)
        {
            var url = $"api/accountscore/pair-up-matches-played-hard-mode?username={username}";
            return await _httpClient.GetFromJsonAsync<int>(url);
        }

        public async Task<List<AverageScoreDto>> GetMathGameAverageScoreLast7Days(string username)
        {
            var url = $"api/accountscore/math-game-average-score-last-7days?username={username}";
            return await _httpClient.GetFromJsonAsync<List<AverageScoreDto>>(url);
        }
    }
}