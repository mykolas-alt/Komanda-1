using Projektas.Server.Enums;
using Projektas.Server.Interfaces.MathGame;

namespace Projektas.Server.Services.MathGame {
    public class MathCalculationService : IMathCalculationService {
        public int CalculateAnswer(List<int> numbers,List<Operation> operations) {
            // handles multiplication and division
            List<int> processedNumbers=new(numbers);
            List<Operation> processedOperations=new(operations);

            HandleMultiplicationAndDivision(processedNumbers,processedOperations);

            // handles addition and subtraction
            int finalResult=HandleAdditionAndSubtraction(processedNumbers,processedOperations);

            return finalResult;
        }

        // handles multiplication and division
        private static void HandleMultiplicationAndDivision(List<int> processedNumbers,List<Operation> processedOperations) {
            for(int i=0;i<processedOperations.Count;i++) {
                if(processedOperations[i]==Operation.Multiplication || processedOperations[i]==Operation.Division) {
                    int left=processedNumbers[i];
                    int right=processedNumbers[i+1];
                    int result=PerformOperation(left,right,processedOperations[i]);

                    // replaces the left number with the result and removes the right number and the operation
                    processedNumbers[i]=result;
                    processedNumbers.RemoveAt(i+1);
                    processedOperations.RemoveAt(i);
                    i--; // adjust index
                }
            }
        }

        // handles addition and subtraction
        private static int HandleAdditionAndSubtraction(List<int> processedNumbers,List<Operation> processedOperations) {
            int result=processedNumbers[0];
            for(int i=0;i<processedOperations.Count;i++) {
                int right=processedNumbers[i+1];
                result=PerformOperation(left: result,right,processedOperations[i]);
            }
            return result;
        }

        // performs calculations by operation
        private static int PerformOperation(int left,int right,Operation operation) {
            switch(operation) {
                case Operation.Multiplication:
                    return left*right;
                case Operation.Division:
                    return left/right;
                case Operation.Addition:
                    return left+right;
                case Operation.Subtraction:
                    return left-right;
                default:
                    return 0;
            }
        }
    }
}
