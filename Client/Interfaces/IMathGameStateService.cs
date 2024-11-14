using Projektas.Shared.Models;

namespace Projektas.Client.Interfaces
{
    public interface IMathGameStateService
    {
        public Task<GameState> GetGameState();
        public Task UpdateGameState(GameState gameState);
        public Task IncrementScore(GameState gameState);
    }
}
