using Microsoft.AspNetCore.Components;
using Projektas.Shared.Models;

namespace Projektas.Client.Pages {
	public partial class Account {
		private string? accountUsername = null;
		private string? accountPassword = null;
		private string? newAccountName = null;
		private string? newAccountSurname = null;
		private string? newAccountUsername = null;
		private string? newAccountPassword = null;

		private bool isUsernameNew = true;
		private bool isFieldsFilled = true;
		private bool isNewFieldsFilled = true;
		private bool test = false;

		private async void LogInEvent() {
			if(accountUsername==null||accountPassword==null) {
				isFieldsFilled = false;
			} else {
				User account = new User();
	
				isFieldsFilled=true;
	
				account.Username=accountUsername;
				account.Password=accountPassword;

				test = await AccountServices.LogIn(account);
			}
		}

		private async void SignUpEvent() {
			if(newAccountName==null||newAccountSurname==null||newAccountUsername==null||newAccountPassword==null) {
				isNewFieldsFilled=false;
			} else if(isUsernameNew) {
				User newAccount = new User();

				isNewFieldsFilled=true;

				newAccount.Name=newAccountName;
				newAccount.Surname=newAccountSurname;
				newAccount.Username=newAccountUsername;
				newAccount.Password=newAccountPassword;

				await AccountServices.CreateAccount(newAccount);
			}
			StateHasChanged();
		}

		private async void UsernameChange(ChangeEventArgs changeEvent) {
			List<string> usernames = await AccountServices.GetUsernames();

			newAccountUsername=(string)changeEvent.Value;

			isUsernameNew=true;

			foreach(string username in usernames) {
				if(username==newAccountUsername) {
					isUsernameNew=false;
				}
			}

			StateHasChanged();
		}
	}
}
