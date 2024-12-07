using Projektas.Shared.Enums;
using Projektas.Shared.Interfaces;

namespace Projektas.Shared.Models {
	public class Score<T> where T : IGame {
		public int Id {get; set;}
		public int UserId {get; set;}

		public T GameData {get; set;}

		public User User {get; set;}
	}
}
