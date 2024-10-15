using Projektas.Server.Enums;
using Projektas.Server.Extensions;
using System.Text;

namespace Projektas.Server.Services.MathGame
{
    public class MathQuestionService
    {
        private readonly Random _random = new();
        private readonly MathCalculationService _mathCalculationsService;
        public int Answer { get; set; }
        public List<int> numbers = new();
        public List<Operation> operations = new();

        public MathQuestionService(MathCalculationService mathCalculationsService)
        {
            _mathCalculationsService = mathCalculationsService;
        }

        private int MaxNumber(int score) => 10 + score * 2; // increases the range of numbers as the score increases
        private int MinNumber(int score) => 1 + score;

        public string GenerateQuestion(int score)
        {
            int minOperands = (int)(2 + score * 0.1);
            int maxOperands = (int)(3 + score * 0.1); // increases the range of operands as the score inscreases
            int numberOfOperands = _random.Next(minOperands, maxOperands);

            // generate numbers and operations
            numbers = GenerateNumbers(numberOfOperands, score);
            operations = GenerateOperations(numberOfOperands, score);

            // adjust numbers by operation (division or multiplication)
            AdjustNumbersForOperations(score);

            Answer = CalculateAnswer();

            return BuildQuestion();
        }

        // generates numbers and adds to the list
        private List<int> GenerateNumbers(int numberOfOperands, int score)
        {
            List<int> numbers = new();
            for (int i = 0; i < numberOfOperands; i++)
            {
                numbers.Add(GenerateNumber(score));
            }
            return numbers;
        }

        // generates operations and adds to the list
        private List<Operation> GenerateOperations(int numberOfOperands, int score)
        {
            List<Operation> operations = new();
            Operation[] possibleOperations;

            if (score <= 5)
            {
                possibleOperations = new[] { Operation.Addition, Operation.Subtraction };
            }
            else
            {
                possibleOperations = new[] { Operation.Addition, Operation.Subtraction, Operation.Multiplication, Operation.Division };
            }

            for (int i = 0; i < numberOfOperands - 1; i++)
            {
                operations.Add(possibleOperations[_random.Next(possibleOperations.Length)]);
            }

            return operations;
        }

        // adjusts numbers by operations
        private void AdjustNumbersForOperations(int score)
        {
            for (int i = 1; i < numbers.Count; i++)
            {
                if (operations[i - 1] == Operation.Division)
                {
                    AdjustForDivision(i);
                }
                else if (operations[i - 1] == Operation.Multiplication)
                {
                    int limit = Math.Max(2, score / 2);
                    numbers[i] = _random.Next(2, limit);

                }
            }
        }

        // selects a random divisor for the number
        private void AdjustForDivision(int index)
        {
            int previousNumber = numbers[index - 1];

            List<int> divisors = GetDivisors(previousNumber);

            if (divisors.Count == 0)
            {
                numbers[index] = 1;
                return;
            }

            numbers[index] = divisors[_random.Next(divisors.Count)];
        }

        private string BuildQuestion()
        {
            StringBuilder questionBuilder = new();
            questionBuilder.Append(numbers[0]);
            for (int i = 0; i < operations.Count; i++)
            {
                questionBuilder.Append($" {operations[i].GetOperationSymbol()} {numbers[i + 1]}");
            }
            return questionBuilder.ToString();
        }

        public List<int> GenerateOptions()
        {
            int limit = Math.Max(2, Answer / 10); // adjusts the limit to be a smaller range
            HashSet<int> options = new();
            options.Add(Answer);
            while (options.Count < 4)
            {
                int option = _random.Next(Answer - limit, Answer + limit + 1);
                options.Add(option);
            }
            return options.OrderBy(x => Guid.NewGuid()).ToList(); // shuffles options
        }

        private int CalculateAnswer()
        {
            Answer = _mathCalculationsService.CalculateAnswer(numbers, operations);
            return Answer;
        }

        private int GenerateNumber(int score)
        {
            int number = _random.Next(MinNumber(score), MaxNumber(score));

            return number;
        }

        private static List<int> GetDivisors(int number)
        {
            List<int> divisors = new();
            for (int i = 2; i <= number; i++)
            {
                if (number % i == 0)
                {
                    divisors.Add(i);
                }
            }
            return divisors;
        }
    }
}
