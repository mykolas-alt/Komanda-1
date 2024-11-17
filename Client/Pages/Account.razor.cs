using Microsoft.AspNetCore.Components;
using Projektas.Client.Interfaces;
using Projektas.Shared.Models;

namespace Projektas.Client.Pages {
	public partial class Account {
		private string? username=null;
		public string accountUsername { get; set; } = "";
		public string accountPassword { get; set; } = "";
		public string newAccountName { get; set; } = "";
		public string newAccountSurname { get; set; } = "";
		public string newAccountUsername { get; set; } = "";
		public string newAccountPassword { get; set; } = "";

		public string? token { get; private set; } = "";

		public bool isUsernameNew { get; private set; } = true;
		public bool isFieldsFilled { get; private set; } = true;
		public bool isNewFieldsFilled { get; private set; } = true;
		private bool test = false;

        [Inject]
        public IAccountAuthStateProvider AuthStateProvider { get; set; }
        [Inject]
        public IAccountService AccountServices { get; set; }

        public async void LogInEvent() {
			if(accountUsername==""||accountPassword=="") {
				isFieldsFilled = false;
			} else {
				User account = new User();
	
				isFieldsFilled=true;
	
				account.Username=accountUsername;
				account.Password=accountPassword;

				token = await AccountServices.LogIn(account);

				Console.WriteLine(token);

				if(!string.IsNullOrEmpty(token)) {
					await AuthStateProvider.MarkUserAsAuthenticated(token);
					Navigation.NavigateTo("/");
				}
			}
		}

		public void LogOffEvent() {
			AuthStateProvider.MarkUserAsLoggedOut();

			token="";

			Navigation.NavigateTo("/");
		}

		public async void SignUpEvent() {
			if(newAccountName==""||newAccountSurname==""||newAccountUsername==""||newAccountPassword=="") {
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

		public async void UsernameChange(ChangeEventArgs changeEvent) {
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

		protected override async Task OnInitializedAsync() {
			AuthStateProvider.AuthenticationStateChanged += OnAuthenticationStateChanged;

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
