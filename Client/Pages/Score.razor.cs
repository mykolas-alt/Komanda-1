using Microsoft.AspNetCore.Components;
using Projektas.Client.Components;
using Projektas.Client.Interfaces;
using Projektas.Shared.Enums;
using Projektas.Shared.Models;

namespace Projektas.Client.Pages {
    partial class Score : IDisposable {
        [Inject]
        public required IAccountScoreService accountScoreService {get; set;}
        [Inject]
        public required IAccountAuthStateProvider AuthStateProvider {get; set;}


        public required List<UserScoreDto<AimTrainerData>> AimTrainerScores {get; set;}
        public required List<UserScoreDto<MathGameData>> MathGameScores {get; set;}
        public required List<UserScoreDto<PairUpData>> PairUpScores {get; set;}
        public required List<UserScoreDto<SudokuData>> SudokuScores {get; set;}

        
        public int MathGame_Played {get; set;}

        public int AimTrainer_Played_Normal {get; set;}
        public int AimTrainer_Played_Hard {get; set;}

        public int PairUp_Played_Easy {get; set;}
        public int PairUp_Played_Normal {get; set;}
        public int PairUp_Played_Hard {get; set;}

        public int Sudoku_Played_Easy_4x4 {get; set;}
        public int Sudoku_Played_Normal_4x4 {get; set;}
        public int Sudoku_Played_Hard_4x4 {get; set;}

        public int Sudoku_Played_Easy_9x9 {get; set;}
        public int Sudoku_Played_Normal_9x9 {get; set;}
        public int Sudoku_Played_Hard_9x9 {get; set;}

        public int Sudoku_Played_Easy_16x16 {get; set;}
        public int Sudoku_Played_Normal_16x16 {get; set;}
        public int Sudoku_Played_Hard_16x16 {get; set;}


        public required GameScore MathGame_Highscore {get; set;}

        public required GameScore AimTrainer_Highscore_Normal {get; set;}
        public required GameScore AimTrainer_Highscore_Hard {get; set;}

        public required GameScore PairUp_Highscore_Easy {get; set;}
        public required GameScore PairUp_Highscore_Normal {get; set;}
        public required GameScore PairUp_Highscore_Hard {get; set;}

        public required GameScore Sudoku_Highscore_Easy_4x4 {get; set;}
        public required GameScore Sudoku_Highscore_Normal_4x4 {get; set;}
        public required GameScore Sudoku_Highscore_Hard_4x4 {get; set;}

        public required GameScore Sudoku_Highscore_Easy_9x9 {get; set;}
        public required GameScore Sudoku_Highscore_Normal_9x9 {get; set;}
        public required GameScore Sudoku_Highscore_Hard_9x9 {get; set;}

        public required GameScore Sudoku_Highscore_Easy_16x16 {get; set;}
        public required GameScore Sudoku_Highscore_Normal_16x16 {get; set;}
        public required GameScore Sudoku_Highscore_Hard_16x16 {get; set;}


        public required GameScore MathGame_AllTimeAverage {get; set;}

        public required GameScore AimTrainer_AllTimeAverage_Normal {get; set;}
        public required GameScore AimTrainer_AllTimeAverage_Hard {get; set;}

        public required GameScore PairUp_AllTimeAverage_Easy {get; set;}
        public required GameScore PairUp_AllTimeAverage_Normal {get; set;}
        public required GameScore PairUp_AllTimeAverage_Hard {get; set;}

        public required GameScore Sudoku_AllTimeAverage_Easy_4x4 {get; set;}
        public required GameScore Sudoku_AllTimeAverage_Normal_4x4 {get; set;}
        public required GameScore Sudoku_AllTimeAverage_Hard_4x4 {get; set;}

        public required GameScore Sudoku_AllTimeAverage_Easy_9x9 {get; set;}
        public required GameScore Sudoku_AllTimeAverage_Normal_9x9 {get; set;}
        public required GameScore Sudoku_AllTimeAverage_Hard_9x9 {get; set;}

        public required GameScore Sudoku_AllTimeAverage_Easy_16x16 {get; set;}
        public required GameScore Sudoku_AllTimeAverage_Normal_16x16 {get; set;}
        public required GameScore Sudoku_AllTimeAverage_Hard_16x16 {get; set;}


        public required List<AverageScoreDto> MathGame_Average_Last7Days {get; set;}

        public required List<AverageScoreDto> AimTrainer_Average_Last7Days_Normal {get; set;}
        public required List<AverageScoreDto> AimTrainer_Average_Last7Days_Hard {get; set;}

        public required List<AverageScoreDto> PairUp_Average_Last7Days_Easy {get; set;}
        public required List<AverageScoreDto> PairUp_Average_Last7Days_Normal {get; set;}
        public required List<AverageScoreDto> PairUp_Average_Last7Days_Hard {get; set;}

        public required List<AverageScoreDto> Sudoku_Average_Last7Days_Easy_4x4 {get; set;}
        public required List<AverageScoreDto> Sudoku_Average_Last7Days_Normal_4x4 {get; set;}
        public required List<AverageScoreDto> Sudoku_Average_Last7Days_Hard_4x4 {get; set;}

        public required List<AverageScoreDto> Sudoku_Average_Last7Days_Easy_9x9 {get; set;}
        public required List<AverageScoreDto> Sudoku_Average_Last7Days_Normal_9x9 {get; set;}
        public required List<AverageScoreDto> Sudoku_Average_Last7Days_Hard_9x9 {get; set;}

        public required List<AverageScoreDto> Sudoku_Average_Last7Days_Easy_16x16 {get; set;}
        public required List<AverageScoreDto> Sudoku_Average_Last7Days_Normal_16x16 {get; set;}
        public required List<AverageScoreDto> Sudoku_Average_Last7Days_Hard_16x16 {get; set;}

        public required Dataset[] MathGame_Average_Last7Days_Dataset {get; set;}

        public required Dataset[] AimTrainer_Average_Last7Days_Dataset {get; set;}

        public required Dataset[] PairUp_Average_Score_Last7Days_Dataset {get; set;}
        public required Dataset[] PairUp_Average_Time_Last7Days_Dataset {get; set;}

        public required Dataset[] Sudoku_Average_Time_Last7Days_4x4_Dataset {get; set;}
        public required Dataset[] Sudoku_Average_Time_Last7Days_9x9_Dataset {get; set;}
        public required Dataset[] Sudoku_Average_Time_Last7Days_16x16_Dataset {get; set;}

        private string activeTab_AimTrainer = "lastGames";
        private string activeTab_MathGame = "lastGames";
        private string activeTab_PairUp = "lastGames";
        private string activeTab_Sudoku = "lastGames";

        public string? username = null;
        
        private bool IsAimTrainerActive {get; set;} = false;
        private bool IsMathGameActive {get; set;} = false;
        private bool IsPairUpActive {get; set;} = false;
        private bool IsSudokuActive {get; set;} = false;

        private void ToggleAimTrainerInfo() {
            IsAimTrainerActive = !IsAimTrainerActive;
        }
        private void ToggleMathGameInfo() {
            IsMathGameActive = !IsMathGameActive;
        }
        private void TogglePairUpInfo() {
            IsPairUpActive = !IsPairUpActive;
        }
        private void ToggleSudokuInfo() {
            IsSudokuActive = !IsSudokuActive;
        }

        private void SetActiveTabAimTrainer(string tabName) {
            activeTab_AimTrainer = tabName;
        }
        private void SetActiveTabMathGame(string tabName) {
            activeTab_MathGame = tabName;
        }
        private void SetActiveTabPairUp(string tabName) {
            activeTab_PairUp = tabName;
        }
        private void SetActiveTabSudoku(string tabName) {
            activeTab_Sudoku = tabName;
        }

        protected override async Task OnInitializedAsync() {
            AuthStateProvider.AuthenticationStateChanged += OnAuthenticationStateChanged;

            await LoadUsernameAsync();
            if(username != null) {
                await LoadScoresAsync();
            }
        }

        public async Task LoadUsernameAsync() {
            username = await ((IAccountAuthStateProvider)AuthStateProvider).GetUsernameAsync();
            StateHasChanged();
        }

        private async void OnAuthenticationStateChanged(Task<AuthenticationState> task) {
            await InvokeAsync(LoadUsernameAsync);
            if (username != null) {
                await InvokeAsync(LoadScoresAsync);
            }
            StateHasChanged();
        }
        
        public void Dispose() {
            AuthStateProvider.AuthenticationStateChanged -= OnAuthenticationStateChanged;
        }
    }
}

/*
       

        

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
*/