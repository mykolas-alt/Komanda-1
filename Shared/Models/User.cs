namespace Projektas.Shared.Models
{
    public class User {
        public int Id {get;set;}

        public string Name {get;set;}="";
        public string Surname {get;set;}="";
        public string Username {get;set;}="";
        public string Password {get;set;}="";

        public List<Score<MathGameData>> MathGameScores {get;set;}=new List<Score<MathGameData>>();
        public List<Score<SudokuData>> SudokuScores {get;set;}=new List<Score<SudokuData>>();
        public List<Score<AimTrainerData>> AimTrainerScores {get;set;}=new List<Score<AimTrainerData>>();
        public List<Score<PairUpData>> PairUpScores {get;set;}=new List<Score<PairUpData>>();
    }
}
