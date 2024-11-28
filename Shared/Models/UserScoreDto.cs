namespace Projektas.Shared.Models {
    public class UserScoreDto {
        public string Username {get;set;}="";
        public int Score {get;set;}
        public DateTime Timestamp {get;set;}
        public string? Difficulty {get;set;}
        public string showOtherDateTimeFormat()
        {
            return Timestamp.ToString("yyyy-MM-dd HH:mm:ss");
        }
    }
}
