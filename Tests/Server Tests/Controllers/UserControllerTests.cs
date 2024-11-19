using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.TestHost;
using Moq;
using Projektas.Server.Interfaces;
using Projektas.Shared.Models;
using System.Net.Http.Json;

namespace Projektas.Tests.Controllers {
	public class UserControllerTests : IClassFixture<WebApplicationFactory<Program>> {
		private readonly HttpClient _client;
		private readonly WebApplicationFactory<Program> _factory;
		private readonly Mock<IUserService> _mockUserService;

		public UserControllerTests(WebApplicationFactory<Program> factory) {
			_factory=factory;

			_mockUserService=new Mock<IUserService>();
			_mockUserService.Setup(m => m.LogInToUser(It.IsAny<User>())).ReturnsAsync(true);
			_mockUserService.Setup(m => m.GenerateJwtToken(It.IsAny<User>())).Returns("fake-jwt-token");
			_mockUserService.Setup(m => m.GetUsernamesAsync()).ReturnsAsync(new List<string> { "user1", "user2" });

			_client = _factory.WithWebHostBuilder(builder => {
				builder.ConfigureTestServices(services => {
					services.AddSingleton(_mockUserService.Object);
				});
			})
			.CreateClient();
		}

		[Fact]
		public async Task LogIn_ReturnsToken_WhenCredentialsAreValid() {
			var user=new User {Username="test",Password="password"};

			var response=await _client.PostAsJsonAsync("api/user/login",user);
			response.EnsureSuccessStatusCode();
			var result=await response.Content.ReadFromJsonAsync<LoginResponse>();

			Assert.NotNull(result);
			Assert.Equal("fake-jwt-token",result.Token);
		}

		[Fact]
		public async Task CreateUser_ReturnsOk() {
			var newUser=new User{Username="newuser",Password="password"};

			var response=await _client.PostAsJsonAsync("api/user/create_user",newUser);
			response.EnsureSuccessStatusCode();

			_mockUserService.Verify(m => m.CreateUser(It.Is<User>(u => u.Username==newUser.Username && u.Password==newUser.Password)),Times.Once);
		}

		[Fact]
		public async Task GetUsernames_ReturnsListOfUsernames() {
			var response=await _client.GetAsync("api/user/usernames");
			response.EnsureSuccessStatusCode();
			var usernames=await response.Content.ReadFromJsonAsync<List<string>>();

			Assert.NotNull(usernames);
			Assert.Equal(2,usernames.Count);
			Assert.Contains("user1",usernames);
			Assert.Contains("user2",usernames);
		}
	}
}

