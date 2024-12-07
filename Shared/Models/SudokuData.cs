using Projektas.Shared.Enums;
using Projektas.Shared.Interfaces;

namespace Projektas.Shared.Models {
    public class SudokuData : IGame, IGameWithDifficulty {
        public int TimeInSeconds { get; set; }
        public GameDifficulty Difficulty {get; set;}
        public GameMode Mode {get; set;}
    }
}
