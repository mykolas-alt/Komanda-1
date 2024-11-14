using Microsoft.Extensions.Configuration;
using Moq;
using Projektas.Shared.Models;
using System.Text.Json;
using Projektas.Server.Services;

namespace Projektas.Tests.Services
{
    public class UserServiceTests : IDisposable
    {
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly string _testFilePath = "test_users.json";
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _mockConfiguration = new Mock<IConfiguration>();
            _mockConfiguration.Setup(config => config["Jwt:Key"]).Returns("a_very_long_secret_key_1234567890");
            _mockConfiguration.Setup(config => config["Jwt:Issuer"]).Returns("issuer");
            _mockConfiguration.Setup(config => config["Jwt:Audience"]).Returns("audience");

            _userService = new UserService(_testFilePath, _mockConfiguration.Object);

            var initialUsers = new List<User>
            {
                new User { Name = "John", Surname = "Doe", Username = "johndoe", Password = "password123"}
            };
            File.WriteAllText(_testFilePath, JsonSerializer.Serialize(initialUsers));
        }

        [Fact]
        public void LogInToUser_ValidUser_ReturnsTrue()
        {
            var user = new User { Username = "johndoe", Password = "password123" };

            var result = _userService.LogInToUser(user);

            Assert.True(result);
        }

        [Fact]
        public void GenerateJwtToken_ValidUser_ReturnsToken()
        {
            var user = new User { Username = "johndoe" };

            var token = _userService.GenerateJwtToken(user);

            Assert.NotNull(token);
            Assert.NotEmpty(token);
        }

        [Fact]
        public void LogInToUser_InvalidUser_ReturnsFalse()
        {
            var userService = new UserService(_testFilePath, _mockConfiguration.Object);
            var user = new User { Username = "janedoe", Password = "wrongpassword" };

            var result = userService.LogInToUser(user);

            Assert.False(result);
        }

        [Fact]
        public void Create_User_NewUser_AddsUserToList()
        {
            var newUser = new User { Name = "Jane", Surname = "Doe", Username = "janedoe", Password = "password456" };

            _userService.CreateUser(newUser);

            var users = _userService.GetUsernames();
            Assert.Contains("janedoe", users);
        }

        [Fact]
        public void GetUsernames_ReturnsListOfUsernames()
        {
            var usernames = _userService.GetUsernames();

            Assert.Contains("johndoe", usernames);
        }

        public void Dispose()
        {
            if (File.Exists(_testFilePath))
            {
                File.Delete(_testFilePath);
            }
        }
    }
}
