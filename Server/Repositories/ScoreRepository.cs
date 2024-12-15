using System.Data;
using Projektas.Shared.Models;
using Projektas.Server.Database;
using Projektas.Server.Exceptions;
using Projektas.Server.Interfaces;
using Microsoft.EntityFrameworkCore;
using Projektas.Shared.Interfaces;
using Projektas.Shared.Enums;

namespace Projektas.Server.Repositories {
    public class ScoreRepository : IScoreRepository {
        private readonly UserDbContext _userDbContext;

		public ScoreRepository(UserDbContext userDbContext) {
			_userDbContext = userDbContext;
		}

		public async Task AddScoreToUserAsync<T>(string username, T gameData, DateTime timestamp) where T : IGame {
			try {
				var user=await _userDbContext.Users.FirstOrDefaultAsync(u => u.Username==username);
				if(user==null) {
                    return;
                }

				var score=new Score<T> {
					UserId=user.Id,
					User=user,
					GameData=gameData,
					Timestamp = timestamp
				};

				_userDbContext.Set<Score<T>>().Add(score);
				await _userDbContext.SaveChangesAsync();
			} catch(DbUpdateException dbEx) {
				throw new DatabaseOperationException("An error occurred while updating the database.",dbEx);
			} catch(Exception ex) {
				throw new DatabaseOperationException("An error occurred during the database operation.",ex);
			}
			
        }

		public async Task<List<UserScoreDto<T>>> GetHighscoreFromUserAsync<T>(string username) where T : IGame {
			try {
				var user = await _userDbContext.Users.FirstOrDefaultAsync(u => u.Username == username);

				if(user == null)
					return new List<UserScoreDto<T>>();

				return await _userDbContext.Set<Score<T>>()
					.AsNoTracking()
					.Where(s => s.UserId == user.Id)
					.Select(s => new UserScoreDto<T> {
						Username = s.User.Username,
						IsPrivate = s.User.IsPrivate,
						GameData = (T)(IGame)s.GameData
					})
					.ToListAsync();
			} catch(DbUpdateException dbEx) {
				throw new DatabaseOperationException("An error occurred while updating the database.", dbEx);
			} catch(Exception ex) {
				throw new DatabaseOperationException("An error occurred during the database operation.", ex);
			}
        }

		public async Task<List<UserScoreDto<T>>> GetAllScoresAsync<T>() where T : IGame {
			try {
				return await _userDbContext.Set<Score<T>>()
					.AsNoTracking()
					.Include(s => s.User)
					.Select(s => new UserScoreDto<T> {
						Username = s.User.Username,
						IsPrivate = s.User.IsPrivate,
						GameData = (T)(IGame)s.GameData,
						Timestamp = s.Timestamp
					})
					.ToListAsync();
			} catch(DbUpdateException dbEx) {
				throw new DatabaseOperationException("An error occurred while updating the database.", dbEx);
			} catch(Exception ex) {
				throw new DatabaseOperationException("An error occurred during the database operation.", ex);
			}
		}
    }
}