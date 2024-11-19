namespace Projektas.Server.Interfaces.MathGame {
    public interface IMathGameService {
        public string GenerateQuestion(int score);
        public bool CheckAnswer(int option);
        public List<int> GenerateOptions();
    }
}
