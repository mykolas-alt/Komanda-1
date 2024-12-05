using Projektas.Server.Enums;

namespace Projektas.Server.Interfaces.MathGame {
    public interface IMathCalculationService {
        int CalculateAnswer(List<int> numbers, List<Operation> operations);
    }
}
