namespace Projektas.Server.Services.MathGame
{
    public class MathGameScoreboardService : IComparer<int>
    {
        private readonly MathGameDataService _dataService;

        public MathGameScoreboardService(MathGameDataService dataService)
        {
            _dataService = dataService;
        }

        public List<int> GetTopScores(int topCount)
        {
            List<int> scores = _dataService.LoadData();

            scores.Sort(this);

            List<int> topScores = new List<int>();

            for (int i = 0; i < topCount && i < scores.Count; i++)
            {
                topScores.Add(scores[i]);
            }

            return topScores;
        }

        public int Compare(int a, int b)
        {
            return b.CompareTo(a);
        }
    }
}
