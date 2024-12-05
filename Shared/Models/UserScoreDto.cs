using Projektas.Shared.Interfaces;

namespace Projektas.Shared.Models {
    public class UserScoreDto<T> where T : IGame {
        public string Username {get; set;} = "";
        public T GameData {get; set;}
    }
}
