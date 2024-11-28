using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Projektas.Shared.Interfaces;
using Projektas.Shared.Models;

namespace Projektas.Server.Database
{
    public class UserDbContext : DbContext {
		public UserDbContext(DbContextOptions<UserDbContext> options) : base(options) {}

		public DbSet<User> Users {get;set;}

		public DbSet<Score<MathGameModel>> MathGameScores {get;set;}
		public DbSet<Score<SudokuModel>> SudokuScores {get;set;}
		public DbSet<Score<AimTrainerModel>> AimTrainerScores {get;set;}
		public DbSet<Score<PairUpModel>> PairUpScores {get;set;}

		protected override void OnModelCreating(ModelBuilder modelBuilder) {
			var userConfig=modelBuilder.Entity<User>();
			userConfig.ToTable("users");
			userConfig.HasKey(c => c.Id);
			userConfig.Property(c => c.Name).HasColumnName("name").IsRequired();
			userConfig.Property(c => c.Surname).HasColumnName("surname").IsRequired();
			userConfig.Property(c => c.Username).HasColumnName("username").IsRequired();
			userConfig.Property(c => c.Password).HasColumnName("password").IsRequired();
			
			ConfigureScoreTable<AimTrainerModel>(modelBuilder,"aimTrainerScores",score => {
				score.OwnsOne(s => s.GameInfo,gameInfo => {
					gameInfo.Property(g => g.UserScores).HasColumnName("userScore");
				});
				score.HasOne(s => s.User)
					.WithMany(u => u.AimTrainerScores)
					.HasForeignKey(s => s.UserId)
					.OnDelete(DeleteBehavior.Cascade);
			});
			
			ConfigureScoreTable<MathGameModel>(modelBuilder,"mathGameScores",score => {
				score.OwnsOne(s => s.GameInfo,gameInfo => {
					gameInfo.Property(g => g.UserScores).HasColumnName("userScore");
				});
				score.HasOne(s => s.User)
					.WithMany(u => u.MathGameScores)
					.HasForeignKey(s => s.UserId)
					.OnDelete(DeleteBehavior.Cascade);
			});
			
			ConfigureScoreTable<PairUpModel>(modelBuilder,"pairUpScores",score => {
				score.OwnsOne(s => s.GameInfo,gameInfo => {
					gameInfo.Property(g => g.UserTimeInSeconds).HasColumnName("userTimeInSeconds");
					gameInfo.Property(g => g.Fails).HasColumnName("fails");
				});
				score.HasOne(s => s.User)
					.WithMany(u => u.PairUpScores)
					.HasForeignKey(s => s.UserId)
					.OnDelete(DeleteBehavior.Cascade);
			});
			
			ConfigureScoreTable<SudokuModel>(modelBuilder,"sudokuScores",score => {
				score.OwnsOne(s => s.GameInfo,gameInfo => {
					gameInfo.Property(g => g.UserTimeInSeconds).HasColumnName("userTimeInSeconds");
					gameInfo.Property(g => g.Solved).HasColumnName("solved");
				});
				score.HasOne(s => s.User)
					.WithMany(u => u.SudokuScores)
					.HasForeignKey(s => s.UserId)
					.OnDelete(DeleteBehavior.Cascade);
			});
		}

		private void ConfigureScoreTable<T>(ModelBuilder modelBuilder,string tableName,Action<EntityTypeBuilder<Score<T>>> configureGameInfo) where T : IGame {
			var scoreConfig=modelBuilder.Entity<Score<T>>();
			scoreConfig.ToTable(tableName);
			scoreConfig.HasKey(s => s.Id);

			configureGameInfo(scoreConfig);
		}
	}
}
