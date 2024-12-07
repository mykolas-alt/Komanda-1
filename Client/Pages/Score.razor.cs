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

        public List<UserScoreDto<MathGameData>> MathGameScores { get; set; }
        public List<UserScoreDto<AimTrainerData>> AimTrainerScores { get; set; }
        public List<UserScoreDto<PairUpData>> PairUpScores { get; set; }

        public int? MathGameHighscore { get; set; }
        public GameScore AimTrainerHighscoreNormalMode { get; set; }
        public GameScore AimTrainerHighscoreHardMode { get; set; }
        public GameScore PairUpHighscoreEasyMode { get; set; }
        public GameScore PairUpHighscoreMediumMode { get; set; }
        public GameScore PairUpHighscoreHardMode { get; set; }

        public int? MathGameAllTimeAverage { get; set; }
        public GameScore AimTrainerAllTimeAverageNormalMode { get; set; }
        public GameScore AimTrainerAllTimeAverageHardMode { get; set; }
        public GameScore PairUpAllTimeAverageEasyMode { get; set; }
        public GameScore PairUpAllTimeAverageMediumMode { get; set; }
        public GameScore PairUpAllTimeAverageHardMode { get; set; }
        public int MathGameMatchesPlayes { get; set; }
        public int AimTrainerMatchesPlayedNormalMode { get; set; }
        public int AimTrainerMatchesPlayedHardMode { get; set; }
        public int PairUpMatchesPlayedEasyMode { get; set; }
        public int PairUpMatchesPlayedMediumMode { get; set; }
        public int PairUpMatchesPlayedHardMode { get; set; }
        public List<AverageScoreDto> MathGameAverageScoreLast7Days { get; set; }
        public List<AverageScoreDto> AimTrainerAverageScoreLast7DaysNormalMode { get; set; }
        public List<AverageScoreDto> AimTrainerAverageScoreLast7DaysHardMode { get; set; }
        public List<AverageScoreDto> PairUpAverageScoreLast7DaysEasyMode { get; set; }
        public List<AverageScoreDto> PairUpAverageScoreLast7DaysMediumMode { get; set; }
        public List<AverageScoreDto> PairUpAverageScoreLast7DaysHardMode { get; set; }

        private string activeTab = "MathGame";
        public Dataset[] MathGameAverageScoreLast7DaysDataset { get; set; }
        public Dataset[] AimTrainerAverageScoreLast7DaysDataset { get; set; }
        public Dataset[] PairUpAverageScoreLast7DaysDataset { get; set; }
        public Dataset[] PairUpAverageTimeSpentLast7DaysDataset { get; set; }

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
            AimTrainerMatchesPlayedNormalMode = await accountScoreService.GetAimTrainerMatchesPlayedNormalModeAsync(username);
            AimTrainerMatchesPlayedHardMode = await accountScoreService.GetAimTrainerMatchesPlayedHardModeAsync(username);

            PairUpMatchesPlayedEasyMode = await accountScoreService.GetPairUpMatchesPlayedEasyModeAsync(username);
            PairUpMatchesPlayedMediumMode = await accountScoreService.GetPairUpMatchesPlayedMediumModeAsync(username);
            PairUpMatchesPlayedHardMode = await accountScoreService.GetPairUpMatchesPlayedHardModeAsync(username);


            MathGameHighscore = await accountScoreService.GetMathGameHighscoreAsync(username);
            AimTrainerHighscoreNormalMode = await accountScoreService.GetAimTrainerHighscoreNormalModeAsync(username);
            AimTrainerHighscoreHardMode = await accountScoreService.GetAimTrainerHighscoreHardModeAsync(username);

            PairUpHighscoreEasyMode = await accountScoreService.GetPairUpHighscoreEasyModeAsync(username);
            PairUpHighscoreMediumMode = await accountScoreService.GetPairUpHighscoreMediumModeAsync(username);
            PairUpHighscoreHardMode = await accountScoreService.GetPairUpHighscoreHardModeAsync(username);

            MathGameAllTimeAverage = await accountScoreService.GetMathGameAllTimeAverageAsync(username);
            AimTrainerAllTimeAverageNormalMode = await accountScoreService.GetAimTrainerAllTimeAverageNormalModeAsync(username);
            AimTrainerAllTimeAverageHardMode = await accountScoreService.GetAimTrainerAllTimeAverageHardModeAsync(username);

            PairUpAllTimeAverageEasyMode = await accountScoreService.GetPairUpAllTimeAverageEasyModeAsync(username);
            PairUpAllTimeAverageMediumMode = await accountScoreService.GetPairUpAllTimeAverageMediumModeAsync(username);
            PairUpAllTimeAverageHardMode = await accountScoreService.GetPairUpAllTimeAverageHardModeAsync(username);

            MathGameAverageScoreLast7Days = await accountScoreService.GetMathGameAverageScoreLast7Days(username);
            AimTrainerAverageScoreLast7DaysNormalMode = await accountScoreService.GetAimTrainerAverageScoreLast7DaysNormalMode(username);
            AimTrainerAverageScoreLast7DaysHardMode = await accountScoreService.GetAimTrainerAverageScoreLast7DaysHardMode(username);
            PairUpAverageScoreLast7DaysEasyMode = await accountScoreService.GetPairUpAverageScoreLast7DaysEasyMode(username);
            PairUpAverageScoreLast7DaysMediumMode = await accountScoreService.GetPairUpAverageScoreLast7DaysMediumMode(username);
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
                    Data = MathGameAverageScoreLast7Days.Select(s => s.Score.Scores).ToArray(),
                    BorderColor = "rgba(75, 192, 192, 1)" // Green
                }
            };

            AimTrainerAverageScoreLast7DaysDataset = new Dataset[]
            {
                new Dataset
                {
                    Label = "Normal difficulty",
                    Data = AimTrainerAverageScoreLast7DaysNormalMode.Select(s => s.Score.Scores).ToArray(),
                    BorderColor = "rgba(54, 162, 235, 1)" // Blue
                },
                new Dataset
                {
                    Label = "Hard difficulty",
                    Data = AimTrainerAverageScoreLast7DaysHardMode.Select(s => s.Score.Scores).ToArray(),
                    BorderColor = "rgba(255, 99, 132, 1)" // Red
                }
            };

            PairUpAverageScoreLast7DaysDataset = new Dataset[]
            {
                new Dataset
                {
                    Label = "Easy difficulty",
                    Data = PairUpAverageScoreLast7DaysEasyMode.Select(s => s.Score.Scores).ToArray(),
                    BorderColor = "rgba(255, 206, 86, 1)" // Yellow
                },
                new Dataset
                {
                    Label = "Medium difficulty",
                    Data = PairUpAverageScoreLast7DaysMediumMode.Select(s => s.Score.Scores).ToArray(),
                    BorderColor = "rgba(153, 102, 255, 1)" // Purple
                },
                new Dataset
                {
                    Label = "Hard difficulty",
                    Data = PairUpAverageScoreLast7DaysHardMode.Select(s => s.Score.Scores).ToArray(),
                    BorderColor = "rgba(255, 159, 64, 1)" // Orange
                }
            };

            PairUpAverageTimeSpentLast7DaysDataset = new Dataset[]
            {
                new Dataset
                {
                    Label = "Easy difficulty",
                    Data = PairUpAverageScoreLast7DaysEasyMode.Select(s => s.Score.TimeSpent ?? 0).ToArray(),
                    BorderColor = "rgba(75, 192, 192, 1)" // Green
                },
                new Dataset
                {
                    Label = "Medium difficulty",
                    Data = PairUpAverageScoreLast7DaysMediumMode.Select(s => s.Score.TimeSpent ?? 0).ToArray(),
                    BorderColor = "rgba(255, 99, 132, 1)" // Red
                },
                new Dataset
                {
                    Label = "Hard difficulty",
                    Data = PairUpAverageScoreLast7DaysHardMode.Select(s => s.Score.TimeSpent ?? 0).ToArray(),
                    BorderColor = "rgba(54, 162, 235, 1)" // Blue
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