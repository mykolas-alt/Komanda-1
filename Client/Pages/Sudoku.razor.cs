namespace Projektas.Client.Pages
{
    using System;
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Components;
    using Projektas.Client.Interfaces;
    using Projektas.Client.Services;

    public partial class Sudoku
    {
        [Inject]
        public required SudokuService _sudokuService { get; set; }
        [Inject]
        public required ITimerService TimerService { get; set; }

        private static Random _random = new Random();
        public enum Difficulty
        {
            Easy,
            Medium,
            Hard
        }

        private Difficulty CurrentDifficulty { get; set; }

        private bool IsGameActive { get; set; }
        private bool IsLoading { get; set; }

        private int GridSize { get; set; }
        private int[,]? GridValues { get; set; }
        private int[,]? Solution { get; set; }

        private List<(int, int)>? DisabledCells;

        private List<int>? PossibleValues { get; set; }
        private int SelectedRow { get; set; }
        private int SelectedCol { get; set; }

        public int ElapsedTime { get; private set; }
        private string? Message { get; set; }



        protected override void OnInitialized()
        {
            GridSize = 9;
            PossibleValues = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            TimerService.OnTick += TimerTick;
            IsGameActive = false;
            GenerateSudokuGame();
        }


        private async Task GenerateSudokuGame()
        {
            IsLoading = true;
            ElapsedTime = 0;
            TimerService.Stop();
            StateHasChanged();

            GridValues = SudokuService.GenerateSolvedSudoku(GridSize);
            Solution = (int[,])GridValues.Clone();

            GridValues = await _sudokuService.HideNumbers(GridValues, GridSize, SudokuDifficulty());

            DisabledCells = Enumerable
               .Range(0, GridSize)
               .SelectMany(row => Enumerable.Range(0, GridSize)
                   .Where(col => GridValues[row, col] != 0)
                   .Select(col => (row, col)))
               .ToList();

            IsGameActive = true;
            IsLoading = false;
            TimerService.Start(1800);
            StateHasChanged();
        }

        private int SudokuDifficulty()
        {
            return CurrentDifficulty switch
            {
                Difficulty.Easy => _random.Next(20, 25),
                Difficulty.Medium => _random.Next(40, 45),
                Difficulty.Hard => _random.Next(55, 57),
                _ => 0,
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
        private void IsCorrect()
        {
            if (IsGameActive)
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
            if (Enum.TryParse(e.Value?.ToString(), true, out Difficulty parsedDifficulty))
            {
                CurrentDifficulty = parsedDifficulty;
            }
        }
        public string FormatTime(int totalSeconds)
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
        protected void HandleCellClicked(int row, int col)
        {
            SelectedRow = row;
            SelectedCol = col;
        }
        protected void HandleValueSelected(ChangeEventArgs args, int row, int col)
        {
            int value = int.Parse(args.Value.ToString());
            GridValues![row, col] = value;
        }
    }
}
