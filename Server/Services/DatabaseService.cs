using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Projektas.Server.Interface;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Projektas.Shared.Models;

namespace Projektas.Server.Services {
	public class DatabaseService : IUserRepository {
		private readonly IConfiguration _configuration;

		public DatabaseService(IConfiguration configuration) {
			_configuration = configuration;
		}

		public async Task<int> CreateUserAsync(User user) {
			using (IDbConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"))) {
				var sql = "INSERT INTO Users (Name, Surname, Username, Password) VALUES (@Name, @Surname, @Username, @Password); SELECT CAST(SCOPE_IDENTITY() as int);";
				return await db.ExecuteScalarAsync<int>(sql, user);
			}
		}

		public async Task<IEnumerable<User>> GetAllUsersAsync() {
			using (IDbConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"))) {
				return await db.QueryAsync<User>("SELECT * FROM Users");
			}
		}

		public async Task<User> GetUserByIdAsync(int id) {
			using (IDbConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"))) {
				return await db.QueryFirstOrDefaultAsync<User>("SELECT * FROM Users WHERE Id = @Id", new {Id=id});
			}
		}

		public async Task<bool> ValidateUserAsync(string username, string password) {
			using (IDbConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"))) {
				var user = await db.QuerySingleOrDefaultAsync<User>("SELECT * FROM Users WHERE Username = @Username", new {Username = username});

				if (user != null && password==user.Password) {
					return true;
				}

				return false;
			}
		}
	}
}
