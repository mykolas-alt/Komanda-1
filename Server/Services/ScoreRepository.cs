using System.Data;
using Projektas.Shared.Models;
using Projektas.Server.Database;
using Projektas.Server.Exceptions;
using Microsoft.EntityFrameworkCore;
using Projektas.Shared.Interfaces;

namespace Projektas.Server.Services {
    public class ScoreRepository<T> where T : IGame {
        private readonly UserDbContext _userDbContext;

		public ScoreRepository(UserDbContext userDbContext) {
			_userDbContext=userDbContext;
		}

		public async Task AddScoreToUserAsync(string username,int userScore) {
			try {
				var user=await _userDbContext.Users.FirstOrDefaultAsync(u => u.Username==username);
				if(user==null) {
                    return;
                }

				var score=new Score<T> {
					UserScores=userScore,
					UserId=user.Id
				};

				_userDbContext.Set<Score<T>>().Add(score);
				await _userDbContext.SaveChangesAsync();
			} catch (DbUpdateException dbEx) {
				throw new DatabaseOperationException("An error occurred while updating the database.", dbEx);
			} catch (Exception ex) {
				throw new DatabaseOperationException("An error occurred during the database operation.", ex);
			}
			
        }

		public async Task<int?> GetHighscoreFromUserAsync(string username) {
			try {
				var user=await _userDbContext.Users.FirstOrDefaultAsync(u => u.Username==username);

				if (user==null)
					return null;

				var scores=_userDbContext.Set<Score<T>>().Where(s => s.UserId==user.Id);

				return scores.Max(s => s.UserScores);
			} catch (DbUpdateException dbEx) {
				throw new DatabaseOperationException("An error occurred while updating the database.", dbEx);
			} catch (Exception ex) {
				throw new DatabaseOperationException("An error occurred during the database operation.", ex);
			}
        }

		public async Task<List<UserScoreDto>> GetAllScoresAsync() {
			try {
				return await _userDbContext.Set<Score<T>>()
					.Include(s => s.User)
					.OrderByDescending(s => s.UserScores)
					.Select(s => new UserScoreDto {
						Username=s.User.Username,
						Score=s.UserScores
					})
					.ToListAsync();
			} catch (DbUpdateException dbEx) {
				throw new DatabaseOperationException("An error occurred while updating the database.", dbEx);
			} catch (Exception ex) {
				throw new DatabaseOperationException("An error occurred during the database operation.", ex);
			}
		}
    }
}
