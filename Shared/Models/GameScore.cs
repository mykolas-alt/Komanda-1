namespace Projektas.Shared.Models
{
    public class GameScore
    {
        public int Scores { get; set; }
        public int? TimeSpent { get; set; }
        public string GetFormattedTimeSpent()
        {
            if (TimeSpent.HasValue)
            {
                int minutes = TimeSpent.Value / 60;
                int seconds = TimeSpent.Value % 60;
                return $"{minutes:D2}:{seconds:D2}";
            }
            return "00:00";
        }
    }
}
