using Projektas.Shared.Interfaces;

namespace Projektas.Shared.Models {
    public class PairUpModel : IGame {
        public int UserTimeInSeconds {get;set;}
        public int Fails {get;set;}
    }
}
