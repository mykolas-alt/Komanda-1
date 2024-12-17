using Projektas.Shared.Enums;
using Projektas.Shared.Models;
using System.Net.Http.Json;
using Projektas.Client.Interfaces;

namespace Projektas.Client.Services {
    public class AimTrainerService : IAimTrainerService {
        private readonly HttpClient _httpClient;

        public AimTrainerService(HttpClient httpClient) {
            _httpClient = httpClient;
        }

        public async Task SaveScoreAsync(string username, int score, GameDifficulty difficulty) {
            var data = new UserScoreDto<AimTrainerData> {
                Username = username,
                Timestamp = DateTime.UtcNow.ToLocalTime(),
                GameData = new AimTrainerData {
                    Scores = score,
                    Difficulty = difficulty
                }
            };
            await _httpClient.PostAsJsonAsync("api/aimtrainer/save-score", data);
        }

        public async Task<UserScoreDto<AimTrainerData>> GetUserHighscoreAsync(string username, GameDifficulty difficulty) {
            var url = $"api/aimtrainer/highscore?username={username}&difficulty={difficulty}";
            return await _httpClient.GetFromJsonAsync<UserScoreDto<AimTrainerData>>(url);
        }

        public async Task<List<UserScoreDto<AimTrainerData>>> GetTopScoresAsync(GameDifficulty difficulty, int topCount = 10)  {
            var url = $"api/aimtrainer/top-score?topCount={topCount}&difficulty={difficulty}";
            return await _httpClient.GetFromJsonAsync<List<UserScoreDto<AimTrainerData>>>(url);
        }
    }
}
