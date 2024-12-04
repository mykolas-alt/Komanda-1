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

		public async Task AddScoreToUserAsync<T>(string username,T gameData,object additionalInfo) where T : IGame {
			try {
				var user=await _userDbContext.Users.FirstOrDefaultAsync(u => u.Username==username);
				if(user==null) {
                    return;
                }

				if(gameData is AimTrainerData aimTrainer && additionalInfo is int aimTScore) {
					aimTrainer.Scores=aimTScore;
				} else if(gameData is MathGameData mathGame && additionalInfo is int mathScore) {
					mathGame.Scores=mathScore;
				} else if(gameData is PairUpData pairUp && additionalInfo is (int pairUpTime,int pairUpFails)) {
					pairUp.TimeInSeconds=pairUpTime;
					pairUp.Fails=pairUpFails;
				} else if(gameData is SudokuData sudokuGame && additionalInfo is (int sudokuTime,bool sudokuSolved)) {
					sudokuGame.TimeInSeconds=sudokuTime;
					sudokuGame.Solved=sudokuSolved;
				} else {
					throw new ArgumentException("Invalid game info or additional info provided.");
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

		public async Task<UserScoreDto<T>?> GetHighscoreFromUserAsync<T>(string username) where T : IGame {
			try {
				var user=await _userDbContext.Users.FirstOrDefaultAsync(u => u.Username==username);

				if(user==null)
					return null;

				if(typeof(T)==typeof(AimTrainerData)) {
					var score=await _userDbContext.Set<Score<AimTrainerData>>()
						.Where(s => s.UserId==user.Id)
						.OrderByDescending(s => s.GameData.Scores)
						.FirstOrDefaultAsync();

					return score==null ? null : new UserScoreDto<T> {
						Username=username,
						GameData=(T)(IGame)score.GameData
					};
				} else if(typeof(T)==typeof(MathGameData)) {
					var score=await _userDbContext.Set<Score<MathGameData>>()
						.Where(s => s.UserId==user.Id)
						.OrderByDescending(s => s.GameData.Scores)
						.FirstOrDefaultAsync();

					return score==null ? null : new UserScoreDto<T> {
						Username=username,
						GameData=(T)(IGame)score.GameData
					};
				} else if(typeof(T)==typeof(PairUpData)) {
					var score=await _userDbContext.Set<Score<PairUpData>>()
						.Where(s => s.UserId==user.Id)
						.OrderBy(s => s.GameData.TimeInSeconds)
						.ThenBy(s => s.GameData.Fails)
						.FirstOrDefaultAsync();

					return score==null ? null : new UserScoreDto<T> {
						Username=username,
						GameData=(T)(IGame)score.GameData
					};
				} else if(typeof(T)==typeof(SudokuData)) {
					var score=await _userDbContext.Set<Score<SudokuData>>()
						.Where(s => s.UserId==user.Id && s.GameData.Solved)
						.OrderBy(s => s.GameData.TimeInSeconds)
						.FirstOrDefaultAsync();

					return score==null ? null : new UserScoreDto<T> {
						Username=username,
						GameData=(T)(IGame)score.GameData
					};
				} else {
					throw new NotSupportedException($"Highscore retrieval is not supported for type {typeof(T).Name}");
				}
			} catch(DbUpdateException dbEx) {
				throw new DatabaseOperationException("An error occurred while updating the database.",dbEx);
			} catch(Exception ex) {
				throw new DatabaseOperationException("An error occurred during the database operation.",ex);
			}
        }

		public async Task<List<UserScoreDto<T>>> GetAllScoresAsync<T>() where T : IGame {
			try {
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
						.Where(s => s.GameData.Solved)
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