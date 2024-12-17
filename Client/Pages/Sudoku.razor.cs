using Microsoft.AspNetCore.Components;
using Projektas.Client.Interfaces;
using Projektas.Client.Services;
using Projektas.Shared.Enums;
using Projektas.Shared.Models;

namespace Projektas.Client.Pages {
    public partial class Sudoku {
        [Inject]
        public required ISudokuService SudokuService {get; set;}
        
        [Inject]
        public required ITimerService TimerService {get; set;}

        [Inject]
        public IAccountAuthStateProvider AuthStateProvider {get; set;}

        private static Random _random = new Random();

        
        public string gameScreen = "main";
        private GameDifficulty Difficulty {get; set;} = GameDifficulty.Normal;
        public GameMode Size {get; set;} = GameMode.NineByNine; // nextGridSize

        public int GridSize {get; set;}
        public int GridElemSize {get; set;}
        public int InternalGridSize {get; set;}
        public int[,]? GridValues {get; set;}
        public int[,]? Solution {get; set;}

        private List<(int, int)>? DisabledCells;

        public List<int>? PossibleValues {get; set;}
        public int SelectedRow {get; set;}
        public int SelectedCol {get; set;}

        public int ElapsedTime {get; private set;}
        public string? Message {get; set;}
        
        private UserScoreDto<SudokuData>? highscore {get; set;}
        public List<UserScoreDto<SudokuData>>? topScores {get; private set;}

        public string? username = null;

        public void ChangeScreen(string mode) {
            gameScreen = mode;
        }

        public void ChangeDifficulty(string mode) {
            switch(mode) {
                case "Easy":
                    Difficulty = GameDifficulty.Easy;
                    break;
                case "Normal":
                    Difficulty = GameDifficulty.Normal;
                    break;
                case "Hard":
                    Difficulty = GameDifficulty.Hard;
                    break;
            }
        }

        public void ChangeSize(string size) {
            switch(size) {
                case "4x4":
                    Size = GameMode.FourByFour;
                    break;
                case "9x9":
                    Size = GameMode.NineByNine;
                    break;
                case "16x16":
                    Size = GameMode.SixteenBySixteen;
                    break;
            }
        }

        static public string FormatTime(int totalSeconds) {
            int minutes = totalSeconds / 60;
            int seconds = totalSeconds % 60;
            return $"{minutes:D2}:{seconds:D2}";
        }

        protected override async Task OnInitializedAsync() {
            AuthStateProvider.AuthenticationStateChanged += OnAuthenticationStateChangedAsync;

            await LoadUsernameAsync();
            if(username != null) {
                highscore = await SudokuService.GetUserHighscoreAsync(username);
            }
            topScores = await SudokuService.GetTopScoresAsync(topCount: 10);
        }

        private async Task LoadUsernameAsync() {
            username = await ((IAccountAuthStateProvider)AuthStateProvider).GetUsernameAsync();
            StateHasChanged();
        }

        private async void OnAuthenticationStateChangedAsync(Task<AuthenticationState> task) {
            await InvokeAsync(LoadUsernameAsync);
            StateHasChanged();
        }

        public async Task StartGameAsync() {
            Message = null;
            TimerService.OnTick += TimerTick;
            await GenerateSudokuGameAsync();
            gameScreen = "started";
        }

        public async Task GenerateSudokuGameAsync() {
            ElapsedTime = 0;
            TimerService.Stop();
            GridSize = (int)Size;
            int toHide = SudokuDifficulty();
            InternalGridSize = (int)Math.Sqrt(GridSize);
            PossibleValues = Enumerable.Range(1, GridSize).ToList();
            TimerService.Stop();
            await InvokeAsync(StateHasChanged);

            GridValues = await SudokuService.GenerateSolvedSudokuAsync(GridSize);
            Solution = (int[,])GridValues.Clone();

            GridValues = await SudokuService.HideNumbersAsync(GridValues, GridSize, toHide);

            DisabledCells = Enumerable
               .Range(0, GridSize)
               .SelectMany(row => Enumerable.Range(0, GridSize)
                   .Where(col => GridValues[row, col] != 0)
                   .Select(col => (row, col)))
               .ToList();
            
            TimerService.Start(18000);
            await InvokeAsync(StateHasChanged);
        }

        public int SudokuDifficulty() {
            return GridSize switch {
                4 => Difficulty switch {
                    GameDifficulty.Easy => _random.Next(7, 8),
                    GameDifficulty.Normal => _random.Next(9, 10),
                    GameDifficulty.Hard => _random.Next(11, 12),
                    _ => 0,
                },
                9 => Difficulty switch {
                    GameDifficulty.Easy => _random.Next(30, 35),
                    GameDifficulty.Normal => _random.Next(45, 48),
                    GameDifficulty.Hard => _random.Next(53, 57),
                    _ => 0,
                },
                16 => Difficulty switch {
                    GameDifficulty.Easy => _random.Next(30, 50),
                    GameDifficulty.Normal => _random.Next(100, 130),
                    GameDifficulty.Hard => _random.Next(140, 150),
                    _ => 0,
                },
                _ => throw new ArgumentException("Unsupported grid size"),
            };
        }

        public void TimerTick() {
            ElapsedTime++;
            if(TimerService.RemainingTime == 0) {
                EndGameAsync();
            }

            InvokeAsync(StateHasChanged);
        }

        private async Task EndGameAsync() {
            gameScreen = "ended";
            if(username != null) {
                await SudokuService.SaveScoreAsync(username, ElapsedTime, Difficulty, Size);
                highscore = await SudokuService.GetUserHighscoreAsync(username);
            }
            topScores = await SudokuService.GetTopScoresAsync(topCount: 10);
            TimerService.Stop();
            await InvokeAsync(StateHasChanged);
        }

        public void IsCorrect() {
            if(gameScreen == "started") {
                if (GridValues!.Cast<int>().SequenceEqual(Solution!.Cast<int>())) {
                    EndGameAsync();
                } else {
                    Message = "Incorrect solution";
                }
            }
        }

        private bool IsCellDisabled(int row, int col) {
            if(gameScreen != "started") {
                return true;
            }
            return DisabledCells!.Contains((row, col));
        }

        public void HandleCellClicked(int row, int col) {
            SelectedRow = row;
            SelectedCol = col;
        }

        public void HandleValueSelected(ChangeEventArgs args, int row, int col) {
            int value = int.Parse(args.Value.ToString());
            GridValues![row, col] = value;
        }

        public void Dispose() {
            AuthStateProvider.AuthenticationStateChanged -= OnAuthenticationStateChangedAsync;
        }
    }
}