using Microsoft.Extensions.Configuration;
using Moq;
using Projektas.Shared.Models;
using Projektas.Server.Services;
using Projektas.Server.Interfaces;

namespace Projektas.Tests.Services {
    public class UserServiceTests {
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<UserTrackingService> _mockUserTrackingService;
        private readonly UserService _userService;

		public UserServiceTests() {
			_mockConfiguration = new Mock<IConfiguration>();
			_mockConfiguration.Setup(config => config["Jwt:Key"]).Returns("a_very_long_secret_key_1234567890");
			_mockConfiguration.Setup(config => config["Jwt:Issuer"]).Returns("issuer");
			_mockConfiguration.Setup(config => config["Jwt:Audience"]).Returns("audience");

			_mockUserRepository = new Mock<IUserRepository>();
			_mockUserTrackingService = new Mock<UserTrackingService>();
			_userService = new UserService(_mockConfiguration.Object, _mockUserRepository.Object, _mockUserTrackingService.Object);

			var initialUsers = new List<User> {
				new User {Name = "John", Surname = "Doe", Username = "johndoe", Password = "password123"}
			};

			_mockUserRepository.Setup(repo => repo.ValidateUserAsync("johndoe", "password123")).ReturnsAsync(true);
			_mockUserRepository.Setup(repo => repo.ValidateUserAsync("janedoe", "wrongpassword")).ReturnsAsync(false);
			_mockUserRepository.Setup(repo => repo.GetAllUsersAsync()).ReturnsAsync(initialUsers);
			_mockUserRepository.Setup(repo => repo.CreateUserAsync(It.IsAny<User>())).Returns(Task.CompletedTask);
		}

		[Fact]
		public async Task LogInToUser_ValidUser_ReturnsTrue() {
			var user = new User {Username = "johndoe", Password = "password123"};

			var result = await _userService.LogInToUserAsync(user);

			Assert.True(result);
		}

		[Fact]
		public void GenerateJwtToken_ValidUser_ReturnsToken() {
			var user = new User {Username = "johndoe"};

			var token = _userService.GenerateJwtToken(user);

			Assert.NotNull(token);
			Assert.NotEmpty(token);
		}

		[Fact]
		public async Task LogInToUser_InvalidUser_ReturnsFalse() {
			var user = new User {Username = "janedoe", Password = "wrongpassword"};

			var result = await _userService.LogInToUserAsync(user);

			Assert.False(result);
		}

		[Fact]
		public async Task CreateUser_NewUser_AddsUserToList() {
			var newUser = new User {Name = "Jane", Surname = "Doe", Username = "janedoe", Password = "password456"};

			await _userService.CreateUserAsync(newUser);

			_mockUserRepository.Setup(repo => repo.GetAllUsersAsync()).ReturnsAsync(new List<User> {
				new User {Name = "John", Surname = "Doe", Username = "johndoe", Password = "password123"},
				new User {Name = "Jane", Surname = "Doe", Username = "janedoe", Password = "password456"}
			});

			var users = await _userService.GetUsernamesAsync();
			Assert.Contains("janedoe", users);
		}

		[Fact]
		public async Task GetUsernames_ReturnsListOfUsernames() {
			var usernames = await _userService.GetUsernamesAsync();

			Assert.Contains("johndoe", usernames);
		}
    }
}
