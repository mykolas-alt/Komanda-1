using Projektas.Shared.Interfaces;

namespace Projektas.Shared.Models {
    public class SudokuModel : IGame {
		public int UserTimeInSeconds {get;set;}
        public bool Solved {get;set;}
    }
}
