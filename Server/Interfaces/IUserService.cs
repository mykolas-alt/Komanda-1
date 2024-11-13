using Projektas.Shared.Models;

namespace Projektas.Server.Interfaces
{
    public interface IUserService
    {
        public bool LogInToUser(User userInfo);

        public void CreateUser(User newUser);

        public string GenerateJwtToken(User user);

        public List<string> GetUsernames();
    }
}
