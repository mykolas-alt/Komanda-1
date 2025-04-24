﻿using Projektas.Shared.Models;
using Projektas.Server.Database;
using Projektas.Server.Exceptions;
using Microsoft.EntityFrameworkCore;
using Projektas.Server.Interfaces;

namespace Projektas.Server.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserDbContext _userDbContext;

        public UserRepository(UserDbContext userDbContext)
        {
            _userDbContext = userDbContext;
        }

        public async Task CreateUserAsync(User user)
        {
            try
            {
                if (user == null)
                {
                    throw new DatabaseOperationException($"User data is null.", "USER_IS_NULL");
                }
                user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
                _userDbContext.Users.Add(user);
                await _userDbContext.SaveChangesAsync();
            }
            catch (DbUpdateException dbEx)
            {
                throw new DatabaseOperationException("An error occurred while updating the database.", dbEx);
            }
            catch (Exception ex)
            {
                throw new DatabaseOperationException("An error occurred during the database operation.", ex);
            }
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            try
            {
                List<User> users = await _userDbContext.Users.ToListAsync();
                if (users == null || users.Count == 0)
                {
                    throw new DatabaseOperationException($"Users data is null.", "USERS_IS_NULL");
                }
                return users;
            }
            catch (DbUpdateException dbEx)
            {
                throw new DatabaseOperationException("An error occurred while updating the database.", dbEx);
            }
            catch (Exception ex)
            {
                throw new DatabaseOperationException("An error occurred during the database operation.", ex);
            }
        }

        public async Task<bool> ValidateUserAsync(string username, string password)
        {
            var user = await _userDbContext.Users.FirstOrDefaultAsync(u => u.Username == username);

            if (user != null && BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                return true;
            }

            return false;
        }

        public async Task ChangePrivateAsync(string username, bool priv)
        {
            try
            {
                var user = await _userDbContext.Users
                    .FirstOrDefaultAsync(u => u.Username == username);

                if (user == null)
                {
                    throw new DatabaseOperationException($"No user was found.", "USER_IS_NULL");
                }

                user.IsPrivate = priv;

                await _userDbContext.SaveChangesAsync();
            }
            catch (DbUpdateException dbEx)
            {
                throw new DatabaseOperationException("An error occurred while updating the database.", dbEx);
            }
            catch (Exception ex)
            {
                throw new DatabaseOperationException("An error occurred during the database operation.", ex);
            }
        }

        public async Task<bool> GetPrivateAsync(string username)
        {
            var user = await _userDbContext.Users.FirstOrDefaultAsync(u => u.Username == username);

            if (user != null)
            {
                return user.IsPrivate;
            }

            return false;
        }
    }
}
