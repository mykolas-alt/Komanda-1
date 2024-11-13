﻿using Projektas.Server.Enums;
using System.Text;
using Projektas.Server.Extensions;

namespace Projektas.Server.Services.MathGame {
    public class MathGameService {
        private readonly Random _random=new();
        private readonly MathCalculationService _mathCalculationService;
        private readonly MathGenerationService _mathGenerationService;

        public int Answer {get;set;}
        public List<int> numbers=new();
        public List<Operation> operations=new();

        public MathGameService(MathCalculationService mathCalculationService, MathGenerationService mathGenerationService) {
            _mathCalculationService=mathCalculationService;
            _mathGenerationService=mathGenerationService;
        }

        public string GenerateQuestion(int score) {
            int minOperands=(int)(2+score*0.1);
            int maxOperands=(int)(3+score*0.1); // increases the range of operands as the score inscreases
            int numberOfOperands=_random.Next(minOperands,maxOperands);

            // generate numbers and operations
            numbers=_mathGenerationService.GenerateNumbers(numberOfOperands,score);
            operations=_mathGenerationService.GenerateOperations(numberOfOperands,score);

            // adjust numbers by operation (division or multiplication)
            _mathGenerationService.AdjustNumbersForOperations(score,numbers,operations);

            Answer=_mathCalculationService.CalculateAnswer(numbers,operations);

            return BuildQuestionToString();
        }

        public bool CheckAnswer(int option) {
            return Answer==option;
        }
        public List<int> GenerateOptions() {
            int limit=Math.Max(2,Answer/10); // adjusts the limit to be a smaller range
            HashSet<int> options=new();
            options.Add(Answer);
            while (options.Count<4) {
                int option=_random.Next(Answer-limit,Answer+limit+1);
                options.Add(option);
            }
            return options.OrderBy(x => Guid.NewGuid()).ToList(); // shuffles options
        }

        private string BuildQuestionToString() {
            StringBuilder questionBuilder=new();
            questionBuilder.Append(numbers[0]);
            for (int i=0;i<operations.Count;i++) {
                questionBuilder.Append($" {operations[i].GetOperationSymbol()} {numbers[i+1]}");
            }
            return questionBuilder.ToString();
        }
    }
}