using System.Data;
using Projektas.Shared.Models;
using Projektas.Server.Database;
using Projektas.Server.Exceptions;
using Projektas.Server.Interfaces;
using Microsoft.EntityFrameworkCore;
using Projektas.Shared.Interfaces;

namespace Projektas.Server.Repositories {
    public class ScoreRepository : IScoreRepository {
        private readonly UserDbContext _userDbContext;

		public ScoreRepository(UserDbContext userDbContext) {
			_userDbContext=userDbContext;
		}

		public async Task AddScoreToUserAsync<T>(string username,T gameData) where T : IGame {
			try {
				var user=await _userDbContext.Users.FirstOrDefaultAsync(u => u.Username==username);
				if(user==null) {
                    return;
                }

				var score=new Score<T> {
					UserId=user.Id,
					User=user,
					GameData=gameData
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
				var user=await _userDbContext.Users.FirstOrDefaultAsync(u => u.Username==username);

				if(user==null)
					return new List<UserScoreDto<T>>();

				return await _userDbContext.Set<Score<T>>()
					.AsNoTracking()
					.Where(s => s.UserId==user.Id)
					.Select(s => new UserScoreDto<T> {
						Username=s.User.Username,
						GameData=(T)(IGame)s.GameData
					})
					.ToListAsync();
			} catch(DbUpdateException dbEx) {
				throw new DatabaseOperationException("An error occurred while updating the database.",dbEx);
			} catch(Exception ex) {
				throw new DatabaseOperationException("An error occurred during the database operation.",ex);
			}
        }

		public async Task<List<UserScoreDto<T>>> GetAllScoresAsync<T>() where T : IGame {
			try {
				return await _userDbContext.Set<Score<T>>()
					.AsNoTracking()
					.Include(s => s.User)
					.Select(s => new UserScoreDto<T> {
						Username=s.User.Username,
						GameData=(T)(IGame)s.GameData
					})
					.ToListAsync();
				if(typeof(T)==typeof(AimTrainerData)) {
					return await _userDbContext.Set<Score<AimTrainerData>>()
						.AsNoTracking()
						.Include(s => s.User)
						.OrderByDescending(s => s.GameData.Scores)
						.Select(s => new UserScoreDto<T> {
							Username=s.User.Username,
							GameData=(T)(IGame)s.GameData
						})
						.ToListAsync();
				} else if(typeof(T)==typeof(MathGameData)) {
					return await _userDbContext.Set<Score<MathGameData>>()
						.AsNoTracking()
						.Include(s => s.User)
						.OrderByDescending(s => s.GameData.Scores)
						.Select(s => new UserScoreDto<T> {
							Username=s.User.Username,
							GameData=(T)(IGame)s.GameData
						})
						.ToListAsync();
				} else if(typeof(T)==typeof(PairUpData)) {
					return await _userDbContext.Set<Score<PairUpData>>()
						.AsNoTracking()
						.Include(s => s.User)
						.OrderBy(s => s.GameData.TimeInSeconds)
						.ThenBy(s => s.GameData.Fails)
						.Select(s => new UserScoreDto<T> {
							Username=s.User.Username,
							GameData=(T)(IGame)s.GameData
						})
						.ToListAsync();
				} else if(typeof(T)==typeof(SudokuData)) {
					return await _userDbContext.Set<Score<SudokuData>>()
						.AsNoTracking()
						.Include(s => s.User)
						.OrderByDescending(s => s.GameData.TimeInSeconds)
						.Select(s => new UserScoreDto<T> {
							Username=s.User.Username,
							GameData=(T)(IGame)s.GameData
						})
						.ToListAsync();
				} else {
					throw new NotSupportedException($"Highscore retrieval is not supported for type {typeof(T).Name}");
				}
			} catch(DbUpdateException dbEx) {
				throw new DatabaseOperationException("An error occurred while updating the database.",dbEx);
			} catch(Exception ex) {
				throw new DatabaseOperationException("An error occurred during the database operation.",ex);
			}
		}
    }
}