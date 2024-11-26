using Projektas.Server.Enums;
using Projektas.Server.Interfaces.MathGame;

namespace Projektas.Server.Services.MathGame {

    public class MathGenerationService : IMathGenerationService {
        private readonly Random _random=new();

        private static int MaxNumber(int score) => 10+score*2; // increases the range of numbers as the score increases
        private static int MinNumber(int score) => 1+score;

        // generates numbers and adds to the list
        public List<int> GenerateNumbers(int numberOfOperands,int score) {
            List<int> numbers=new();
            for(int i=0;i<numberOfOperands;i++) {
                numbers.Add(GenerateNumber(score));
            }
            return numbers;
        }

        // generates operations and adds to the list
        public List<Operation> GenerateOperations(int numberOfOperands,int score) {
            List<Operation> operations=new();
            Operation[] possibleOperations;

            if(score<=5) {
                possibleOperations=new[] {Operation.Addition,Operation.Subtraction};
            } else {
                possibleOperations=new[] {Operation.Addition,Operation.Subtraction,Operation.Multiplication,Operation.Division};
            }

            for(int i=0;i<numberOfOperands-1;i++) {
                operations.Add(possibleOperations[_random.Next(possibleOperations.Length)]);
            }

            return operations;
        }

        // adjusts numbers by operations
        public void AdjustNumbersForOperations(int score,List<int> numbers,List<Operation> operations) {
            for(int i=1;i<numbers.Count;i++) {
                if(operations[i-1]==Operation.Division && numbers[i-1]%numbers[i]!=0) {
                    AdjustForDivision(i,numbers);
                } else if(operations[i-1]==Operation.Multiplication) {
                    int limit=Math.Max(2,score/2);
                    if(numbers[i]>limit){
                        numbers[i]=_random.Next(2,limit);
                    }
                }
            }
        }

        // selects a random divisor for the number
        private void AdjustForDivision(int index,List<int> numbers) {
            int previousNumber=numbers[index-1];

            List<int> divisors=GetDivisors(previousNumber);

            if(divisors.Count==0) {
                numbers[index]=1;
                return;
            }

            numbers[index]=divisors[_random.Next(divisors.Count)];
        }

        private int GenerateNumber(int score) {
            int number=_random.Next(MinNumber(score),MaxNumber(score));

            return number;
        }

        private static List<int> GetDivisors(int number) {
            List<int> divisors=new();
            for (int i=2;i<=number;i++) {
                if (number%i==0) {
                    divisors.Add(i);
                }
            }
            return divisors;
        }
    }
}
