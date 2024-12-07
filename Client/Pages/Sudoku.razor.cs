using Microsoft.AspNetCore.Components;
using Projektas.Client.Interfaces;
using Projektas.Shared.Enums;

namespace Projektas.Client.Pages
{
    public partial class Sudoku
    {
        [Inject]
        public required ISudokuService SudokuService { get; set; }
        [Inject]
        public required ITimerService TimerService { get; set; }

        private static Random _random = new Random();

        private GameDifficulty CurrentDifficulty { get; set; }
        private GameMode GameMode {get; set;} // nextGridSize

        public bool IsGameActive { get; set; }
        public bool IsLoading { get; set; }

        public int GridSize { get; set; }
        public int InternalGridSize { get; set; }
        public int[,]? GridValues { get; set; }
        public int[,]? Solution { get; set; }

        private List<(int, int)>? DisabledCells;

        public List<int>? PossibleValues { get; set; }
        public int SelectedRow { get; set; }
        public int SelectedCol { get; set; }

        public int ElapsedTime { get; private set; }
        public string? Message { get; set; }
        public string? username = null;

        [Inject]
        public IAccountAuthStateProvider AuthStateProvider { get; set; }

        protected override async Task OnInitializedAsync()
        {
            AuthStateProvider.AuthenticationStateChanged += OnAuthenticationStateChangedAsync;

            await LoadUsernameAsync();
        }

        private async Task LoadUsernameAsync()
        {
            username = await ((IAccountAuthStateProvider)AuthStateProvider).GetUsernameAsync();
            StateHasChanged();
        }

        private async void OnAuthenticationStateChangedAsync(Task<AuthenticationState> task)
        {
            await InvokeAsync(LoadUsernameAsync);
            StateHasChanged();
        }

        protected override void OnInitialized()
        {
            GameMode = GameMode.NineByNine; 
            CurrentDifficulty = GameDifficulty.Medium;
            TimerService.OnTick += TimerTick;
            IsGameActive = false;
            GenerateSudokuGameAsync();
        }


        public async Task GenerateSudokuGameAsync()
        {
            if (IsLoading)
            {
                return;
            }
            IsLoading = true;
            GridSize = (int)GameMode;
            int toHide = SudokuDifficulty();
            InternalGridSize = (int)Math.Sqrt(GridSize);
            PossibleValues = Enumerable.Range(1, GridSize).ToList();
            ElapsedTime = 0;
            TimerService.Stop();
            StateHasChanged();

            GridValues = await SudokuService.GenerateSolvedSudokuAsync(GridSize);
            Solution = (int[,])GridValues.Clone();

            GridValues = await SudokuService.HideNumbersAsync(GridValues, GridSize, toHide);

            DisabledCells = Enumerable
               .Range(0, GridSize)
               .SelectMany(row => Enumerable.Range(0, GridSize)
                   .Where(col => GridValues[row, col] != 0)
                   .Select(col => (row, col)))
               .ToList();

            IsGameActive = true;
            IsLoading = false;
            TimerService.Start(180000);
            StateHasChanged();
        }

        public int SudokuDifficulty()
        {
            return GridSize switch
            {
                4 => CurrentDifficulty switch
                {
                    GameDifficulty.Easy => _random.Next(7, 8),
                    GameDifficulty.Medium => _random.Next(9, 10),
                    GameDifficulty.Hard => _random.Next(11, 12),
                    _ => 0,
                },
                9 => CurrentDifficulty switch
                {
                    GameDifficulty.Easy => _random.Next(30, 35),
                    GameDifficulty.Medium => _random.Next(45, 48),
                    GameDifficulty.Hard => _random.Next(53, 57),
                    _ => 0,
                },
                16 => CurrentDifficulty switch
                {
                    GameDifficulty.Easy => _random.Next(30, 50),
                    GameDifficulty.Medium => _random.Next(100, 130),
                    GameDifficulty.Hard => _random.Next(140, 150),
                    _ => 0,
                },
                _ => throw new ArgumentException("Unsupported grid size"),
            };
        }

        public void TimerTick()
        {
            ElapsedTime++;
            Message = null;
            if (TimerService.RemainingTime == 0)
            {
                EndGame(false);
            }

            InvokeAsync(StateHasChanged);
        }

        private void EndGame(bool won)
        {
            IsGameActive = false;
            if (username != null)
            {
                SudokuService.SaveScoreAsync(username, ElapsedTime, CurrentDifficulty, GameMode);
            }
            TimerService.Stop();
            if (won)
            {
                Message = "Correct solution. Solved in " + FormatTime(ElapsedTime);
            }
            else
            {
                Message = "Ran out of time";
            }

            InvokeAsync(StateHasChanged);
        }

        public void IsCorrect()
        {
            if (IsGameActive && !IsLoading)
            {
                if (GridValues!.Cast<int>().SequenceEqual(Solution!.Cast<int>()))
                {
                    EndGame(true);
                }
                else
                {
                    Message = "Incorrect solution";
                }
            }
        }

        public void OnDifficultyChanged(ChangeEventArgs e)
        {
            if (Enum.TryParse(e.Value?.ToString(), true, out GameDifficulty parsedDifficulty))
            {
                CurrentDifficulty = parsedDifficulty;
            }
        }

        public void OnSizeChanged(ChangeEventArgs e)
        {
            if (int.TryParse(e.Value?.ToString(), out int size))
            {
                GameMode = (GameMode)size;
            }
        }

        static public string FormatTime(int totalSeconds)
        {
            int minutes = totalSeconds / 60;
            int seconds = totalSeconds % 60;
            return $"{minutes:D2}:{seconds:D2}";
        }

        private bool IsCellDisabled(int row, int col)
        {
            if (!IsGameActive)
            {
                return true;
            }
            return DisabledCells!.Contains((row, col));
        }

        public void HandleCellClicked(int row, int col)
        {
            SelectedRow = row;
            SelectedCol = col;
        }

        public void HandleValueSelected(ChangeEventArgs args, int row, int col)
        {
            int value = int.Parse(args.Value.ToString());
            GridValues![row, col] = value;
        }

        public void Dispose()
        {
            AuthStateProvider.AuthenticationStateChanged -= OnAuthenticationStateChangedAsync;
        }
    }
}