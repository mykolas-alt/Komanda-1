using Microsoft.AspNetCore.Components;
using Projektas.Client.Interfaces;
using Projektas.Client.Services;
using Projektas.Shared.Models;

namespace Projektas.Client.Pages
{
    partial class Score
    {
        [Inject]
        public AccountScoreService accountScoreService { get; set; }
        [Inject]
        public IAccountAuthStateProvider AuthStateProvider { get; set; }

        public List<UserScoreDto> MathGameScores { get; set; }
        public List<UserScoreDto> AimTrainerScores { get; set; }
        public List<UserScoreDto> PairUpScores { get; set; }

        public string? username = null;
        protected override async Task OnInitializedAsync()
        {
            AuthStateProvider.AuthenticationStateChanged+=OnAuthenticationStateChanged;

            await LoadUsernameAsync();
            if (username != null)
            {
                MathGameScores = await accountScoreService.GetUsersMathGameScoreAsync(username);
                AimTrainerScores = await accountScoreService.GetUsersAimTrainerScoreAsync(username);
                PairUpScores = await accountScoreService.GetUsersPairUpScoreAsync(username);
            }
        }

        private async Task LoadUsernameAsync()
        {
            username=await ((IAccountAuthStateProvider)AuthStateProvider).GetUsernameAsync();
            StateHasChanged();
        }

        private async void OnAuthenticationStateChanged(Task<AuthenticationState> task)
        {
            await InvokeAsync(LoadUsernameAsync);
            StateHasChanged();
        }
    }
}
