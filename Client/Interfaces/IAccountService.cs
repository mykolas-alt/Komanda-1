﻿using Projektas.Shared.Models;

namespace Projektas.Client.Interfaces
{
    public interface IAccountService
    {
        public Task<string?> LogIn(User user);
        public Task CreateAccount(User newUser);
        public Task<List<string>> GetUsernames();
    }
}
