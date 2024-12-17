using Projektas.Shared.Enums;
using Projektas.Shared.Models;
using System.Net.Http.Json;
using Projektas.Client.Interfaces;

namespace Projektas.Client.Services {
    public class PairUpService : IPairUpService {
        private readonly HttpClient _httpClient;

        public PairUpService(HttpClient httpClient) {
            _httpClient = httpClient;
        }

        public async Task SaveScoreAsync(string username, int score, int fails, GameDifficulty difficulty) {
            var data = new UserScoreDto<PairUpData> {
                Username = username,
                Timestamp = DateTime.UtcNow.ToLocalTime(),
                GameData = new PairUpData {
                    TimeInSeconds = score,
                    Fails = fails,
                    Difficulty = difficulty
                }
            };
            await _httpClient.PostAsJsonAsync("api/pairup/save-score", data);
        }

        public async Task<UserScoreDto<PairUpData>> GetUserHighscoreAsync(string username, GameDifficulty difficulty) {
            var url = $"api/pairup/highscore?username={username}&difficulty={difficulty}";
            return await _httpClient.GetFromJsonAsync<UserScoreDto<PairUpData>>(url);
        }

        public async Task<List<UserScoreDto<PairUpData>>> GetTopScoresAsync(GameDifficulty difficulty, int topCount = 10)  {
            var url = $"api/pairup/top-score?topCount={topCount}&difficulty={difficulty}";
            return await _httpClient.GetFromJsonAsync<List<UserScoreDto<PairUpData>>>(url);
        }
    }
}
