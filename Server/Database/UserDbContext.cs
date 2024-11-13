using Microsoft.EntityFrameworkCore;
using Projektas.Shared.Models;

namespace Projektas.Server.Database {
	public class UserDbContext : DbContext {
		public UserDbContext(DbContextOptions<UserDbContext> options) : base(options) {}

		public DbSet<User> Users { get; set; }
		public DbSet<Score> MathGameScores { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder) {
			var userConfig=modelBuilder.Entity<User>();
			userConfig.ToTable("users");
			userConfig.HasKey(c => c.Id);
			userConfig.Property(c => c.Name).HasColumnName("name").IsRequired();
			userConfig.Property(c => c.Surname).HasColumnName("surname").IsRequired();
			userConfig.Property(c => c.Username).HasColumnName("username").IsRequired();
			userConfig.Property(c => c.Password).HasColumnName("password").IsRequired();

			var mgScoresConfig=modelBuilder.Entity<Score>();
			mgScoresConfig.ToTable("userScores");
			mgScoresConfig.HasKey(c => c.Id);
			mgScoresConfig.Property(s => s.UserScores).HasColumnName("userScore");

			mgScoresConfig.HasOne(s => s.User).WithMany(u => u.MathGameScores).HasForeignKey(s => s.UserId);
		}
	}
}
