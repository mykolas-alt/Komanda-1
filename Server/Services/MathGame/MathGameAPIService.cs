namespace Projektas.Server.Services.MathGame
{
    public class MathGameAPIService
    {
        private readonly MathQuestionService _mathQuestionService;

        public MathGameAPIService()
        {
            MathCalculationService mathCalculations = new MathCalculationService();
            _mathQuestionService = new MathQuestionService(mathCalculations);
        }

        public string GenerateQuestion(int score)
        {
            return _mathQuestionService.GenerateQuestion(score);
        }

        public bool CheckAnswer(int option)
        {
            return _mathQuestionService.Answer == option;
        }
        public List<int> GenerateOptions()
        {
            return _mathQuestionService.GenerateOptions();
        }

    }
}