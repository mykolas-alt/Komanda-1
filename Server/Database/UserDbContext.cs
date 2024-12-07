using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Projektas.Shared.Enums;
using Projektas.Shared.Interfaces;
using Projektas.Shared.Models;

namespace Projektas.Server.Database
{
    public class UserDbContext : DbContext {
		public UserDbContext(DbContextOptions<UserDbContext> options) : base(options) {}

		public DbSet<User> Users {get; set;}

		public DbSet<Score<MathGameData>> MathGameScores {get; set;}
		public DbSet<Score<SudokuData>> SudokuScores {get; set;}
		public DbSet<Score<AimTrainerData>> AimTrainerScores {get; set;}
		public DbSet<Score<PairUpData>> PairUpScores {get; set;}

		protected override void OnModelCreating(ModelBuilder modelBuilder) {
			var userConfig = modelBuilder.Entity<User>();
			userConfig.ToTable("users");
			userConfig.HasKey(c => c.Id);
			userConfig.Property(c => c.Name).HasColumnName("name").IsRequired();
			userConfig.Property(c => c.Surname).HasColumnName("surname").IsRequired();
			userConfig.Property(c => c.Username).HasColumnName("username").IsRequired();
			userConfig.Property(c => c.Password).HasColumnName("password").IsRequired();
			
			ConfigureScoreTable<AimTrainerData>(modelBuilder,"aimTrainerScores", score => {
				score.OwnsOne(s => s.GameData, gameData => {
					gameData.Property(g => g.Scores).HasColumnName("score");
					gameData.Property(g => g.Timestamp).HasColumnName("timestamp");
					gameData.Property(g => g.Difficulty)
						.HasColumnName("difficulty")
                        .HasConversion(new EnumToStringConverter<GameDifficulty>());
                });
				score.HasOne(s => s.User)
					.WithMany(u => u.AimTrainerScores)
					.HasForeignKey(s => s.UserId)
					.OnDelete(DeleteBehavior.Cascade);
			});
			
			ConfigureScoreTable<MathGameData>(modelBuilder,"mathGameScores", score => {
				score.OwnsOne(s => s.GameData, gameData => {
					gameData.Property(g => g.Scores).HasColumnName("score");
                    gameData.Property(g => g.Timestamp).HasColumnName("timestamp");
                });
				score.HasOne(s => s.User)
					.WithMany(u => u.MathGameScores)
					.HasForeignKey(s => s.UserId)
					.OnDelete(DeleteBehavior.Cascade);
			});
			
			ConfigureScoreTable<PairUpData>(modelBuilder,"pairUpScores", score => {
				score.OwnsOne(s => s.GameData, gameData => {
					gameData.Property(g => g.TimeInSeconds).HasColumnName("timeInSeconds");
					gameData.Property(g => g.Fails).HasColumnName("fails");
                    gameData.Property(g => g.Timestamp).HasColumnName("timestamp");
                    gameData.Property(g => g.Difficulty)
						.HasColumnName("difficulty")
						.HasConversion(new EnumToStringConverter<GameDifficulty>());
                });
				score.HasOne(s => s.User)
					.WithMany(u => u.PairUpScores)
					.HasForeignKey(s => s.UserId)
					.OnDelete(DeleteBehavior.Cascade);
			});
			
			ConfigureScoreTable<SudokuData>(modelBuilder,"sudokuScores", score => {
				score.OwnsOne(s => s.GameData, gameData => {
					gameData.Property(g => g.TimeInSeconds).HasColumnName("timeInSeconds");
                    gameData.Property(g => g.Timestamp).HasColumnName("timestamp");
                    gameData.Property(g => g.Difficulty)
						.HasColumnName("difficulty")
						.HasConversion(new EnumToStringConverter<GameDifficulty>());
                    gameData.Property(g => g.Mode)
						.HasColumnName("mode")
						.HasConversion(new EnumToStringConverter<GameMode>());
                });
				score.HasOne(s => s.User)
					.WithMany(u => u.SudokuScores)
					.HasForeignKey(s => s.UserId)
					.OnDelete(DeleteBehavior.Cascade);
			});
		}

		private void ConfigureScoreTable<T>(ModelBuilder modelBuilder, string tableName, Action<EntityTypeBuilder<Score<T>>> configureGameInfo) where T : IGame {
			var scoreConfig = modelBuilder.Entity<Score<T>>();
			scoreConfig.ToTable(tableName);
			scoreConfig.HasKey(s => s.Id);

			configureGameInfo(scoreConfig);
		}
	}
}
