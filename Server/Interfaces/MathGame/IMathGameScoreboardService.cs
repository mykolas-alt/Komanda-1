namespace Projektas.Server.Interfaces.MathGame
{
    public interface IMathGameScoreboardService
    {
        public List<int> GetTopScores(int topCount);
    }
}
