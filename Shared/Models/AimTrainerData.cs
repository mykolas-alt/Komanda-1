using Projektas.Shared.Enums;
using Projektas.Shared.Interfaces;

namespace Projektas.Shared.Models {
    public class AimTrainerData : IGame {
        public int Scores {get; set;}
        public GameDifficulty Difficulty {get; set;}
    }
}
