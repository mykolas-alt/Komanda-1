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

        

        public int MathGame_Played_Easy {get; set;}
        public int MathGame_Played_Normal {get; set;}
        public int MathGame_Played_Hard {get; set;}

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



        public required GameScore MathGame_Highscore_Easy {get; set;}
        public required GameScore MathGame_Highscore_Normal {get; set;}
        public required GameScore MathGame_Highscore_Hard {get; set;}

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



        public required GameScore MathGame_AllTimeAverage_Easy {get; set;}
        public required GameScore MathGame_AllTimeAverage_Normal {get; set;}
        public required GameScore MathGame_AllTimeAverage_Hard {get; set;}

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



        public required List<AverageScoreDto> MathGame_Average_Last7Days_Easy {get; set;}
        public required List<AverageScoreDto> MathGame_Average_Last7Days_Normal {get; set;}
        public required List<AverageScoreDto> MathGame_Average_Last7Days_Hard {get; set;}

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

        
        private string activeDifficulty_MathGame = "easy";
        private string activeDifficulty_AimTrainer = "normal";
        private string activeDifficulty_PairUp = "easy";
        private string activeDifficulty_Sudoku = "easy";
        
        private string activeSize_Sudoku = "4x4";

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


        private void SetActiveDifficultyMathGame(string difficulty) {
            activeDifficulty_MathGame = difficulty;
        }

        private void SetActiveDifficultyAimTrainer(string difficulty) {
            activeDifficulty_AimTrainer = difficulty;
        }
        private void SetActiveDifficultyPairUp(string difficulty) {
            activeDifficulty_PairUp = difficulty;
        }
        private void SetActiveDifficultySudoku(string difficulty) {
            activeDifficulty_Sudoku = difficulty;
        }

        private void SetActiveSizeSudoku(string size) {
            activeSize_Sudoku = size;
        }

        protected override async Task OnInitializedAsync() {
            AuthStateProvider.AuthenticationStateChanged += OnAuthenticationStateChangedAsync;

            await LoadUsernameAsync();
            if(username != null) {
                await LoadScoresAsync();
            }
        }

        public async Task LoadUsernameAsync() {
            username = await ((IAccountAuthStateProvider)AuthStateProvider).GetUsernameAsync();
            StateHasChanged();
        }

        private async void OnAuthenticationStateChangedAsync(Task<AuthenticationState> task) {
            await InvokeAsync(LoadUsernameAsync);
            if(username != null) {
                await InvokeAsync(LoadScoresAsync);
            }
            StateHasChanged();
        }

        public async Task LoadScoresAsync() {
            await LoadAimTrainerScoresAsync();
            await LoadMathGameScoresAsync();
            await LoadPairUpScoresAsync();
            await LoadSudokuScoresAsync();
            
            LoadAimTrainerDatasets();
            LoadMathGameDatasets();
            LoadPairUpDatasets();
            LoadSudokuDatasets();

            StateHasChanged();
        }

        public async Task LoadAimTrainerScoresAsync() {
            // last 10 games
            AimTrainerScores = await accountScoreService.GetAimTrainerScoresAsync(username);

            // matches played
            AimTrainer_Played_Normal = await accountScoreService.GetAimTrainerMatchesPlayedAsync(username, GameDifficulty.Normal);
            AimTrainer_Played_Hard = await accountScoreService.GetAimTrainerMatchesPlayedAsync(username, GameDifficulty.Hard);
            
            // highscore
            AimTrainer_Highscore_Normal = await accountScoreService.GetAimTrainerHighscoreAsync(username, GameDifficulty.Normal);
            AimTrainer_Highscore_Hard = await accountScoreService.GetAimTrainerHighscoreAsync(username, GameDifficulty.Hard);

            // average scores
            AimTrainer_AllTimeAverage_Normal = await accountScoreService.GetAimTrainerAverageScoreAsync(username, GameDifficulty.Normal);
            AimTrainer_AllTimeAverage_Hard = await accountScoreService.GetAimTrainerAverageScoreAsync(username, GameDifficulty.Hard);

            // average scores for the last 7 days
            AimTrainer_Average_Last7Days_Normal = await accountScoreService.GetAimTrainerAverageScoreLast7DaysAsync(username, GameDifficulty.Normal);
            AimTrainer_Average_Last7Days_Hard = await accountScoreService.GetAimTrainerAverageScoreLast7DaysAsync(username, GameDifficulty.Hard);
        }

        public async Task LoadMathGameScoresAsync() {
            // last 10 games
            MathGameScores = await accountScoreService.GetMathGameScoresAsync(username);

            // matches played
            MathGame_Played_Easy = await accountScoreService.GetMathGameMatchesPlayedAsync(username, GameDifficulty.Easy);
            MathGame_Played_Normal  = await accountScoreService.GetMathGameMatchesPlayedAsync(username, GameDifficulty.Normal);
            MathGame_Played_Hard = await accountScoreService.GetMathGameMatchesPlayedAsync(username, GameDifficulty.Hard);

            // highscore
            MathGame_Highscore_Easy = await accountScoreService.GetMathGameHighscoreAsync(username, GameDifficulty.Easy);
            MathGame_Highscore_Normal = await accountScoreService.GetMathGameHighscoreAsync(username, GameDifficulty.Normal);
            MathGame_Highscore_Hard = await accountScoreService.GetMathGameHighscoreAsync(username, GameDifficulty.Hard);

            // average score
            MathGame_AllTimeAverage_Easy = await accountScoreService.GetMathGameAverageScoreAsync(username, GameDifficulty.Easy);
            MathGame_AllTimeAverage_Normal = await accountScoreService.GetMathGameAverageScoreAsync(username, GameDifficulty.Normal);
            MathGame_AllTimeAverage_Hard = await accountScoreService.GetMathGameAverageScoreAsync(username, GameDifficulty.Hard);

            // average score for the last 7 days
            MathGame_Average_Last7Days_Easy = await accountScoreService.GetMathGameAverageScoreLast7DaysAsync(username, GameDifficulty.Easy);
            MathGame_Average_Last7Days_Normal = await accountScoreService.GetMathGameAverageScoreLast7DaysAsync(username, GameDifficulty.Normal);
            MathGame_Average_Last7Days_Hard = await accountScoreService.GetMathGameAverageScoreLast7DaysAsync(username, GameDifficulty.Hard);
        }

        public async Task LoadPairUpScoresAsync() {
            // last 10 games
            PairUpScores = await accountScoreService.GetPairUpScoresAsync(username);

            // matches played
            PairUp_Played_Easy = await accountScoreService.GetPairUpMatchesPlayedAsync(username, GameDifficulty.Easy);
            PairUp_Played_Normal = await accountScoreService.GetPairUpMatchesPlayedAsync(username, GameDifficulty.Normal);
            PairUp_Played_Hard = await accountScoreService.GetPairUpMatchesPlayedAsync(username, GameDifficulty.Hard);

            // highscore
            PairUp_Highscore_Easy = await accountScoreService.GetPairUpHighscoreAsync(username, GameDifficulty.Easy);
            PairUp_Highscore_Normal = await accountScoreService.GetPairUpHighscoreAsync(username, GameDifficulty.Normal);
            PairUp_Highscore_Hard = await accountScoreService.GetPairUpHighscoreAsync(username, GameDifficulty.Hard);

            // average scores
            PairUp_AllTimeAverage_Easy = await accountScoreService.GetPairUpAverageScoreAsync(username, GameDifficulty.Easy);
            PairUp_AllTimeAverage_Normal = await accountScoreService.GetPairUpAverageScoreAsync(username, GameDifficulty.Normal);
            PairUp_AllTimeAverage_Hard = await accountScoreService.GetPairUpAverageScoreAsync(username, GameDifficulty.Hard);

            // average scores for the last 7 days
            PairUp_Average_Last7Days_Easy = await accountScoreService.GetPairUpAverageScoreLast7DaysAsync(username, GameDifficulty.Easy);
            PairUp_Average_Last7Days_Normal = await accountScoreService.GetPairUpAverageScoreLast7DaysAsync(username, GameDifficulty.Normal);
            PairUp_Average_Last7Days_Hard = await accountScoreService.GetPairUpAverageScoreLast7DaysAsync(username, GameDifficulty.Hard);
        }

        public async Task LoadSudokuScoresAsync() {
            // last 10 games
            SudokuScores = await accountScoreService.GetSudokuScoresAsync(username);

            // Matches played
            Sudoku_Played_Easy_4x4 = await accountScoreService.GetSudokuMatchesPlayedAsync(username, GameDifficulty.Easy, GameMode.FourByFour);
            Sudoku_Played_Normal_4x4 = await accountScoreService.GetSudokuMatchesPlayedAsync(username, GameDifficulty.Normal, GameMode.FourByFour);
            Sudoku_Played_Hard_4x4 = await accountScoreService.GetSudokuMatchesPlayedAsync(username, GameDifficulty.Hard, GameMode.FourByFour);

            Sudoku_Played_Easy_9x9 = await accountScoreService.GetSudokuMatchesPlayedAsync(username, GameDifficulty.Easy, GameMode.NineByNine);
            Sudoku_Played_Normal_9x9 = await accountScoreService.GetSudokuMatchesPlayedAsync(username, GameDifficulty.Normal, GameMode.NineByNine);
            Sudoku_Played_Hard_9x9 = await accountScoreService.GetSudokuMatchesPlayedAsync(username, GameDifficulty.Hard, GameMode.NineByNine);

            Sudoku_Played_Easy_16x16 = await accountScoreService.GetSudokuMatchesPlayedAsync(username, GameDifficulty.Easy, GameMode.SixteenBySixteen);
            Sudoku_Played_Normal_16x16 = await accountScoreService.GetSudokuMatchesPlayedAsync(username, GameDifficulty.Normal, GameMode.SixteenBySixteen);
            Sudoku_Played_Hard_16x16 = await accountScoreService.GetSudokuMatchesPlayedAsync(username, GameDifficulty.Hard, GameMode.SixteenBySixteen);

            // Higscore
            Sudoku_Highscore_Easy_4x4 = await accountScoreService.GetSudokuHighscoreAsync(username, GameDifficulty.Easy, GameMode.FourByFour);
            Sudoku_Highscore_Normal_4x4 = await accountScoreService.GetSudokuHighscoreAsync(username, GameDifficulty.Normal, GameMode.FourByFour);
            Sudoku_Highscore_Hard_4x4 = await accountScoreService.GetSudokuHighscoreAsync(username, GameDifficulty.Hard, GameMode.FourByFour);

            Sudoku_Highscore_Easy_9x9 = await accountScoreService.GetSudokuHighscoreAsync(username, GameDifficulty.Easy, GameMode.NineByNine);
            Sudoku_Highscore_Normal_9x9 = await accountScoreService.GetSudokuHighscoreAsync(username, GameDifficulty.Normal, GameMode.NineByNine);
            Sudoku_Highscore_Hard_9x9 = await accountScoreService.GetSudokuHighscoreAsync(username, GameDifficulty.Hard, GameMode.NineByNine);

            Sudoku_Highscore_Easy_16x16 = await accountScoreService.GetSudokuHighscoreAsync(username, GameDifficulty.Easy, GameMode.SixteenBySixteen);
            Sudoku_Highscore_Normal_16x16 = await accountScoreService.GetSudokuHighscoreAsync(username, GameDifficulty.Normal, GameMode.SixteenBySixteen);
            Sudoku_Highscore_Hard_16x16 = await accountScoreService.GetSudokuHighscoreAsync(username, GameDifficulty.Hard, GameMode.SixteenBySixteen);

            // Average scores
            Sudoku_AllTimeAverage_Easy_4x4 = await accountScoreService.GetSudokuAverageScoreAsync(username, GameDifficulty.Easy, GameMode.FourByFour);
            Sudoku_AllTimeAverage_Normal_4x4 = await accountScoreService.GetSudokuAverageScoreAsync(username, GameDifficulty.Normal, GameMode.FourByFour);
            Sudoku_AllTimeAverage_Hard_4x4 = await accountScoreService.GetSudokuAverageScoreAsync(username, GameDifficulty.Hard, GameMode.FourByFour);

            Sudoku_AllTimeAverage_Easy_9x9 = await accountScoreService.GetSudokuAverageScoreAsync(username, GameDifficulty.Easy, GameMode.NineByNine);
            Sudoku_AllTimeAverage_Normal_9x9 = await accountScoreService.GetSudokuAverageScoreAsync(username, GameDifficulty.Normal, GameMode.NineByNine);
            Sudoku_AllTimeAverage_Hard_9x9 = await accountScoreService.GetSudokuAverageScoreAsync(username, GameDifficulty.Hard, GameMode.NineByNine);

            Sudoku_AllTimeAverage_Easy_16x16 = await accountScoreService.GetSudokuAverageScoreAsync(username, GameDifficulty.Easy, GameMode.SixteenBySixteen);
            Sudoku_AllTimeAverage_Normal_16x16 = await accountScoreService.GetSudokuAverageScoreAsync(username, GameDifficulty.Normal, GameMode.SixteenBySixteen);
            Sudoku_AllTimeAverage_Hard_16x16 = await accountScoreService.GetSudokuAverageScoreAsync(username, GameDifficulty.Hard, GameMode.SixteenBySixteen);

            // Average scores in last 7 days
            Sudoku_Average_Last7Days_Easy_4x4 = await accountScoreService.GetSudokuAverageScoreLast7DaysAsync(username, GameDifficulty.Easy, GameMode.FourByFour);
            Sudoku_Average_Last7Days_Normal_4x4 = await accountScoreService.GetSudokuAverageScoreLast7DaysAsync(username, GameDifficulty.Normal, GameMode.FourByFour);
            Sudoku_Average_Last7Days_Hard_4x4 = await accountScoreService.GetSudokuAverageScoreLast7DaysAsync(username, GameDifficulty.Hard, GameMode.FourByFour);

            Sudoku_Average_Last7Days_Easy_9x9 = await accountScoreService.GetSudokuAverageScoreLast7DaysAsync(username, GameDifficulty.Easy, GameMode.NineByNine);
            Sudoku_Average_Last7Days_Normal_9x9 = await accountScoreService.GetSudokuAverageScoreLast7DaysAsync(username, GameDifficulty.Normal, GameMode.NineByNine);
            Sudoku_Average_Last7Days_Hard_9x9 = await accountScoreService.GetSudokuAverageScoreLast7DaysAsync(username, GameDifficulty.Hard, GameMode.NineByNine);

            Sudoku_Average_Last7Days_Easy_16x16 = await accountScoreService.GetSudokuAverageScoreLast7DaysAsync(username, GameDifficulty.Easy, GameMode.SixteenBySixteen);
            Sudoku_Average_Last7Days_Normal_16x16 = await accountScoreService.GetSudokuAverageScoreLast7DaysAsync(username, GameDifficulty.Normal, GameMode.SixteenBySixteen);
            Sudoku_Average_Last7Days_Hard_16x16 = await accountScoreService.GetSudokuAverageScoreLast7DaysAsync(username, GameDifficulty.Hard, GameMode.SixteenBySixteen);
        }

        public void LoadAimTrainerDatasets() {
            AimTrainer_Average_Last7Days_Dataset = new Dataset[] {
                new Dataset {
                    Label = "Normal difficulty",
                    Data = AimTrainer_Average_Last7Days_Normal.Select(s => s.Score.Scores ?? 0).ToArray(),
                    BorderColor = "rgba(54, 162, 235, 1)", // Blue
                    yAxisLabel = "Points"
                },
                new Dataset {
                    Label = "Hard difficulty",
                    Data = AimTrainer_Average_Last7Days_Hard.Select(s => s.Score.Scores ?? 0).ToArray(),
                    BorderColor = "rgba(255, 99, 132, 1)" // Red
                }
            };
        }

        public void LoadMathGameDatasets() {
            MathGame_Average_Last7Days_Dataset = new Dataset[] {
                new Dataset {
                    Label = "Easy difficulty",
                    Data = MathGame_Average_Last7Days_Easy.Select(s => s.Score.Scores ?? 0).ToArray(),
                    BorderColor = "rgba(75, 192, 192, 1)", // Green
                },
                new Dataset {
                    Label = "Normal difficulty",
                    Data = MathGame_Average_Last7Days_Normal.Select(s => s.Score.Scores ?? 0).ToArray(),
                    BorderColor = "rgba(153, 102, 255, 1)", // Purple
                },
                new Dataset {
                    Label = "Hard difficulty",
                    Data = MathGame_Average_Last7Days_Hard.Select(s => s.Score.Scores ?? 0).ToArray(),
                    BorderColor = "rgba(255, 159, 64, 1)" // Orange
                }

            };
        }

        public void LoadPairUpDatasets() {
            PairUp_Average_Score_Last7Days_Dataset = new Dataset[] {
                new Dataset {
                    Label = "Easy difficulty",
                    Data = PairUp_Average_Last7Days_Easy.Select(s => s.Score.Scores ?? 0).ToArray(),
                    BorderColor = "rgba(255, 206, 86, 1)", // Yellow
                    yAxisLabel = "Mistakes"
                },
                new Dataset {
                    Label = "Normal difficulty",
                    Data = PairUp_Average_Last7Days_Normal.Select(s => s.Score.Scores ?? 0).ToArray(),
                    BorderColor = "rgba(153, 102, 255, 1)" // Purple
                },
                new Dataset {
                    Label = "Hard difficulty",
                    Data = PairUp_Average_Last7Days_Hard.Select(s => s.Score.Scores ?? 0).ToArray(),
                    BorderColor = "rgba(255, 159, 64, 1)" // Orange
                }
            };

            PairUp_Average_Time_Last7Days_Dataset = new Dataset[] {
                new Dataset {
                    Label = "Easy difficulty",
                    Data = PairUp_Average_Last7Days_Easy.Select(s => s.Score.TimeSpent ?? 0).ToArray(),
                    BorderColor = "rgba(75, 192, 192, 1)", // Green
                    yAxisLabel = "Solution time"
                },
                new Dataset {
                    Label = "Normal difficulty",
                    Data = PairUp_Average_Last7Days_Normal.Select(s => s.Score.TimeSpent ?? 0).ToArray(),
                    BorderColor = "rgba(255, 99, 132, 1)" // Red
                },
                new Dataset {
                    Label = "Hard difficulty",
                    Data = PairUp_Average_Last7Days_Hard.Select(s => s.Score.TimeSpent ?? 0).ToArray(),
                    BorderColor = "rgba(255, 159, 64, 1)" // Orange
                }
            };
        }

        public void LoadSudokuDatasets() {
            Sudoku_Average_Time_Last7Days_4x4_Dataset = new Dataset[] {
                new Dataset {
                    Label = "Easy difficulty",
                    Data = Sudoku_Average_Last7Days_Easy_4x4.Select(s => s.Score.TimeSpent ?? 0).ToArray(),
                    BorderColor = "rgba(255, 165, 0, 1)", // Orange
                    yAxisLabel = "Solution Time"
                },
                new Dataset {
                    Label = "Normal difficulty",
                    Data = Sudoku_Average_Last7Days_Normal_4x4.Select(s => s.Score.TimeSpent ?? 0).ToArray(),
                    BorderColor = "rgba(75, 0, 130, 1)" // Indigo
                },
                new Dataset {
                    Label = "Hard difficulty",
                    Data = Sudoku_Average_Last7Days_Hard_4x4.Select(s => s.Score.TimeSpent ?? 0).ToArray(),
                    BorderColor = "rgba(255, 20, 147, 1)" // Deep Pink
                }
            };

            Sudoku_Average_Time_Last7Days_9x9_Dataset = new Dataset[] {
                new Dataset {
                    Label = "Easy difficulty",
                    Data = Sudoku_Average_Last7Days_Easy_9x9.Select(s => s.Score.TimeSpent ?? 0).ToArray(),
                    BorderColor = "rgba(238, 130, 238, 1)", // Pink
                    yAxisLabel = "Solution time"
                },
                new Dataset {
                    Label = "Normal difficulty",
                    Data = Sudoku_Average_Last7Days_Normal_9x9.Select(s => s.Score.TimeSpent ?? 0).ToArray(),
                    BorderColor = "rgba(60, 179, 113, 1)" // Green
                },
                new Dataset {
                    Label = "Hard difficulty",
                    Data = Sudoku_Average_Last7Days_Hard_9x9.Select(s => s.Score.TimeSpent ?? 0).ToArray(),
                    BorderColor = "rgba(255, 165, 0, 1)" // Yellow
                }
            };

            Sudoku_Average_Time_Last7Days_16x16_Dataset = new Dataset[] {
                new Dataset {
                    Label = "Easy difficulty",
                    Data = Sudoku_Average_Last7Days_Easy_16x16.Select(s => s.Score.TimeSpent ?? 0).ToArray(),
                    BorderColor = "rgba(75, 192, 192, 1)", // Green
                    yAxisLabel = "Solution Time"
                },
                new Dataset {
                    Label = "Normal difficulty",
                    Data = Sudoku_Average_Last7Days_Normal_16x16.Select(s => s.Score.TimeSpent ?? 0).ToArray(),
                    BorderColor = "rgba(54, 162, 235, 1)" // Blue
                },
                new Dataset {
                    Label = "Hard difficulty",
                    Data = Sudoku_Average_Last7Days_Hard_16x16.Select(s => s.Score.TimeSpent ?? 0).ToArray(),
                    BorderColor = "rgba(255, 99, 132, 1)" // Red
                }
            };
        }
        
        public void Dispose() {
            AuthStateProvider.AuthenticationStateChanged -= OnAuthenticationStateChangedAsync;
        }
    }
}