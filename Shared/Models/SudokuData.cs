using Projektas.Shared.Enums;
using Projektas.Shared.Interfaces;

namespace Projektas.Shared.Models {
    public class SudokuData : IGame, IGameWithDifficulty, IGameWithModes {
        public int TimeInSeconds { get; set; }
        public GameDifficulty Difficulty {get; set;}
        public GameMode Mode {get; set;}
        public string GetFormattedTimeSpent()
        {
            int minutes = TimeInSeconds / 60;
            int seconds = TimeInSeconds % 60;
            return $"{minutes:D2}:{seconds:D2}";
        }
    }
}
