namespace Projektas.Shared.Models {
    public class User {
        public int Id {get;set;}

        public string Name {get;set;}="";
        public string Surname {get;set;}="";
        public string Username {get;set;}="";
        public string Password {get;set;}="";

        public List<MathGameScore> MathGameScores {get;set;}=new List<MathGameScore>();
        public List<SudokuScore> SudokuScores {get;set;}=new List<SudokuScore>();
        public List<AimTrainerScore> AimTrainerScores {get;set;}=new List<AimTrainerScore>();
        public List<PairUpScore> PairUpScores {get;set;}=new List<PairUpScore>();
    }
}
