using Projektas.Server.Enums;
using Projektas.Shared.Enums;

namespace Projektas.Server.Interfaces.MathGame {
    public interface IMathGenerationService {
        List<int> GenerateNumbers(int numberOfOperands, int score, GameDifficulty difficulty);
        List<Operation> GenerateOperations(int numberOfOperands, int score, GameDifficulty difficulty);
        void AdjustNumbersForOperations(int score, List<int> numbers, List<Operation> operations);
    }
}
