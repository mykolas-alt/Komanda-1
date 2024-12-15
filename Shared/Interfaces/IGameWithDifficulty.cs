using Projektas.Shared.Enums;

namespace Projektas.Shared.Interfaces {
    public interface IGameWithDifficulty {
        public GameDifficulty Difficulty {get; set;}
    }
}