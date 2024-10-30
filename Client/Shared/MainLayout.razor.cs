using Projektas.Client.Services;

namespace Projektas.Client.Shared {
	public partial class MainLayout {
		public string? username="";

		protected override async Task OnInitializedAsync() {
			AuthStateProvider.AuthenticationStateChanged += OnAuthenticationStateChanged;

			await LoadUsernameAsync();
		}

		private async Task LoadUsernameAsync() {
			username = await ((AccountAuthStateProvider)AuthStateProvider).GetUsernameAsync();
			StateHasChanged();
		}

		private async void OnAuthenticationStateChanged(Task<AuthenticationState> task) {
			await InvokeAsync(LoadUsernameAsync);
		}

		public void Dispose() {
			AuthStateProvider.AuthenticationStateChanged -= OnAuthenticationStateChanged;
		}
	}
}
