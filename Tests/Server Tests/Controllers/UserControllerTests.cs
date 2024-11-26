using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Projektas.Shared.Models;
using System.Net.Http.Json;
using Projektas.Tests.Server_Tests;
using Projektas.Server.Database;
using Microsoft.EntityFrameworkCore;

namespace Projektas.Tests.Controllers {
	public class UserControllerTests : IClassFixture<CustomWebApplicationFactory<Program>> {
		private readonly HttpClient _client;
		private readonly CustomWebApplicationFactory<Program> _factory;

		public UserControllerTests(CustomWebApplicationFactory<Program> factory) {
			_factory=factory;
			_client=_factory.CreateClient(new WebApplicationFactoryClientOptions {AllowAutoRedirect=false});
		}

		[Fact]
		public async Task LogIn_ReturnsToken_WhenCredentialsAreValid() {
			using(var scope=_factory.Services.CreateScope()) {
				var scopedServices=scope.ServiceProvider;
				var db=scopedServices.GetRequiredService<UserDbContext>();

				db.Database.EnsureCreated();
				Seeding.InitializeTestDB(db);
			}

			var user=new User {Username="johndoe",Password="password123"};

			var response=await _client.PostAsJsonAsync("api/user/login",user);
			response.EnsureSuccessStatusCode();

			Assert.Equal(System.Net.HttpStatusCode.OK,response.StatusCode);

			var result=await response.Content.ReadFromJsonAsync<LoginResponse>();

			Assert.NotNull(result);
			Assert.NotNull(result.Token);
		}

		[Fact]
		public async Task CreateUser_ReturnsOk_AddsNewUserToTheDatabase() {
			using(var scope=_factory.Services.CreateScope()) {
				var scopedServices=scope.ServiceProvider;
				var db=scopedServices.GetRequiredService<UserDbContext>();

				db.Database.EnsureCreated();
				Seeding.InitializeTestDB(db);
			}

			var newUser=new User {Username="newuser",Password="password"};

			var response=await _client.PostAsJsonAsync("api/user/create_user",newUser);
			response.EnsureSuccessStatusCode();

			Assert.Equal(System.Net.HttpStatusCode.OK,response.StatusCode);

			using(var scope=_factory.Services.CreateScope()) {
				var scopedServices=scope.ServiceProvider;
				var db=scopedServices.GetRequiredService<UserDbContext>();
				var createdUser=await db.Users.FirstOrDefaultAsync(u => u.Username==newUser.Username);

				Assert.NotNull(createdUser);
				Assert.Equal(newUser.Username,createdUser.Username);
				Assert.Equal(newUser.Password,createdUser.Password);
			}
		}

		[Fact]
		public async Task GetUsernames_ReturnsListOfUsernamesFromTheDatabase() {
			using(var scope=_factory.Services.CreateScope()) {
				var scopedServices=scope.ServiceProvider;
				var db=scopedServices.GetRequiredService<UserDbContext>();

				db.Database.EnsureCreated();
				Seeding.InitializeTestDB(db);
			}

			var response=await _client.GetAsync("api/user/usernames");
			response.EnsureSuccessStatusCode();
			var usernames=await response.Content.ReadFromJsonAsync<List<string>>();

			Assert.Equal(System.Net.HttpStatusCode.OK,response.StatusCode);

			using(var scope=_factory.Services.CreateScope()) {
				var scopedServices=scope.ServiceProvider;
				var db=scopedServices.GetRequiredService<UserDbContext>();

				List<string> actualUsernames=await db.Users.Select(u => u.Username).ToListAsync();
				Assert.NotNull(usernames);
				Assert.NotEmpty(usernames);
				Assert.NotEmpty(actualUsernames);
				Assert.Equal(actualUsernames.Count,usernames.Count);
				foreach(var username in actualUsernames) {
					Assert.Contains(username,usernames);
				}
			}
		}
	}
}

