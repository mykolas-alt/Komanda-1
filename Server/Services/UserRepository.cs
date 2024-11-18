using System.Data;
using Projektas.Shared.Models;
using Projektas.Server.Database;
using Projektas.Server.Exceptions;
using Microsoft.EntityFrameworkCore;
using Projektas.Server.Interfaces;

namespace Projektas.Server.Services {
	public class UserRepository : IUserRepository {
		private readonly UserDbContext _userDbContext;

		public UserRepository(UserDbContext userDbContext) {
			_userDbContext=userDbContext;
		}

		public async Task CreateUserAsync(User user) {
			try {
				if(user==null) {
                    throw new DatabaseOperationException($"User data is null.", "USER_IS_NULL");
                }
				_userDbContext.Users.Add(user);
				await _userDbContext.SaveChangesAsync();
			} catch (DbUpdateException dbEx) {
				throw new DatabaseOperationException("An error occurred while updating the database.", dbEx);
			} catch (Exception ex) {
				throw new DatabaseOperationException("An error occurred during the database operation.", ex);
			}
		}

		public async Task<List<User>> GetAllUsersAsync() {
			try {
				List<User> users=await _userDbContext.Users.ToListAsync();
				if(users==null || users.Count==0) {
					throw new DatabaseOperationException($"Users data is null.", "USERS_IS_NULL");
				}
				return users;
			} catch (DbUpdateException dbEx) {
				throw new DatabaseOperationException("An error occurred while updating the database.", dbEx);
			} catch (Exception ex) {
				throw new DatabaseOperationException("An error occurred during the database operation.", ex);
			}
		}

		public async Task<User> GetUserByIdAsync(int id) {
			try {
                var user=await _userDbContext.Users.FirstOrDefaultAsync(u => u.Id==id);
                if (user==null) {
                    throw new DatabaseOperationException($"User with ID '{id}' not found in the database.", "USER_NOT_FOUND");
                }
                return user;
            }
            catch (DbUpdateException dbEx) {
                throw new DatabaseOperationException("An error occurred while updating the database.", dbEx);
            }
            catch (Exception ex) {
                throw new DatabaseOperationException("An error occurred during the database operation.", ex);
            }
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
