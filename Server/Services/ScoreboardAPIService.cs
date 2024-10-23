namespace Projektas.Server.Services
{
    public class ScoreboardAPIService
    {
        public List<int> GetTopScores(List<int> scores, int topCount)
        {
            scores.Sort(CompareScoresDescending);

            List<int> topScores = new List<int>();

            for (int i = 0; i < topCount && i < scores.Count; i++)
            {
                topScores.Add(scores[i]);
            }

            return topScores;
        }

        private static int CompareScoresDescending(int a, int b)
        {
            return b.CompareTo(a);
        }
    }
}
