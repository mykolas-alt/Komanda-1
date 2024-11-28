namespace Projektas.Shared.Models
{
    public class User {
        public int Id {get;set;}

        public string Name {get;set;}="";
        public string Surname {get;set;}="";
        public string Username {get;set;}="";
        public string Password {get;set;}="";

        public List<Score<MathGameModel>> MathGameScores {get;set;}=new List<Score<MathGameModel>>();
        public List<Score<SudokuModel>> SudokuScores {get;set;}=new List<Score<SudokuModel>>();
        public List<Score<AimTrainerModel>> AimTrainerScores {get;set;}=new List<Score<AimTrainerModel>>();
        public List<Score<PairUpModel>> PairUpScores {get;set;}=new List<Score<PairUpModel>>();
    }
}
