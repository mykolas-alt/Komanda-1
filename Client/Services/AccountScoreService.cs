using Projektas.Shared.Enums;
using Projektas.Shared.Models;
using System.Net.Http.Json;
using Projektas.Client.Interfaces;

namespace Projektas.Client.Services
{
    public class AccountScoreService : IAccountScoreService
    {
        private readonly HttpClient _httpClient;

        public AccountScoreService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        private string ConstructUrl(string endpoint, string username, GameDifficulty? difficulty = null, GameMode? mode = null)
        {
            return $"api/accountscore/{endpoint}?username={username}&difficulty={difficulty}&mode={mode}";
        }

        public async Task<List<UserScoreDto<MathGameData>>> GetMathGameScoresAsync(string username)
        {
            var url = $"api/accountscore/scores/math?username={username}";
            return await _httpClient.GetFromJsonAsync<List<UserScoreDto<MathGameData>>>(url);
        }

        public async Task<List<UserScoreDto<AimTrainerData>>> GetAimTrainerScoresAsync(string username)
        {
            var url = $"api/accountscore/scores/aim?username={username}";
            return await _httpClient.GetFromJsonAsync<List<UserScoreDto<AimTrainerData>>>(url);
        }

        public async Task<List<UserScoreDto<PairUpData>>> GetPairUpScoresAsync(string username)
        {
            var url = $"api/accountscore/scores/pairup?username={username}";
            return await _httpClient.GetFromJsonAsync<List<UserScoreDto<PairUpData>>>(url);
        }

        public async Task<List<UserScoreDto<SudokuData>>> GetSudokuScoresAsync(string username)
        {
            var url = $"api/accountscore/scores/sudoku?username={username}";
            return await _httpClient.GetFromJsonAsync<List<UserScoreDto<SudokuData>>>(url);
        }
        public async Task<GameScore> GetMathGameHighscoreAsync(string username, GameDifficulty? difficulty = null, GameMode? mode = null)
        {
            var url = ConstructUrl("highscore/math", username, difficulty, mode);
            return await _httpClient.GetFromJsonAsync<GameScore>(url);
        }

        public async Task<GameScore> GetAimTrainerHighscoreAsync(string username, GameDifficulty? difficulty = null, GameMode? mode = null)
        {
            var url = ConstructUrl("highscore/aim", username, difficulty, mode);
            return await _httpClient.GetFromJsonAsync<GameScore>(url);
        }

        public async Task<GameScore> GetPairUpHighscoreAsync(string username, GameDifficulty? difficulty = null, GameMode? mode = null)
        {
            var url = ConstructUrl("highscore/pairup", username, difficulty, mode);
            return await _httpClient.GetFromJsonAsync<GameScore>(url);
        }

        public async Task<GameScore> GetSudokuHighscoreAsync(string username, GameDifficulty? difficulty = null, GameMode? mode = null)
        {
            var url = ConstructUrl("highscore/sudoku", username, difficulty, mode);
            return await _httpClient.GetFromJsonAsync<GameScore>(url);
        }

        public async Task<GameScore> GetMathGameAverageScoreAsync(string username, GameDifficulty? difficulty = null, GameMode? mode = null)
        {
            var url = ConstructUrl("average-score/math", username, difficulty, mode);
            return await _httpClient.GetFromJsonAsync<GameScore>(url);
        }

        public async Task<GameScore> GetAimTrainerAverageScoreAsync(string username, GameDifficulty? difficulty = null, GameMode? mode = null)
        {
            var url = ConstructUrl("average-score/aim", username, difficulty, mode);
            return await _httpClient.GetFromJsonAsync<GameScore>(url);
        }

        public async Task<GameScore> GetPairUpAverageScoreAsync(string username, GameDifficulty? difficulty = null, GameMode? mode = null)
        {
            var url = ConstructUrl("average-score/pairup", username, difficulty, mode);
            return await _httpClient.GetFromJsonAsync<GameScore>(url);
        }

        public async Task<GameScore> GetSudokuAverageScoreAsync(string username, GameDifficulty? difficulty = null, GameMode? mode = null)
        {
            var url = ConstructUrl("average-score/sudoku", username, difficulty, mode);
            return await _httpClient.GetFromJsonAsync<GameScore>(url);
        }

        public async Task<int> GetMathGameMatchesPlayedAsync(string username, GameDifficulty? difficulty = null, GameMode? mode = null)
        {
            var url = ConstructUrl("matches-played/math", username, difficulty, mode);
            return await _httpClient.GetFromJsonAsync<int>(url);
        }

        public async Task<int> GetAimTrainerMatchesPlayedAsync(string username, GameDifficulty? difficulty = null, GameMode? mode = null)
        {
            var url = ConstructUrl("matches-played/aim", username, difficulty, mode);
            return await _httpClient.GetFromJsonAsync<int>(url);
        }

        public async Task<int> GetPairUpMatchesPlayedAsync(string username, GameDifficulty? difficulty = null, GameMode? mode = null)
        {
            var url = ConstructUrl("matches-played/pairup", username, difficulty, mode);
            return await _httpClient.GetFromJsonAsync<int>(url);
        }

        public async Task<int> GetSudokuMatchesPlayedAsync(string username, GameDifficulty? difficulty = null, GameMode? mode = null)
        {
            var url = ConstructUrl("matches-played/sudoku", username, difficulty, mode);
            return await _httpClient.GetFromJsonAsync<int>(url);
        }

        public async Task<List<AverageScoreDto>> GetMathGameAverageScoreLast7DaysAsync(string username, GameDifficulty? difficulty = null, GameMode? mode = null)
        {
            var url = ConstructUrl("average-score-last-7days/math", username, difficulty, mode);
            return await _httpClient.GetFromJsonAsync<List<AverageScoreDto>>(url);
        }

        public async Task<List<AverageScoreDto>> GetAimTrainerAverageScoreLast7DaysAsync(string username, GameDifficulty? difficulty = null, GameMode? mode = null)
        {
            var url = ConstructUrl("average-score-last-7days/aim", username, difficulty, mode);
            return await _httpClient.GetFromJsonAsync<List<AverageScoreDto>>(url);
        }

        public async Task<List<AverageScoreDto>> GetPairUpAverageScoreLast7DaysAsync(string username, GameDifficulty? difficulty = null, GameMode? mode = null)
        {
            var url = ConstructUrl("average-score-last-7days/pairup", username, difficulty, mode);
            return await _httpClient.GetFromJsonAsync<List<AverageScoreDto>>(url);
        }

        public async Task<List<AverageScoreDto>> GetSudokuAverageScoreLast7DaysAsync(string username, GameDifficulty? difficulty = null, GameMode? mode = null)
        {
            var url = ConstructUrl("average-score-last-7days/sudoku", username, difficulty, mode);
            return await _httpClient.GetFromJsonAsync<List<AverageScoreDto>>(url);
        }
    }
}