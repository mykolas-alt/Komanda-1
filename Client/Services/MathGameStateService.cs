using Blazored.LocalStorage;
using Projektas.Shared.Models;

namespace Projektas.Client.Services
{
    public class MathGameStateService
    {
        private readonly ILocalStorageService _localStorage;

        public MathGameStateService(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        // saves data in browser's local storage
        public async Task UpdateGameState(GameState gameState)
        {
            await _localStorage.SetItemAsync("Score", gameState.Score);
            await _localStorage.SetItemAsync("Highscore", gameState.Highscore);
        }

        // gets game state data from browser's local storage
        public async Task<GameState> GetGameState()
        {
            int score = 0;
            int highscore = 0;

            if (await _localStorage.ContainKeyAsync("Score"))
            {
                score = await _localStorage.GetItemAsync<int>("Score");
            }

            if (await _localStorage.ContainKeyAsync("Highscore"))
            {
                highscore = await _localStorage.GetItemAsync<int>("Highscore");
            }

            return new GameState
            {
                Score = score,
                Highscore = highscore
            };
        }

        public async Task IncrementScore(GameState gameState)
        {
            gameState.Score++;
            if (gameState.Score > gameState.Highscore)
            {
                gameState.Highscore = gameState.Score;
            }
            await UpdateGameState(gameState);
        }
    }
}