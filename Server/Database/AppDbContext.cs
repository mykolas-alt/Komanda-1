using Microsoft.EntityFrameworkCore;
using Projektas.Shared.Models;

namespace Projektas.Server.Database {
	public class AppDbContext : DbContext {
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

		public DbSet<User> Users { get; set; }
	}
}
