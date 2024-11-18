namespace Projektas.Shared.Models
{
    public class User {
        public int Id {get;set;}

        public string Name {get;set;}="";
        public string Surname {get;set;}="";
        public string Username {get;set;}="";
        public string Password {get;set;}="";

        public List<Score<MathGameM>> MathGameScores {get;set;}=new List<Score<MathGameM>>();
        public List<Score<SudokuM>> SudokuScores {get;set;}=new List<Score<SudokuM>>();
        public List<Score<AimTrainerM>> AimTrainerScores {get;set;}=new List<Score<AimTrainerM>>();
        public List<Score<PairUpM>> PairUpScores {get;set;}=new List<Score<PairUpM>>();
    }
}
