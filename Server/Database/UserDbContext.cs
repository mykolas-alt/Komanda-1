using Microsoft.EntityFrameworkCore;
using Projektas.Shared.Models;

namespace Projektas.Server.Database {
	public class UserDbContext : DbContext {
		public UserDbContext(DbContextOptions<UserDbContext> options) : base(options) {}

		public DbSet<User> Users {get;set;}
		public DbSet<MathGameScore> MathGameScores {get;set;}
		public DbSet<SudokuScore> SudokuScores {get;set;}
		public DbSet<AimTrainerScore> AimTrainerScores {get;set;}
		public DbSet<PairUpScore> PairUpScores {get;set;}

		protected override void OnModelCreating(ModelBuilder modelBuilder) {
			var userConfig=modelBuilder.Entity<User>();
			userConfig.ToTable("users");
			userConfig.HasKey(c => c.Id);
			userConfig.Property(c => c.Name).HasColumnName("name").IsRequired();
			userConfig.Property(c => c.Surname).HasColumnName("surname").IsRequired();
			userConfig.Property(c => c.Username).HasColumnName("username").IsRequired();
			userConfig.Property(c => c.Password).HasColumnName("password").IsRequired();

			var mgScoresConfig=modelBuilder.Entity<MathGameScore>();
			mgScoresConfig.ToTable("mathGameScores");
			mgScoresConfig.HasKey(c => c.Id);
			mgScoresConfig.Property(s => s.UserScores).HasColumnName("userScore");

			mgScoresConfig.HasOne(s => s.User).WithMany(u => u.MathGameScores).HasForeignKey(s => s.UserId);
			
			var sudokuScoresConfig=modelBuilder.Entity<SudokuScore>();
			sudokuScoresConfig.ToTable("sudokuScores");
			sudokuScoresConfig.HasKey(c => c.Id);
			sudokuScoresConfig.Property(s => s.UserScores).HasColumnName("userScore");

			sudokuScoresConfig.HasOne(s => s.User).WithMany(u => u.SudokuScores).HasForeignKey(s => s.UserId);

			var atScoresConfig=modelBuilder.Entity<AimTrainerScore>();
			atScoresConfig.ToTable("aimTrainerScores");
			atScoresConfig.HasKey(c => c.Id);
			atScoresConfig.Property(s => s.UserScores).HasColumnName("userScore");

			atScoresConfig.HasOne(s => s.User).WithMany(u => u.AimTrainerScores).HasForeignKey(s => s.UserId);

			var puScoresConfig=modelBuilder.Entity<PairUpScore>();
			puScoresConfig.ToTable("pairUpScores");
			puScoresConfig.HasKey(c => c.Id);
			puScoresConfig.Property(s => s.UserScores).HasColumnName("userScore");

			puScoresConfig.HasOne(s => s.User).WithMany(u => u.PairUpScores).HasForeignKey(s => s.UserId);
		}
	}
}
