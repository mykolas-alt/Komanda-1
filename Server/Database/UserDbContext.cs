using Microsoft.EntityFrameworkCore;
using Projektas.Shared.Models;

namespace Projektas.Server.Database
{
    public class UserDbContext : DbContext {
		public UserDbContext(DbContextOptions<UserDbContext> options) : base(options) {}

		public DbSet<User> Users {get;set;}

		public DbSet<Score<MathGameM>> MathGameScores {get;set;}
		public DbSet<Score<SudokuM>> SudokuScores {get;set;}
		public DbSet<Score<AimTrainerM>> AimTrainerScores {get;set;}
		public DbSet<Score<PairUpM>> PairUpScores {get;set;}

		protected override void OnModelCreating(ModelBuilder modelBuilder) {
			var userConfig=modelBuilder.Entity<User>();
			userConfig.ToTable("users");
			userConfig.HasKey(c => c.Id);
			userConfig.Property(c => c.Name).HasColumnName("name").IsRequired();
			userConfig.Property(c => c.Surname).HasColumnName("surname").IsRequired();
			userConfig.Property(c => c.Username).HasColumnName("username").IsRequired();
			userConfig.Property(c => c.Password).HasColumnName("password").IsRequired();

			var mgScoresConfig=modelBuilder.Entity<Score<MathGameM>>();
			mgScoresConfig.ToTable("mathGameScores");
			mgScoresConfig.HasKey(c => c.Id);
			mgScoresConfig.Property(s => s.UserScores).HasColumnName("userScore");
            mgScoresConfig.Property(s => s.Timestamp).HasColumnName("Timestamp").IsRequired();

            mgScoresConfig.HasOne(s => s.User).WithMany(u => u.MathGameScores).HasForeignKey(s => s.UserId);
			
			var sudokuScoresConfig=modelBuilder.Entity<Score<SudokuM>>();
			sudokuScoresConfig.ToTable("sudokuScores");
			sudokuScoresConfig.HasKey(c => c.Id);
			sudokuScoresConfig.Property(s => s.UserScores).HasColumnName("userScore");
            sudokuScoresConfig.Property(s => s.Timestamp).HasColumnName("Timestamp").IsRequired();
            sudokuScoresConfig.Property(s => s.Difficulty).HasColumnName("Difficulty");

            sudokuScoresConfig.HasOne(s => s.User).WithMany(u => u.SudokuScores).HasForeignKey(s => s.UserId);

			var atScoresConfig=modelBuilder.Entity<Score<AimTrainerM>>();
			atScoresConfig.ToTable("aimTrainerScores");
			atScoresConfig.HasKey(c => c.Id);
			atScoresConfig.Property(s => s.UserScores).HasColumnName("userScore");
            atScoresConfig.Property(s => s.Timestamp).HasColumnName("Timestamp").IsRequired();
			atScoresConfig.Property(s => s.Difficulty).HasColumnName("Difficulty");

            atScoresConfig.HasOne(s => s.User).WithMany(u => u.AimTrainerScores).HasForeignKey(s => s.UserId);

			var puScoresConfig=modelBuilder.Entity<Score<PairUpM>>();
			puScoresConfig.ToTable("pairUpScores");
			puScoresConfig.HasKey(c => c.Id);
			puScoresConfig.Property(s => s.UserScores).HasColumnName("userScore");
            puScoresConfig.Property(s => s.Timestamp).HasColumnName("Timestamp").IsRequired();
            puScoresConfig.Property(s => s.Difficulty).HasColumnName("Difficulty");

            puScoresConfig.HasOne(s => s.User).WithMany(u => u.PairUpScores).HasForeignKey(s => s.UserId);
		}
	}
}
