using Microsoft.AspNetCore.Components;
using Projektas.Client.Components;
using Projektas.Client.Interfaces;
using Projektas.Shared.Enums;
using Projektas.Shared.Models;

namespace Projektas.Client.Pages
{
    partial class Score : IDisposable
    {
        [Inject]
        public required IAccountScoreService accountScoreService { get; set; }
        [Inject]
        public required IAccountAuthStateProvider AuthStateProvider { get; set; }

        public required List<UserScoreDto<MathGameData>> MathGameScores { get; set; }
        public required List<UserScoreDto<AimTrainerData>> AimTrainerScores { get; set; }
        public required List<UserScoreDto<PairUpData>> PairUpScores { get; set; }
        public required List<UserScoreDto<SudokuData>> SudokuScores { get; set; }

        public required GameScore MathGameHighscore { get; set; }

        public required GameScore AimTrainerHighscoreNormalMode { get; set; }
        public required GameScore AimTrainerHighscoreHardMode { get; set; }

        public required GameScore PairUpHighscoreEasyMode { get; set; }
        public required GameScore PairUpHighscoreMediumMode { get; set; }
        public required GameScore PairUpHighscoreHardMode { get; set; }

        public required GameScore SudokuHighscoreEasyMode4x4 { get; set; }
        public required GameScore SudokuHighscoreMediumMode4x4 { get; set; }
        public required GameScore SudokuHighscoreHardMode4x4 { get; set; }

        public required GameScore SudokuHighscoreEasyMode9x9 { get; set; }
        public required GameScore SudokuHighscoreMediumMode9x9 { get; set; }
        public required GameScore SudokuHighscoreHardMode9x9 { get; set; }

        public required GameScore SudokuHighscoreEasyMode16x16 { get; set; }
        public required GameScore SudokuHighscoreMediumMode16x16 { get; set; }
        public required GameScore SudokuHighscoreHardMode16x16 { get; set; }

        public required GameScore MathGameAllTimeAverage { get; set; }

        public required GameScore AimTrainerAllTimeAverageNormalMode { get; set; }
        public required GameScore AimTrainerAllTimeAverageHardMode { get; set; }

        public required GameScore PairUpAllTimeAverageEasyMode { get; set; }
        public required GameScore PairUpAllTimeAverageMediumMode { get; set; }
        public required GameScore PairUpAllTimeAverageHardMode { get; set; }

        public required GameScore SudokuAllTimeAverageEasyMode4x4 { get; set; }
        public required GameScore SudokuAllTimeAverageMediumMode4x4 { get; set; }
        public required GameScore SudokuAllTimeAverageHardMode4x4 { get; set; }

        public required GameScore SudokuAllTimeAverageEasyMode9x9 { get; set; }
        public required GameScore SudokuAllTimeAverageMediumMode9x9 { get; set; }
        public required GameScore SudokuAllTimeAverageHardMode9x9 { get; set; }

        public required GameScore SudokuAllTimeAverageEasyMode16x16 { get; set; }
        public required GameScore SudokuAllTimeAverageMediumMode16x16 { get; set; }
        public required GameScore SudokuAllTimeAverageHardMode16x16 { get; set; }

        public int MathGameMatchesPlayes { get; set; }

        public int AimTrainerMatchesPlayedNormalMode { get; set; }
        public int AimTrainerMatchesPlayedHardMode { get; set; }

        public int PairUpMatchesPlayedEasyMode { get; set; }
        public int PairUpMatchesPlayedMediumMode { get; set; }
        public int PairUpMatchesPlayedHardMode { get; set; }

        public int SudokuMatchesPlayedEasyMode4x4 { get; set; }
        public int SudokuMatchesPlayedMediumMode4x4 { get; set; }
        public int SudokuMatchesPlayedHardMode4x4 { get; set; }

        public int SudokuMatchesPlayedEasyMode9x9 { get; set; }
        public int SudokuMatchesPlayedMediumMode9x9 { get; set; }
        public int SudokuMatchesPlayedHardMode9x9 { get; set; }

        public int SudokuMatchesPlayedEasyMode16x16 { get; set; }
        public int SudokuMatchesPlayedMediumMode16x16 { get; set; }
        public int SudokuMatchesPlayedHardMode16x16 { get; set; }

        public required List<AverageScoreDto> MathGameAverageScoreLast7Days { get; set; }

        public required List<AverageScoreDto> AimTrainerAverageScoreLast7DaysNormalMode { get; set; }
        public required List<AverageScoreDto> AimTrainerAverageScoreLast7DaysHardMode { get; set; }

        public required List<AverageScoreDto> PairUpAverageScoreLast7DaysEasyMode { get; set; }
        public required List<AverageScoreDto> PairUpAverageScoreLast7DaysMediumMode { get; set; }
        public required List<AverageScoreDto> PairUpAverageScoreLast7DaysHardMode { get; set; }

        public required List<AverageScoreDto> SudokuAverageScoreLast7DaysEasyMode4x4 { get; set; }
        public required List<AverageScoreDto> SudokuAverageScoreLast7DaysMediumMode4x4 { get; set; }
        public required List<AverageScoreDto> SudokuAverageScoreLast7DaysHardMode4x4 { get; set; }

        public required List<AverageScoreDto> SudokuAverageScoreLast7DaysEasyMode9x9 { get; set; }
        public required List<AverageScoreDto> SudokuAverageScoreLast7DaysMediumMode9x9 { get; set; }
        public required List<AverageScoreDto> SudokuAverageScoreLast7DaysHardMode9x9 { get; set; }

        public required List<AverageScoreDto> SudokuAverageScoreLast7DaysEasyMode16x16 { get; set; }
        public required List<AverageScoreDto> SudokuAverageScoreLast7DaysMediumMode16x16 { get; set; }
        public required List<AverageScoreDto> SudokuAverageScoreLast7DaysHardMode16x16 { get; set; }
        
        public required Dataset[] MathGameAverageScoreLast7DaysDataset { get; set; }

        public required Dataset[] AimTrainerAverageScoreLast7DaysDataset { get; set; }

        public required Dataset[] PairUpAverageScoreLast7DaysDataset { get; set; }
        public required Dataset[] PairUpAverageTimeSpentLast7DaysDataset { get; set; }

        public required Dataset[] SudokuAverageTimeSpentIn4x4 { get; set; }
        public required Dataset[] SudokuAverageTimeSpentIn9x9 { get; set; }
        public required Dataset[] SudokuAverageTimeSpentIn16x16 { get; set; }

        private string activeTab = "MathGame";

        public string? username = null;

        private bool IsActive {get; set;} = false;

        private void ToggleGameInfo() {
            IsActive = !IsActive;
        }
        protected override async Task OnInitializedAsync()
        {
            AuthStateProvider.AuthenticationStateChanged += OnAuthenticationStateChanged;

            await LoadUsernameAsync();
            if (username != null)
            {
                await LoadScoresAsync();
            }
        }

        public async Task LoadUsernameAsync()
        {
            username = await ((IAccountAuthStateProvider)AuthStateProvider).GetUsernameAsync();
            StateHasChanged();
        }

        public async Task LoadScoresAsync()
        {
            await LoadMathGameScores();
            await LoadAimTrainerScores();
            await LoadPairUpScores();
            await LoadSudokuScores();

            LoadMathGameDatasets();
            LoadAimTrainerDatasets();
            LoadPairUpDatasets();
            LoadSudokuDatasets();

            StateHasChanged();
        }

        public async Task LoadMathGameScores()
        {
            // last 10 games
            MathGameScores = await accountScoreService.GetMathGameScoresAsync(username);

            // matches played
            MathGameMatchesPlayes = await accountScoreService.GetMathGameMatchesPlayedAsync(username);

            // highscore
            MathGameHighscore = await accountScoreService.GetMathGameHighscoreAsync(username);

            // average score
            MathGameAllTimeAverage = await accountScoreService.GetMathGameAverageScoreAsync(username);

            // average score for the last 7 days
            MathGameAverageScoreLast7Days = await accountScoreService.GetMathGameAverageScoreLast7DaysAsync(username);
        }

        public async Task LoadAimTrainerScores()
        {
            // last 10 games
            AimTrainerScores = await accountScoreService.GetAimTrainerScoresAsync(username);

            // matches played
            AimTrainerMatchesPlayedNormalMode = await accountScoreService.GetAimTrainerMatchesPlayedAsync(username, GameDifficulty.Normal);
            AimTrainerMatchesPlayedHardMode = await accountScoreService.GetAimTrainerMatchesPlayedAsync(username, GameDifficulty.Hard);
            
            // highscore
            AimTrainerHighscoreNormalMode = await accountScoreService.GetAimTrainerHighscoreAsync(username, GameDifficulty.Normal);
            AimTrainerHighscoreHardMode = await accountScoreService.GetAimTrainerHighscoreAsync(username, GameDifficulty.Hard);

            // average scores
            AimTrainerAllTimeAverageNormalMode = await accountScoreService.GetAimTrainerAverageScoreAsync(username, GameDifficulty.Normal);
            AimTrainerAllTimeAverageHardMode = await accountScoreService.GetAimTrainerAverageScoreAsync(username, GameDifficulty.Hard);

            // average scores for the last 7 days
            AimTrainerAverageScoreLast7DaysNormalMode = await accountScoreService.GetAimTrainerAverageScoreLast7DaysAsync(username, GameDifficulty.Normal);
            AimTrainerAverageScoreLast7DaysHardMode = await accountScoreService.GetAimTrainerAverageScoreLast7DaysAsync(username, GameDifficulty.Hard);
        }

        public async Task LoadPairUpScores()
        {
            // last 10 games
            PairUpScores = await accountScoreService.GetPairUpScoresAsync(username);

            // matches played
            PairUpMatchesPlayedEasyMode = await accountScoreService.GetPairUpMatchesPlayedAsync(username, GameDifficulty.Easy);
            PairUpMatchesPlayedMediumMode = await accountScoreService.GetPairUpMatchesPlayedAsync(username, GameDifficulty.Medium);
            PairUpMatchesPlayedHardMode = await accountScoreService.GetPairUpMatchesPlayedAsync(username, GameDifficulty.Hard);

            // highscore
            PairUpHighscoreEasyMode = await accountScoreService.GetPairUpHighscoreAsync(username, GameDifficulty.Easy);
            PairUpHighscoreMediumMode = await accountScoreService.GetPairUpHighscoreAsync(username, GameDifficulty.Medium);
            PairUpHighscoreHardMode = await accountScoreService.GetPairUpHighscoreAsync(username, GameDifficulty.Hard);

            // average scores
            PairUpAllTimeAverageEasyMode = await accountScoreService.GetPairUpAverageScoreAsync(username, GameDifficulty.Easy);
            PairUpAllTimeAverageMediumMode = await accountScoreService.GetPairUpAverageScoreAsync(username, GameDifficulty.Medium);
            PairUpAllTimeAverageHardMode = await accountScoreService.GetPairUpAverageScoreAsync(username, GameDifficulty.Hard);

            // average scores for the last 7 days
            PairUpAverageScoreLast7DaysEasyMode = await accountScoreService.GetPairUpAverageScoreLast7DaysAsync(username, GameDifficulty.Easy);
            PairUpAverageScoreLast7DaysMediumMode = await accountScoreService.GetPairUpAverageScoreLast7DaysAsync(username, GameDifficulty.Medium);
            PairUpAverageScoreLast7DaysHardMode = await accountScoreService.GetPairUpAverageScoreLast7DaysAsync(username, GameDifficulty.Hard);
        }

        public async Task LoadSudokuScores()
        {
            // last 10 games
            SudokuScores = await accountScoreService.GetSudokuScoresAsync(username);

            // Matches played
            SudokuMatchesPlayedEasyMode4x4 = await accountScoreService.GetSudokuMatchesPlayedAsync(username, GameDifficulty.Easy, GameMode.FourByFour);
            SudokuMatchesPlayedMediumMode4x4 = await accountScoreService.GetSudokuMatchesPlayedAsync(username, GameDifficulty.Medium, GameMode.FourByFour);
            SudokuMatchesPlayedHardMode4x4 = await accountScoreService.GetSudokuMatchesPlayedAsync(username, GameDifficulty.Hard, GameMode.FourByFour);

            SudokuMatchesPlayedEasyMode9x9 = await accountScoreService.GetSudokuMatchesPlayedAsync(username, GameDifficulty.Easy, GameMode.NineByNine);
            SudokuMatchesPlayedMediumMode9x9 = await accountScoreService.GetSudokuMatchesPlayedAsync(username, GameDifficulty.Medium, GameMode.NineByNine);
            SudokuMatchesPlayedHardMode9x9 = await accountScoreService.GetSudokuMatchesPlayedAsync(username, GameDifficulty.Hard, GameMode.NineByNine);

            SudokuMatchesPlayedEasyMode16x16 = await accountScoreService.GetSudokuMatchesPlayedAsync(username, GameDifficulty.Easy, GameMode.SixteenBySixteen);
            SudokuMatchesPlayedMediumMode16x16 = await accountScoreService.GetSudokuMatchesPlayedAsync(username, GameDifficulty.Medium, GameMode.SixteenBySixteen);
            SudokuMatchesPlayedHardMode16x16 = await accountScoreService.GetSudokuMatchesPlayedAsync(username, GameDifficulty.Hard, GameMode.SixteenBySixteen);

            // Higscore
            SudokuHighscoreEasyMode4x4 = await accountScoreService.GetSudokuHighscoreAsync(username, GameDifficulty.Easy, GameMode.FourByFour);
            SudokuHighscoreMediumMode4x4 = await accountScoreService.GetSudokuHighscoreAsync(username, GameDifficulty.Medium, GameMode.FourByFour);
            SudokuHighscoreHardMode4x4 = await accountScoreService.GetSudokuHighscoreAsync(username, GameDifficulty.Hard, GameMode.FourByFour);

            SudokuHighscoreEasyMode9x9 = await accountScoreService.GetSudokuHighscoreAsync(username, GameDifficulty.Easy, GameMode.NineByNine);
            SudokuHighscoreMediumMode9x9 = await accountScoreService.GetSudokuHighscoreAsync(username, GameDifficulty.Medium, GameMode.NineByNine);
            SudokuHighscoreHardMode9x9 = await accountScoreService.GetSudokuHighscoreAsync(username, GameDifficulty.Hard, GameMode.NineByNine);

            SudokuHighscoreEasyMode16x16 = await accountScoreService.GetSudokuHighscoreAsync(username, GameDifficulty.Easy, GameMode.SixteenBySixteen);
            SudokuHighscoreMediumMode16x16 = await accountScoreService.GetSudokuHighscoreAsync(username, GameDifficulty.Medium, GameMode.SixteenBySixteen);
            SudokuHighscoreHardMode16x16 = await accountScoreService.GetSudokuHighscoreAsync(username, GameDifficulty.Hard, GameMode.SixteenBySixteen);

            // Average scores
            SudokuAllTimeAverageEasyMode4x4 = await accountScoreService.GetSudokuAverageScoreAsync(username, GameDifficulty.Easy, GameMode.FourByFour);
            SudokuAllTimeAverageMediumMode4x4 = await accountScoreService.GetSudokuAverageScoreAsync(username, GameDifficulty.Medium, GameMode.FourByFour);
            SudokuAllTimeAverageHardMode4x4 = await accountScoreService.GetSudokuAverageScoreAsync(username, GameDifficulty.Hard, GameMode.FourByFour);

            SudokuAllTimeAverageEasyMode9x9 = await accountScoreService.GetSudokuAverageScoreAsync(username, GameDifficulty.Easy, GameMode.NineByNine);
            SudokuAllTimeAverageMediumMode9x9 = await accountScoreService.GetSudokuAverageScoreAsync(username, GameDifficulty.Medium, GameMode.NineByNine);
            SudokuAllTimeAverageHardMode9x9 = await accountScoreService.GetSudokuAverageScoreAsync(username, GameDifficulty.Hard, GameMode.NineByNine);

            SudokuAllTimeAverageEasyMode16x16 = await accountScoreService.GetSudokuAverageScoreAsync(username, GameDifficulty.Easy, GameMode.SixteenBySixteen);
            SudokuAllTimeAverageMediumMode16x16 = await accountScoreService.GetSudokuAverageScoreAsync(username, GameDifficulty.Medium, GameMode.SixteenBySixteen);
            SudokuAllTimeAverageHardMode16x16 = await accountScoreService.GetSudokuAverageScoreAsync(username, GameDifficulty.Hard, GameMode.SixteenBySixteen);

            // Average scores in last 7 days
            SudokuAverageScoreLast7DaysEasyMode4x4 = await accountScoreService.GetSudokuAverageScoreLast7DaysAsync(username, GameDifficulty.Easy, GameMode.FourByFour);
            SudokuAverageScoreLast7DaysMediumMode4x4 = await accountScoreService.GetSudokuAverageScoreLast7DaysAsync(username, GameDifficulty.Medium, GameMode.FourByFour);
            SudokuAverageScoreLast7DaysHardMode4x4 = await accountScoreService.GetSudokuAverageScoreLast7DaysAsync(username, GameDifficulty.Hard, GameMode.FourByFour);

            SudokuAverageScoreLast7DaysEasyMode9x9 = await accountScoreService.GetSudokuAverageScoreLast7DaysAsync(username, GameDifficulty.Easy, GameMode.NineByNine);
            SudokuAverageScoreLast7DaysMediumMode9x9 = await accountScoreService.GetSudokuAverageScoreLast7DaysAsync(username, GameDifficulty.Medium, GameMode.NineByNine);
            SudokuAverageScoreLast7DaysHardMode9x9 = await accountScoreService.GetSudokuAverageScoreLast7DaysAsync(username, GameDifficulty.Hard, GameMode.NineByNine);

            SudokuAverageScoreLast7DaysEasyMode16x16 = await accountScoreService.GetSudokuAverageScoreLast7DaysAsync(username, GameDifficulty.Easy, GameMode.SixteenBySixteen);
            SudokuAverageScoreLast7DaysMediumMode16x16 = await accountScoreService.GetSudokuAverageScoreLast7DaysAsync(username, GameDifficulty.Medium, GameMode.SixteenBySixteen);
            SudokuAverageScoreLast7DaysHardMode16x16 = await accountScoreService.GetSudokuAverageScoreLast7DaysAsync(username, GameDifficulty.Hard, GameMode.SixteenBySixteen);
        }

        public void LoadMathGameDatasets()
        {
            MathGameAverageScoreLast7DaysDataset = new Dataset[]
            {
                new Dataset
                {
                    Label = "Scores",
                    Data = MathGameAverageScoreLast7Days.Select(s => s.Score.Scores ?? 0).ToArray(),
                    BorderColor = "rgba(75, 192, 192, 1)", // Green
                    yAxisLabel = "Points"
                }
            };
        }

        public void LoadAimTrainerDatasets()
        {
            AimTrainerAverageScoreLast7DaysDataset = new Dataset[]
            {
                new Dataset
                {
                    Label = "Normal difficulty",
                    Data = AimTrainerAverageScoreLast7DaysNormalMode.Select(s => s.Score.Scores ?? 0).ToArray(),
                    BorderColor = "rgba(54, 162, 235, 1)", // Blue
                    yAxisLabel = "Points"
                },
                new Dataset
                {
                    Label = "Hard difficulty",
                    Data = AimTrainerAverageScoreLast7DaysHardMode.Select(s => s.Score.Scores ?? 0).ToArray(),
                    BorderColor = "rgba(255, 99, 132, 1)" // Red
                }
            };
        }

        public void LoadPairUpDatasets()
        {
            PairUpAverageScoreLast7DaysDataset = new Dataset[]
            {
                new Dataset
                {
                    Label = "Easy difficulty",
                    Data = PairUpAverageScoreLast7DaysEasyMode.Select(s => s.Score.Scores ?? 0).ToArray(),
                    BorderColor = "rgba(255, 206, 86, 1)", // Yellow
                    yAxisLabel = "Mistakes"
                },
                new Dataset
                {
                    Label = "Medium difficulty",
                    Data = PairUpAverageScoreLast7DaysMediumMode.Select(s => s.Score.Scores ?? 0).ToArray(),
                    BorderColor = "rgba(153, 102, 255, 1)" // Purple
                },
                new Dataset
                {
                    Label = "Hard difficulty",
                    Data = PairUpAverageScoreLast7DaysHardMode.Select(s => s.Score.Scores ?? 0).ToArray(),
                    BorderColor = "rgba(255, 159, 64, 1)" // Orange
                }
            };

            PairUpAverageTimeSpentLast7DaysDataset = new Dataset[]
            {
                new Dataset
                {
                    Label = "Easy difficulty",
                    Data = PairUpAverageScoreLast7DaysEasyMode.Select(s => s.Score.TimeSpent ?? 0).ToArray(),
                    BorderColor = "rgba(75, 192, 192, 1)", // Green
                    yAxisLabel = "Solution time"
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
                    BorderColor = "rgba(255, 159, 64, 1)" // Orange
                }
            };
        }

        public void LoadSudokuDatasets()
        {
            SudokuAverageTimeSpentIn4x4 = new Dataset[]
            {
                new Dataset
                {
                    Label = "Easy difficulty",
                    Data = SudokuAverageScoreLast7DaysEasyMode4x4.Select(s => s.Score.TimeSpent ?? 0).ToArray(),
                    BorderColor = "rgba(255, 165, 0, 1)", // Orange
                    yAxisLabel = "Solution Time"
                },
                new Dataset
                {
                    Label = "Medium difficulty",
                    Data = SudokuAverageScoreLast7DaysMediumMode4x4.Select(s => s.Score.TimeSpent ?? 0).ToArray(),
                    BorderColor = "rgba(75, 0, 130, 1)" // Indigo
                },
                new Dataset
                {
                    Label = "Hard difficulty",
                    Data = SudokuAverageScoreLast7DaysHardMode4x4.Select(s => s.Score.TimeSpent ?? 0).ToArray(),
                    BorderColor = "rgba(255, 20, 147, 1)" // Deep Pink
                }
            };

            SudokuAverageTimeSpentIn9x9 = new Dataset[]
            {
                new Dataset
                {
                    Label = "Easy difficulty",
                    Data = SudokuAverageScoreLast7DaysEasyMode9x9.Select(s => s.Score.TimeSpent ?? 0).ToArray(),
                    BorderColor = "rgba(238, 130, 238, 1)", // Pink
                    yAxisLabel = "Solution time"
                },
                new Dataset
                {
                    Label = "Medium difficulty",
                    Data = SudokuAverageScoreLast7DaysMediumMode9x9.Select(s => s.Score.TimeSpent ?? 0).ToArray(),
                    BorderColor = "rgba(60, 179, 113, 1)" // Green
                },
                new Dataset
                {
                    Label = "Hard difficulty",
                    Data = SudokuAverageScoreLast7DaysHardMode9x9.Select(s => s.Score.TimeSpent ?? 0).ToArray(),
                    BorderColor = "rgba(255, 165, 0, 1)" // Yellow
                }
            };

            SudokuAverageTimeSpentIn16x16 = new Dataset[]
            {
                new Dataset
                {
                    Label = "Easy difficulty",
                    Data = SudokuAverageScoreLast7DaysEasyMode16x16.Select(s => s.Score.TimeSpent ?? 0).ToArray(),
                    BorderColor = "rgba(75, 192, 192, 1)", // Green
                    yAxisLabel = "Solution Time"
                },
                new Dataset
                {
                    Label = "Medium difficulty",
                    Data = SudokuAverageScoreLast7DaysMediumMode16x16.Select(s => s.Score.TimeSpent ?? 0).ToArray(),
                    BorderColor = "rgba(54, 162, 235, 1)" // Blue
                },
                new Dataset
                {
                    Label = "Hard difficulty",
                    Data = SudokuAverageScoreLast7DaysHardMode16x16.Select(s => s.Score.TimeSpent ?? 0).ToArray(),
                    BorderColor = "rgba(255, 99, 132, 1)" // Red
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