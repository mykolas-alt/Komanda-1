namespace Projektas.Shared.Models {
	public class Score {
		public int Id {get;set;}

		public int UserScores {get;set;}
		public int UserId {get;set;}

		public User User {get;set;}
	}

	public class MathGameScore : Score { }
    public class SudokuScore : Score { }
    public class AimTrainerScore : Score { }
    public class PairUpScore : Score { }
}
