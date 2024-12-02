using Microsoft.AspNetCore.Components;
using Projektas.Client.Components;
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
        public List<AverageScoreDto> MathGameAverageScoreLast7Days { get; set; }
        public List<AverageScoreDto> AimTrainerAverageScoreLast7DaysNormalMode { get; set; }
        public List<AverageScoreDto> AimTrainerAverageScoreLast7DaysHardMode { get; set; }
        public List<AverageScoreDto> PairUpAverageScoreLast7DaysNormalMode { get; set; }
        public List<AverageScoreDto> PairUpAverageScoreLast7DaysHardMode { get; set; }

        private string activeTab = "MathGame";
        public Dataset[] MathGameAverageScoreLast7DaysDataset { get; set; }
        public Dataset[] AimTrainerAverageScoreLast7DaysDataset { get; set; }
        public Dataset[] PairUpAverageScoreLast7DaysDataset { get; set; }

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

            MathGameAverageScoreLast7Days = await accountScoreService.GetMathGameAverageScoreLast7Days(username);
            AimTrainerAverageScoreLast7DaysNormalMode = await accountScoreService.GetAimTrainerAverageScoreLast7DaysNormalMode(username);
            AimTrainerAverageScoreLast7DaysHardMode = await accountScoreService.GetAimTrainerAverageScoreLast7DaysHardMode(username);
            PairUpAverageScoreLast7DaysNormalMode = await accountScoreService.GetPairUpAverageScoreLast7DaysNormalMode(username);
            PairUpAverageScoreLast7DaysHardMode = await accountScoreService.GetPairUpAverageScoreLast7DaysHardMode(username);
            LoadDataSets();
            StateHasChanged();
        }

        private void LoadDataSets()
        {
            MathGameAverageScoreLast7DaysDataset = new Dataset[]
            {
                new Dataset
                {
                    Label = "Scores",
                    Data = MathGameAverageScoreLast7Days.Select(s => s.AverageScore).ToArray(),
                    BorderColor = "rgba(75, 192, 192, 1)" // Green
                }
            };

            AimTrainerAverageScoreLast7DaysDataset = new Dataset[]
            {
                new Dataset
                {
                    Label = "Normal mode",
                    Data = AimTrainerAverageScoreLast7DaysNormalMode.Select(s => s.AverageScore).ToArray(),
                    BorderColor = "rgba(54, 162, 235, 1)" // Blue
                },
                new Dataset
                {
                    Label = "Hard mode",
                    Data = AimTrainerAverageScoreLast7DaysHardMode.Select(s => s.AverageScore).ToArray(),
                    BorderColor = "rgba(255, 99, 132, 1)" // Red
                }
            };

            PairUpAverageScoreLast7DaysDataset = new Dataset[]
            {
                new Dataset
                {
                    Label = "Normal mode",
                    Data = PairUpAverageScoreLast7DaysNormalMode.Select(s => s.AverageScore).ToArray(),
                    BorderColor = "rgba(153, 102, 255, 1)" // Purple
                },
                new Dataset
                {
                    Label = "Hard mode",
                    Data = PairUpAverageScoreLast7DaysHardMode.Select(s => s.AverageScore).ToArray(),
                    BorderColor = "rgba(255, 159, 64, 1)" // Orange
                }
            };
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

        private void SetActiveTab(string tabName)
        {
            activeTab = tabName;
        }

        public void Dispose()
        {
            AuthStateProvider.AuthenticationStateChanged -= OnAuthenticationStateChanged;
        }
    }
}