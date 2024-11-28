using Microsoft.AspNetCore.Components;
using Projektas.Client.Interfaces;
using Projektas.Client.Services;
using Projektas.Shared.Models;

namespace Projektas.Client.Pages
{
    partial class Score : IDisposable
    {
        [Inject]
        public AccountScoreService accountScoreService { get; set; }
        [Inject]
        public IAccountAuthStateProvider AuthStateProvider { get; set; }

        public List<UserScoreDto> MathGameScores { get; set; }
        public List<UserScoreDto> AimTrainerScores { get; set; }
        public List<UserScoreDto> PairUpScores { get; set; }

        public int? MathGameHighscore { get; set; }
        public int? AimTrainerHighscore { get; set; }
        public int? PairUpHighscore { get; set; }

        public int? MathGameAllTimeAverage { get; set; }
        public int? AimTrainerAllTimeAverage { get; set; }
        public int? PairUpAllTimeAverage { get; set; }

        public string? username = null;

        protected override async Task OnInitializedAsync()
        {
            AuthStateProvider.AuthenticationStateChanged += OnAuthenticationStateChanged;

            await LoadUsernameAsync();
            if (username != null)
            {
                await LoadScoresAsync();
            }
        }

        private async Task LoadUsernameAsync()
        {
            username = await ((IAccountAuthStateProvider)AuthStateProvider).GetUsernameAsync();
            StateHasChanged();
        }

        private async Task LoadScoresAsync()
        {
            MathGameScores = await accountScoreService.GetUsersMathGameScoreAsync(username);
            AimTrainerScores = await accountScoreService.GetUsersAimTrainerScoreAsync(username);
            PairUpScores = await accountScoreService.GetUsersPairUpScoreAsync(username);

            MathGameHighscore = await accountScoreService.GetMathGameHighscoreAsync(username);
            AimTrainerHighscore = await accountScoreService.GetAimTrainerHighscoreAsync(username);
            PairUpHighscore = await accountScoreService.GetPairUpHighscoreAsync(username);

            MathGameAllTimeAverage = await accountScoreService.GetMathGameAllTimeAverageAsync(username);
            AimTrainerAllTimeAverage = await accountScoreService.GetAimTrainerAllTimeAverageAsync(username);
            PairUpAllTimeAverage = await accountScoreService.GetPairUpAllTimeAverageAsync(username);

            StateHasChanged();
        }

        private async void OnAuthenticationStateChanged(Task<AuthenticationState> task)
        {
            await InvokeAsync(LoadUsernameAsync);
            if (username != null)
            {
                await InvokeAsync(LoadScoresAsync);
            }
            StateHasChanged();
        }

        public void Dispose()
        {
            AuthStateProvider.AuthenticationStateChanged -= OnAuthenticationStateChanged;
        }
    }
}