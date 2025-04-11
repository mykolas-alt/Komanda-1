using System.Globalization;
using Projektas.Server.Enums;
using Projektas.Server.Interfaces.MathGame;
using Projektas.Shared.Enums;

namespace Projektas.Server.Services.MathGame {

    public class MathGenerationService : IMathGenerationService {
        private readonly Random _random = new();

        private static int MaxNumber(int score, GameDifficulty difficulty) {
            int number = 1;

            // increases the range of numbers as the score increases
            switch(difficulty) {
                case GameDifficulty.Easy:
                    number = 20;
                    break;
                case GameDifficulty.Normal:
                    number = 50;
                    break;
                case GameDifficulty.Hard:
                    number = 30 + score * 4;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(difficulty), difficulty, null);
            } 
            return number;
        } 
        private static int MinNumber(int score, GameDifficulty difficulty) {
            int number = 1;

            switch(difficulty) {
                case GameDifficulty.Easy:
                    number = 1;
                    break;
                case GameDifficulty.Normal:
                    number = 10;
                    break;
                case GameDifficulty.Hard:
                    number = 30 + score;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(difficulty), difficulty, null);
            }
            return number;
        }  

        // generates numbers and adds to the list
        public List<int> GenerateNumbers(int numberOfOperands, int score, GameDifficulty difficulty) {
            List<int> numbers = new();
            for(int i = 0; i < numberOfOperands; i++) {
                numbers.Add(GenerateNumber(score, difficulty));
            }
            return numbers;
        }

        // generates operations and adds to the list
        public List<Operation> GenerateOperations(int numberOfOperands, int score, GameDifficulty difficulty) {
            List<Operation> operations = new();
            Operation[] possibleOperations;

            switch (difficulty) {
                case GameDifficulty.Easy:
                    if (score <= 5) {
                        possibleOperations = new[] {Operation.Addition};
                    } else {
                        possibleOperations = new[] {Operation.Addition, Operation.Subtraction};
                    }
                    break;

                case GameDifficulty.Normal:
                    if (score <= 5) {
                        possibleOperations = new[] {Operation.Addition, Operation.Subtraction};
                    } else {
                        possibleOperations = new[] {Operation.Addition, Operation.Subtraction, Operation.Multiplication};
                    }
                    break;

                case GameDifficulty.Hard:
                    possibleOperations = new[] {Operation.Addition, Operation.Subtraction, Operation.Multiplication, Operation.Division};
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(difficulty), difficulty, null);
            }


            for(int i = 0; i < numberOfOperands - 1; i++) {
                operations.Add(possibleOperations[_random.Next(possibleOperations.Length)]);
            }

            return operations;
        }

        // adjusts numbers by operations
        public void AdjustNumbersForOperations(int score, List<int> numbers, List<Operation> operations) {
            for(int i = 1; i < numbers.Count; i++) {
                if(operations[i - 1] == Operation.Division && numbers[i - 1] % numbers[i] != 0) {
                    AdjustForDivision(i, numbers);
                } else if(operations[i - 1] == Operation.Multiplication) {
                    int limit = Math.Max(2, score / 2);
                    if(numbers[i] > limit){
                        numbers[i] = _random.Next(2, limit);
                    }
                }
            }
        }

        // selects a random divisor for the number
        private void AdjustForDivision(int index, List<int> numbers) {
            int previousNumber = numbers[index - 1];

            List<int> divisors = GetDivisors(previousNumber);

            if(divisors.Count == 0) {
                numbers[index] = 1;
                return;
            }

            numbers[index] = divisors[_random.Next(divisors.Count)];
        }

        private int GenerateNumber(int score, GameDifficulty difficulty) {
            int number = _random.Next(MinNumber(score, difficulty), MaxNumber(score, difficulty));

            return number;
        }

        private static List<int> GetDivisors(int number) {
            List<int> divisors = new();
            for(int i = 2; i <= number; i++) {
                if(number % i == 0) {
                    divisors.Add(i);
                }
            }
            return divisors;
        }
    }
}
