using Projektas.Shared.Interfaces;

namespace Projektas.Shared.Models {
    public class MathGameData : IGame {
		public int Scores {get; set;}
        public DateTime Timestamp {get; set;}
    }
}
