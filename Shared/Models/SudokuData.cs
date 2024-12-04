using Projektas.Shared.Interfaces;

namespace Projektas.Shared.Models {
    public class SudokuData : IGame {
		public int TimeInSeconds {get;set;}
        public bool Solved {get;set;}
    }
}
