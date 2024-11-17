﻿using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Projektas.Client.Pages;
using Projektas.Shared.Models;
using Projektas.Client.Interfaces;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace Projektas.Tests.Client_Tests.Pages
{
	public class AccountTests : TestContext
	{
		private readonly Mock<IAccountService> mockAccountService;
		private readonly Mock<IAccountAuthStateProvider> mockAuthStateProvider;

		public AccountTests()
		{
			var mockNavigationManager = new Mock<NavigationManager>();
			mockAccountService = new Mock<IAccountService>();
			mockAuthStateProvider = new Mock<IAccountAuthStateProvider>();

			var emptyUser = new ClaimsPrincipal(new ClaimsIdentity());
			mockAuthStateProvider.Setup(a => a.GetAuthenticationStateAsync()).ReturnsAsync(new AuthenticationState(emptyUser));

			Services.AddSingleton<IAccountAuthStateProvider>(mockAuthStateProvider.Object);
			Services.AddSingleton<AuthenticationStateProvider>(provider => (AuthenticationStateProvider)mockAuthStateProvider.Object);
			Services.AddSingleton(mockAccountService.Object);
			Services.AddSingleton(mockNavigationManager.Object);
		}

		[Fact]
		public async Task LogInEvent_ShouldAuthenticateUser_WhenCredentialsAreValid()
		{
			var claims = new List<Claim>
			{
				new Claim(ClaimTypes.Name, "validUser")
			};
			var identity = new ClaimsIdentity(claims, "TestAuthType");
			var user = new ClaimsPrincipal(identity);
			mockAuthStateProvider.Setup(a => a.GetAuthenticationStateAsync()).ReturnsAsync(new AuthenticationState(user));
			mockAccountService.Setup(s => s.LogIn(It.IsAny<User>())).ReturnsAsync("validToken");

			var cut = RenderComponent<Account>();

			cut.Instance.accountUsername = "validUser";
			cut.Instance.accountPassword = "validPassword";
			await cut.InvokeAsync(() => cut.Instance.LogInEvent());
			var authState = await mockAuthStateProvider.Object.GetAuthenticationStateAsync();


			mockAccountService.Verify(s => s.LogIn(It.IsAny<User>()), Times.Once);
			Assert.NotNull(authState.User.Identity);
			Assert.True(authState.User.Identity.IsAuthenticated);
			mockAuthStateProvider.Verify(a => a.MarkUserAsAuthenticated(It.IsAny<string>()), Times.Once);
			Assert.Equal("validUser", authState.User.Identity.Name);
		}

		[Fact]
		public async Task LogInEvent_ShouldNotAuthenticateUser_WhenCredentialsAreInvalid()
		{
			var cut = RenderComponent<Account>();
			cut.Instance.accountUsername = "";
			cut.Instance.accountPassword = "";

			await cut.InvokeAsync(() => cut.Instance.LogInEvent());

			Assert.False(cut.Instance.isFieldsFilled);
			mockAccountService.Verify(s => s.LogIn(It.IsAny<User>()), Times.Never);
			var authState = await mockAuthStateProvider.Object.GetAuthenticationStateAsync();
			Assert.NotNull(authState.User.Identity);
			Assert.False(authState.User.Identity.IsAuthenticated);
		}

		[Fact]
		public void LogOffEvent_ShouldLogOutUser()
		{
			var cut = RenderComponent<Account>();

			cut.InvokeAsync(() => cut.Instance.LogOffEvent());

			mockAuthStateProvider.Verify(a => a.MarkUserAsLoggedOut(), Times.Once);
		}

		[Fact]
		public void SignUpEvent_ShouldCreateAccount_WhenFieldsAreValid()
		{
			var cut = RenderComponent<Account>();

			cut.Instance.newAccountName = "John";
			cut.Instance.newAccountSurname = "Doe";
			cut.Instance.newAccountUsername = "newUser";
			cut.Instance.newAccountPassword = "newPassword";
			mockAccountService.Setup(s => s.CreateAccount(It.IsAny<User>())).Returns(Task.CompletedTask);

			cut.InvokeAsync(() => cut.Instance.SignUpEvent());

			mockAccountService.Verify(a => a.CreateAccount(It.IsAny<User>()), Times.Once);
		}

		[Fact] public void SignUpEvent_ShouldNotCreateAccountWhenFieldsAreEmpty()
		{
			var cut = RenderComponent<Account>();

			cut.Instance.newAccountName = "";
			cut.Instance.newAccountSurname = "";
			cut.Instance.newAccountUsername = "";
			cut.Instance.newAccountPassword = "";

			cut.InvokeAsync(() => cut.Instance.SignUpEvent());

			Assert.False(cut.Instance.isNewFieldsFilled);
		}

		[Fact]
		public void UsernameChange_ShouldSetIsUsernameNewToFalse_WhenUsernameExists()
		{
			var changeEvent = new ChangeEventArgs { Value = "existinguser" };
			mockAccountService.Setup(s => s.GetUsernames()).ReturnsAsync(new List<string> { "existinguser" });

			var cut = RenderComponent<Account>();

			cut.InvokeAsync(() => cut.Instance.UsernameChange(changeEvent));

			Assert.False(cut.Instance.isUsernameNew);
		}
	}
}