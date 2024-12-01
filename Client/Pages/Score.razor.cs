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
        public int? AimTrainerHighscoreNormalMode { get; set; }
        public int? AimTrainerHighscoreHardMode { get; set; }
        public int? PairUpHighscoreNormalMode { get; set; }
        public int? PairUpHighscoreHardMode { get; set; }

        public int? MathGameAllTimeAverage { get; set; }
        public int? AimTrainerAllTimeAverageNormalMode { get; set; }
        public int? AimTrainerAllTimeAverageHardMode { get; set; }
        public int? PairUpAllTimeAverageNormalMode { get; set; }
        public int? PairUpAllTimeAverageHardMode { get; set; }
        public int MathGameMatchesPlayes { get; set; }
        public int AimTrainerTotalMatchesPlayed { get; set; }
        public int AimTrainerMatchesPlayedNormalMode { get; set; }
        public int AimTrainerMatchesPlayedHardMode { get; set; }
        public int PairUpTotalMatchesPlayed { get; set; }
        public int PairUpMatchesPlayedNormalMode { get; set; }
        public int PairUpMatchesPlayedHardMode { get; set; }

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

            MathGameMatchesPlayes = await accountScoreService.GetMathGameMatchesPlayedAsync(username);
            AimTrainerTotalMatchesPlayed = await accountScoreService.GetTotalAimTrainerMatchesPlayedAsync(username);
            AimTrainerMatchesPlayedNormalMode = await accountScoreService.GetAimTrainerMatchesPlayedNormalModeAsync(username);
            AimTrainerMatchesPlayedHardMode = await accountScoreService.GetAimTrainerMatchesPlayedHardModeAsync(username);
            PairUpTotalMatchesPlayed = await accountScoreService.GetTotalPairUpMatchesPlayedAsync(username);
            PairUpMatchesPlayedNormalMode = await accountScoreService.GetPairUpMatchesPlayedNormalModeAsync(username);
            PairUpMatchesPlayedHardMode = await accountScoreService.GetPairUpMatchesPlayedHardModeAsync(username);


            MathGameHighscore = await accountScoreService.GetMathGameHighscoreAsync(username);
            AimTrainerHighscoreNormalMode = await accountScoreService.GetAimTrainerHighscoreNormalModeAsync(username);
            AimTrainerHighscoreHardMode = await accountScoreService.GetAimTrainerHighscoreHardModeAsync(username);

            PairUpHighscoreNormalMode = await accountScoreService.GetPairUpHighscoreNormalModeAsync(username);
            PairUpHighscoreHardMode = await accountScoreService.GetPairUpHighscoreHardModeAsync(username);

            MathGameAllTimeAverage = await accountScoreService.GetMathGameAllTimeAverageAsync(username);
            AimTrainerAllTimeAverageNormalMode = await accountScoreService.GetAimTrainerAllTimeAverageNormalModeAsync(username);
            AimTrainerAllTimeAverageHardMode = await accountScoreService.GetAimTrainerAllTimeAverageHardModeAsync(username);

            PairUpAllTimeAverageNormalMode = await accountScoreService.GetPairUpAllTimeAverageNormalModeAsync(username);
            PairUpAllTimeAverageHardMode = await accountScoreService.GetPairUpAllTimeAverageHardModeAsync(username);


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