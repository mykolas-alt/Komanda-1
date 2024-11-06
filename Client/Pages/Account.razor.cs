using Microsoft.AspNetCore.Components;
using Projektas.Shared.Models;

namespace Projektas.Client.Pages {
	public partial class Account {
		private string? username=null;
		private string accountUsername = "";
		private string accountPassword = "";
		private string newAccountName = "";
		private string newAccountSurname = "";
		private string newAccountUsername = "";
		private string newAccountPassword = "";

		private string? token = "";

		private string? test = "Ok";

		private bool isUsernameNew = true;
		private bool isFieldsFilled = true;
		private bool isNewFieldsFilled = true;

		private async void LogInEvent() {
			if(accountUsername==""||accountPassword=="") {
				isFieldsFilled = false;
			} else {
				User account = new User();
	
				isFieldsFilled=true;
	
				account.Username=accountUsername;
				account.Password=accountPassword;

				token = await AccountService.LogIn(account);

				Console.WriteLine(token);

				if(!string.IsNullOrEmpty(token)) {
					await AuthStateProvider.MarkUserAsAuthenticated(token);
					Navigation.NavigateTo("/");
				}
			}
		}

		private void LogOffEvent() {
			AuthStateProvider.MarkUserAsLoggedOut();

			token="";

			Navigation.NavigateTo("/");
		}

		private async void SignUpEvent() {
			if(newAccountName==""||newAccountSurname==""||newAccountUsername==""||newAccountPassword=="") {
				isNewFieldsFilled=false;
			} else if(isUsernameNew) {
				User newAccount = new User();

				isNewFieldsFilled=true;

				newAccount.Name=newAccountName;
				newAccount.Surname=newAccountSurname;
				newAccount.Username=newAccountUsername;
				newAccount.Password=newAccountPassword;

				await AccountService.CreateAccountAsync(newAccount);
			}
			StateHasChanged();
		}

		private async void UsernameChange(ChangeEventArgs changeEvent) {
			List<string> usernames = await AccountService.GetUsernames();

			newAccountUsername=(string)changeEvent.Value;

			isUsernameNew=true;

			foreach(string username in usernames) {
				if(username==newAccountUsername) {
					isUsernameNew=false;
				}
			}

			StateHasChanged();
		}

		protected override async Task OnInitializedAsync() {
			AuthStateProvider.AuthenticationStateChanged += OnAuthenticationStateChanged;

			User user = await DatabaseService.GetUserByIdAsync(3);

			test=user.Username;

			await LoadUsernameAsync();
		}

		private async Task LoadUsernameAsync() {
			username = await AuthStateProvider.GetUsernameAsync();
			StateHasChanged();
		}

		private async void OnAuthenticationStateChanged(Task<AuthenticationState> task) {
			await InvokeAsync(LoadUsernameAsync);
		}
	}
}
