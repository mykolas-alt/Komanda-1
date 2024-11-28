using System.Data;
using Projektas.Shared.Models;
using Projektas.Server.Database;
using Projektas.Server.Exceptions;
using Microsoft.EntityFrameworkCore;
using Projektas.Shared.Interfaces;

namespace Projektas.Server.Services {
    public class ScoreRepository : IScoreRepository {
        private readonly UserDbContext _userDbContext;

		public ScoreRepository(UserDbContext userDbContext) {
			_userDbContext=userDbContext;
		}

		public async Task AddScoreToUserAsync<T>(string username,T gameInfo,object additionalInfo) where T : IGame {
			try {
				var user=await _userDbContext.Users.FirstOrDefaultAsync(u => u.Username==username);
				if(user==null) {
                    return;
                }

				if(gameInfo is AimTrainerModel aimTrainer && additionalInfo is int aimTScore) {
					aimTrainer.UserScores=aimTScore;
				} else if(gameInfo is MathGameModel mathGame && additionalInfo is int mathScore) {
					mathGame.UserScores=mathScore;
				} else if(gameInfo is PairUpModel pairUp && additionalInfo is (int pairUpTime,int pairUpFails)) {
					pairUp.UserTimeInSeconds=pairUpTime;
					pairUp.Fails=pairUpFails;
				} else if(gameInfo is SudokuModel sudokuGame && additionalInfo is (int sudokuTime,bool sudokuSolved)) {
					sudokuGame.UserTimeInSeconds=sudokuTime;
					sudokuGame.Solved=sudokuSolved;
				} else {
					throw new ArgumentException("Invalid game info or additional info provided.");
				}

				var score=new Score<T> {
					UserId=user.Id,
					User=user,
					GameInfo=gameInfo
				};

				_userDbContext.Set<Score<T>>().Add(score);
				await _userDbContext.SaveChangesAsync();
			} catch(DbUpdateException dbEx) {
				throw new DatabaseOperationException("An error occurred while updating the database.",dbEx);
			} catch(Exception ex) {
				throw new DatabaseOperationException("An error occurred during the database operation.",ex);
			}
			
        }

		public async Task<int?> GetHighscoreFromUserAsync<T>(string username) where T : IGame {
			try {
				var user=await _userDbContext.Users.FirstOrDefaultAsync(u => u.Username==username);

				if (user==null)
					return null;

				if(typeof(T)==typeof(AimTrainerModel)) {
					return await _userDbContext.Set<Score<AimTrainerModel>>()
						.Where(s => s.UserId==user.Id)
						.MaxAsync(s => s.GameInfo.UserScores);
				} else if(typeof(T)==typeof(MathGameModel)) {
					return await _userDbContext.Set<Score<MathGameModel>>()
						.Where(s => s.UserId==user.Id)
						.MaxAsync(s => s.GameInfo.UserScores);
				} else if(typeof(T)==typeof(PairUpModel)) {
					return await _userDbContext.Set<Score<PairUpModel>>()
						.Where(s => s.UserId==user.Id)
						.MaxAsync(s => s.GameInfo.UserTimeInSeconds);
				} else if(typeof(T)==typeof(SudokuModel)) {
					return await _userDbContext.Set<Score<SudokuModel>>()
						.Where(s => s.UserId==user.Id)
						.MaxAsync(s => s.GameInfo.UserTimeInSeconds);
				} else {
					throw new NotSupportedException($"Highscore retrieval is not supported for type {typeof(T).Name}");
				}
			} catch(DbUpdateException dbEx) {
				throw new DatabaseOperationException("An error occurred while updating the database.",dbEx);
			} catch(Exception ex) {
				throw new DatabaseOperationException("An error occurred during the database operation.",ex);
			}
        }

		public async Task<List<UserScoreDto>> GetAllScoresAsync<T>() where T : IGame {
			try {
				if(typeof(T)==typeof(AimTrainerModel)) {
					return await _userDbContext.Set<Score<AimTrainerModel>>()
						.Include(s => s.User)
						.OrderByDescending(s => s.GameInfo.UserScores)
						.Select(s => new UserScoreDto {
							Username=s.User.Username,
							Data=s.GameInfo.UserScores
						})
						.ToListAsync();
				} else if(typeof(T)==typeof(MathGameModel)) {
					return await _userDbContext.Set<Score<MathGameModel>>()
						.Include(s => s.User)
						.OrderByDescending(s => s.GameInfo.UserScores)
						.Select(s => new UserScoreDto {
							Username=s.User.Username,
							Data=s.GameInfo.UserScores
						})
						.ToListAsync();
				} else if(typeof(T)==typeof(PairUpModel)) {
					return await _userDbContext.Set<Score<PairUpModel>>()
						.Include(s => s.User)
						.OrderByDescending(s => s.GameInfo.UserTimeInSeconds)
						.Select(s => new UserScoreDto {
							Username=s.User.Username,
							Data=s.GameInfo.UserTimeInSeconds
						})
						.ToListAsync();
				} else if(typeof(T)==typeof(SudokuModel)) {
					return await _userDbContext.Set<Score<SudokuModel>>()
						.Include(s => s.User)
						.OrderByDescending(s => s.GameInfo.UserTimeInSeconds)
						.Select(s => new UserScoreDto {
							Username=s.User.Username,
							Data=s.GameInfo.UserTimeInSeconds
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
