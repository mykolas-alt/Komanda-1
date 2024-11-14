using Projektas.Server.Interface;
using System.Data;
using Projektas.Shared.Models;
using Projektas.Server.Database;
using Microsoft.EntityFrameworkCore;

namespace Projektas.Server.Services {
	public class UserRepository : IUserRepository {
		private readonly UserDbContext _userDbContext;

		public UserRepository(UserDbContext userDbContext) {
			_userDbContext=userDbContext;
		}

		public async Task CreateUserAsync(User user) {
			_userDbContext.Users.Add(user);
			await _userDbContext.SaveChangesAsync();
		}

		public async Task<List<User>> GetAllUsersAsync() {
			return await _userDbContext.Users.ToListAsync();
		}

		public async Task<User> GetUserByIdAsync(int id) {
			return await _userDbContext.Users.FirstOrDefaultAsync(u => u.Id==id);
		}

		public async Task AddMathGameScoreToUserAsync(string username,int userScore) {
			var user=await _userDbContext.Users.FirstOrDefaultAsync(u => u.Username==username);
			if (user==null) {
				return;
			}

			var score=new MathGameScore {
				UserScores=userScore,
				UserId=user.Id
			};

			_userDbContext.MathGameScores.Add(score);
			await _userDbContext.SaveChangesAsync();
		}

		public async Task<int?> GetMathGameHighscoreFromUserAsync(string username) {
			var user=await _userDbContext.Users
				.Include(u => u.MathGameScores)
				.FirstOrDefaultAsync(u => u.Username==username);

			return user?.MathGameScores.Max(s => s.UserScores);
		}

		public async Task<List<UserScoreDto>> GetAllMathGameScoresAsync() {
			return await _userDbContext.MathGameScores
				.Include(s => s.User)
				.OrderByDescending(s => s.UserScores)
				.Select(s => new UserScoreDto {
					Username=s.User.Username,
					Score=s.UserScores
				})
				.ToListAsync();

		}

		public async Task<bool> ValidateUserAsync(string username,string password) {
			var user=await _userDbContext.Users.FirstOrDefaultAsync(u => u.Username==username);

			if (user!=null && password==user.Password) {
				return true;
			}

			return false;
		}
	}
}
