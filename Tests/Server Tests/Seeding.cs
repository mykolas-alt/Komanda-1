using Projektas.Server.Database;
using Projektas.Shared.Models;
using Projektas.Shared.Enums;

namespace Projektas.Tests.Server_Tests {
	public class Seeding {
		public static void InitializeTestDB(UserDbContext db) {
			db.Database.EnsureDeleted();
			db.Database.EnsureCreated();

			db.Users.RemoveRange(db.Users);
			db.SaveChanges();

			var users = GetUsers();
			db.Users.AddRange(users);
			db.SaveChanges();

			var mathGameScores = GetMathGameScores(users);
            var aimTrainerScores = GetAimTrainerScores(users);
            var pairUpScores = GetPairUpScores(users);
            var sudokuScores = GetSudokuScores(users);
			db.MathGameScores.AddRange(mathGameScores);
            db.AimTrainerScores.AddRange(aimTrainerScores);
            db.PairUpScores.AddRange(pairUpScores);
            db.SudokuScores.AddRange(sudokuScores);
			db.SaveChanges();
		}

		private static List<User> GetUsers() {
			return new List<User>() {
				new User() {Id = 1, Name = "John", Surname = "Doe", Username = "johndoe", Password = "password123"},
				new User() {Id = 2, Name = "Jane", Surname = "Doe", Username = "janedoe", Password = "password456"},
				new User() {Id = 3, Name = "Jake", Surname = "Doe", Username = "jakedoe", Password = "password789"}
			};

		}

        private static List<Score<MathGameData>> GetMathGameScores(List<User> users)
        {
                    return new List<Score<MathGameData>>() {
                new Score<MathGameData>() {Id=1,UserId=users[0].Id,GameData=new MathGameData{Scores=12},User=users[0]},
                new Score<MathGameData>() {Id=2,UserId=users[1].Id,GameData=new MathGameData{Scores=15},User=users[1]},
                new Score<MathGameData>() {Id=3,UserId=users[2].Id,GameData=new MathGameData{Scores=20},User=users[2]},
                new Score<MathGameData>() {Id=4,UserId=users[0].Id,GameData=new MathGameData{Scores=18},User=users[0]},
                new Score<MathGameData>() {Id=5,UserId=users[1].Id,GameData=new MathGameData{Scores=22},User=users[1]},
                new Score<MathGameData>() {Id=6,UserId=users[2].Id,GameData=new MathGameData{Scores=25},User=users[2]}
            };
        }

        private static List<Score<AimTrainerData>> GetAimTrainerScores(List<User> users)
        {
                    return new List<Score<AimTrainerData>>() {
                new Score<AimTrainerData>() {Id=1,UserId=users[0].Id,GameData=new AimTrainerData{Scores=50, Difficulty = GameDifficulty.Normal},User=users[0]},
                new Score<AimTrainerData>() {Id=2,UserId=users[1].Id,GameData=new AimTrainerData{Scores=29, Difficulty = GameDifficulty.Hard},User=users[1]},
                new Score<AimTrainerData>() {Id=3,UserId=users[2].Id,GameData=new AimTrainerData{Scores=34, Difficulty = GameDifficulty.Normal},User=users[2]},
                new Score<AimTrainerData>() {Id=4,UserId=users[0].Id,GameData=new AimTrainerData{Scores=60, Difficulty = GameDifficulty.Hard},User=users[0]},
                new Score<AimTrainerData>() {Id=5,UserId=users[1].Id,GameData=new AimTrainerData{Scores=35, Difficulty = GameDifficulty.Normal},User=users[1]},
                new Score<AimTrainerData>() {Id=6,UserId=users[2].Id,GameData=new AimTrainerData{Scores=40, Difficulty = GameDifficulty.Hard},User=users[2]}
            };
        }

        private static List<Score<PairUpData>> GetPairUpScores(List<User> users)
        {
                    return new List<Score<PairUpData>>() {
                new Score<PairUpData>() {Id=1,UserId=users[0].Id,GameData=new PairUpData{TimeInSeconds=70, Difficulty = GameDifficulty.Medium},User=users[0]},
                new Score<PairUpData>() {Id=2,UserId=users[1].Id,GameData=new PairUpData{TimeInSeconds=49, Difficulty = GameDifficulty.Hard},User=users[1]},
                new Score<PairUpData>() {Id=3,UserId=users[2].Id,GameData=new PairUpData{TimeInSeconds=28, Difficulty = GameDifficulty.Easy},User=users[2]},
                new Score<PairUpData>() {Id=4,UserId=users[0].Id,GameData=new PairUpData{TimeInSeconds=65, Difficulty = GameDifficulty.Hard},User=users[0]},
                new Score<PairUpData>() {Id=5,UserId=users[1].Id,GameData=new PairUpData{TimeInSeconds=55, Difficulty = GameDifficulty.Medium},User=users[1]},
                new Score<PairUpData>() {Id=6,UserId=users[2].Id,GameData=new PairUpData{TimeInSeconds=30, Difficulty = GameDifficulty.Hard},User=users[2]}
            };
        }

        private static List<Score<SudokuData>> GetSudokuScores(List<User> users)
        {
                    return new List<Score<SudokuData>>() {
                new Score<SudokuData>() {Id=1,UserId=users[0].Id,GameData=new SudokuData{TimeInSeconds=70, Difficulty = GameDifficulty.Medium, Mode = GameMode.SixteenBySixteen},User=users[0]},
                new Score<SudokuData>() {Id=2,UserId=users[1].Id,GameData=new SudokuData{TimeInSeconds=49, Difficulty = GameDifficulty.Hard, Mode = GameMode.NineByNine},User=users[1]},
                new Score<SudokuData>() {Id=3,UserId=users[2].Id,GameData=new SudokuData{TimeInSeconds=28, Difficulty = GameDifficulty.Easy, Mode = GameMode.FourByFour},User=users[2]},
                new Score<SudokuData>() {Id=4,UserId=users[0].Id,GameData=new SudokuData{TimeInSeconds=60, Difficulty = GameDifficulty.Hard, Mode = GameMode.NineByNine},User=users[0]},
                new Score<SudokuData>() {Id=5,UserId=users[1].Id,GameData=new SudokuData{TimeInSeconds=55, Difficulty = GameDifficulty.Medium, Mode = GameMode.SixteenBySixteen},User=users[1]},
                new Score<SudokuData>() {Id=6,UserId=users[2].Id,GameData=new SudokuData{TimeInSeconds=30, Difficulty = GameDifficulty.Hard, Mode = GameMode.NineByNine},User=users[2]}
            };
        }
    }
}
