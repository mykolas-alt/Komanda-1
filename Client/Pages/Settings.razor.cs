using Microsoft.AspNetCore.Components;
using Projektas.Client.Interfaces;

namespace Projektas.Client.Pages {
    partial class Settings {
        [Inject]
        public IAccountAuthStateProvider AuthStateProvider {get; set;}

        [Inject]
        public IAccountService AccountService {get; set;}

        private bool IsPrivateActive {get; set;} = false;

        public string? username = null;

        private void TogglePrivate() {
            IsPrivateActive = !IsPrivateActive;
            AccountService.ChangePrivateAsync(username, IsPrivateActive);
        }

        protected override async Task OnInitializedAsync() {
			AuthStateProvider.AuthenticationStateChanged += OnAuthenticationStateChangedAsync;

			await LoadUsernameAsync();
            IsPrivateActive = await AccountService.GetPrivateAsync(username);
		}

		private async Task LoadUsernameAsync() {
			username = await ((IAccountAuthStateProvider)AuthStateProvider).GetUsernameAsync();
			StateHasChanged();
		}

		private async void OnAuthenticationStateChangedAsync(Task<AuthenticationState> task) {
			await InvokeAsync(LoadUsernameAsync);
			StateHasChanged();
		}

        protected virtual void Dispose(bool disposing) {
            AuthStateProvider.AuthenticationStateChanged -= OnAuthenticationStateChangedAsync;
        }
    }
}
