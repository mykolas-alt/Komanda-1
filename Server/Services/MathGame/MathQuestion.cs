using Projektas.Server.Enums;
using Projektas.Server.Extensions;
using System.Text;

namespace Projektas.Server.Services.MathGame
{
    public class MathQuestion
    {
        private readonly Random _random = new();
        private readonly MathGameService _mathGameService;
        private readonly MathCalculations _mathCalculations;
        public int Answer { get; set; }
        public List<int> numbers = new();
        public List<Operation> operations = new();

        public MathQuestion(MathGameService mathGameService, MathCalculations mathCalculations)
        {
            _mathGameService = mathGameService;
            _mathCalculations = mathCalculations;
        }

        private int MaxNumber => 10 + _mathGameService.Score * 2; // increases the range of numbers as the score increases
        private int MinNumber => 1 + _mathGameService.Score;

        public string GenerateQuestion()
        {
            int minOperands = (int)(2 + _mathGameService.Score * 0.1);
            int maxOperands = (int)(3 + _mathGameService.Score * 0.1); // increases the range of operands as the score inscreases
            int numberOfOperands = _random.Next(minOperands, maxOperands);

            // generate numbers and operations
            numbers = GenerateNumbers(numberOfOperands);
            operations = GenerateOperations(numberOfOperands);

            // adjust numbers by operation (division or multiplication)
            AdjustNumbersForOperations();

            Answer = CalculateAnswer();

            return BuildQuestion();
        }

        // generates numbers and adds to the list
        private List<int> GenerateNumbers(int numberOfOperands)
        {
            List<int> numbers = new();
            for (int i = 0; i < numberOfOperands; i++)
            {
                numbers.Add(GenerateNumber());
            }
            return numbers;
        }

        // generates operations and adds to the list
        private List<Operation> GenerateOperations(int numberOfOperands)
        {
            List<Operation> operations = new();
            Operation[] possibleOperations;

            if (_mathGameService.Score <= 5)
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
        private void AdjustNumbersForOperations()
        {
            for (int i = 1; i < numbers.Count; i++)
            {
                if (operations[i - 1] == Operation.Division)
                {
                    AdjustForDivision(i);
                }
                else if (operations[i - 1] == Operation.Multiplication)
                {
                    int limit = Math.Max(2, _mathGameService.Score / 2);
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
            Answer = _mathCalculations.CalculateAnswer(numbers, operations);
            return Answer;
        }

        private int GenerateNumber()
        {
            int number = _random.Next(MinNumber, MaxNumber);

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
