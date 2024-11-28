using System.Data;
using Projektas.Shared.Models;
using Projektas.Server.Database;
using Projektas.Server.Exceptions;
using Microsoft.EntityFrameworkCore;
using Projektas.Shared.Interfaces;
using Projektas.Shared.Enums;

namespace Projektas.Server.Services {
    public class ScoreRepository<T> : IScoreRepository<T> where T : IGame {
        private readonly UserDbContext _userDbContext;

		public ScoreRepository(UserDbContext userDbContext) {
			_userDbContext=userDbContext;
		}

		public async Task AddScoreToUserAsync(string username,int userScore, string? difficulty) {
			try {
				var user=await _userDbContext.Users.FirstOrDefaultAsync(u => u.Username==username);
				if(user==null) {
                    return;
                }

				var score = new Score<T>
				{
					UserScores=userScore,
					UserId=user.Id,
					Timestamp = DateTime.UtcNow.ToLocalTime(),
					Difficulty = difficulty

				};

				_userDbContext.Set<Score<T>>().Add(score);
				await _userDbContext.SaveChangesAsync();
			} catch(DbUpdateException dbEx) {
				throw new DatabaseOperationException("An error occurred while updating the database.",dbEx);
			} catch(Exception ex) {
				throw new DatabaseOperationException("An error occurred during the database operation.",ex);
			}
			
        }

		public async Task<int?> GetHighscoreFromUserAsync(string username) {
			try {
				var user=await _userDbContext.Users.FirstOrDefaultAsync(u => u.Username==username);

				if (user==null)
					return null;

				var scores=_userDbContext.Set<Score<T>>().Where(s => s.UserId==user.Id);
				if (!scores.Any()){
					return 0;
				}
				else{
                    return scores.Max(s => s.UserScores);
                }

				
			} catch(DbUpdateException dbEx) {
				throw new DatabaseOperationException("An error occurred while updating the database.",dbEx);
			} catch(Exception ex) {
				throw new DatabaseOperationException("An error occurred during the database operation.",ex);
			}
        }

		public async Task<List<UserScoreDto>> GetAllScoresAsync() {
			try {

                return await _userDbContext.Set<Score<T>>()
					.Include(s => s.User)
					.Select(s => new UserScoreDto {
						Username=s.User.Username,
						Score=s.UserScores,
						Difficulty=s.Difficulty,
                        Timestamp = s.Timestamp
                    })
					.ToListAsync();
			} catch(DbUpdateException dbEx) {
				throw new DatabaseOperationException("An error occurred while updating the database.",dbEx);
			} catch(Exception ex) {
				throw new DatabaseOperationException("An error occurred during the database operation.",ex);
			}
		}
    }
}
