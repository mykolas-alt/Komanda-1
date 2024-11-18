using Projektas.Server.Enums;

namespace Projektas.Server.Interfaces.MathGame
{
    public interface IMathGenerationService
    {
        List<int> GenerateNumbers(int numberOfOperands, int score);
        List<Operation> GenerateOperations(int numberOfOperands, int score);
        void AdjustNumbersForOperations(int score, List<int> numbers, List<Operation> operations);
    }
}
