using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Projektas.Server.Database;
using System.Data.Common;

namespace Projektas.Tests.Server_Tests {
	public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class {
		protected override void ConfigureWebHost(IWebHostBuilder builder) {
			base.ConfigureWebHost(builder);

			builder.ConfigureTestServices(services => {
				var dbContextDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<UserDbContext>));

				services.Remove(dbContextDescriptor);

				var dbConnectionDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbConnection));

				services.Remove(dbConnectionDescriptor);

				services.AddSingleton<DbConnection>(container => {
					var connection = new SqliteConnection("DataSource=:memory:");
					connection.Open();

					return connection;
				});

				services.AddDbContext<UserDbContext>((container,options) => {
					var connection = container.GetRequiredService<DbConnection>();
					options.UseSqlite(connection);
				});
			});

			builder.UseEnvironment("Development");
		}
	}
}
