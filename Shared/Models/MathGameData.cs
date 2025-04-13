using Projektas.Shared.Enums;
using Projektas.Shared.Interfaces;

namespace Projektas.Shared.Models {
    public class MathGameData : IGame, IGameWithDifficulty {
		  public int Scores {get; set;}
      public GameDifficulty Difficulty {get; set;}
    }
}
