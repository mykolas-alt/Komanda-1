using Projektas.Shared.Enums;

namespace Projektas.Server.Interfaces.MathGame {
    public interface IMathGameService {
        public string GenerateQuestion(int score, GameDifficulty difficulty);
        public bool CheckAnswer(int option);
        public List<int> GenerateOptions();
    }
}
