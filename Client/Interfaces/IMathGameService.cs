namespace Projektas.Client.Interfaces
{
    public interface IMathGameService
    {
        public Task<string> GetQuestionAsync(int score);
        public Task<List<int>> GetOptionsAsync();
        public Task<bool> CheckAnswerAsync(int option);
        public Task SaveDataAsync(int score);
        public Task<List<int>> GetTopScoresAsync(int topCount);
    }
}
