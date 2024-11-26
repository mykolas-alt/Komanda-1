using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Projektas.Client.Interfaces;
using Projektas.Client.Services;﻿

namespace Projektas.Client.Pages {
    public partial class Sudoku {
        [Inject]
        public required ISudokuService SudokuService {get;set;}
        [Inject]
        public required ITimerService TimerService {get;set;}

        private static Random _random=new Random();
        public enum Difficulty {
            Easy,
            Medium,
            Hard
        }

        private Difficulty CurrentDifficulty {get;set;}

        public bool IsGameActive {get;set;}
        private bool IsLoading {get;set;}

        public int GridSize {get;set;}
        public int[,]? GridValues {get;set;}
        public int[,]? Solution {get;set;}

        private List<(int,int)>? DisabledCells;

        public List<int>? PossibleValues {get;set;}
        public int SelectedRow {get;set;}
        public int SelectedCol {get;set;}

        public int ElapsedTime {get;private set;}
        public string? Message {get;set;}
		    public string? username=null;

        [Inject]
        public IAccountAuthStateProvider AuthStateProvider {get;set;}

		    protected override async Task OnInitializedAsync() {
			      AuthStateProvider.AuthenticationStateChanged+=OnAuthenticationStateChanged;

			      await LoadUsernameAsync();
		    }

		    private async Task LoadUsernameAsync() {
			      username=await ((IAccountAuthStateProvider)AuthStateProvider).GetUsernameAsync();
			      StateHasChanged();
		    }

		    private async void OnAuthenticationStateChanged(Task<AuthenticationState> task) {
			      await InvokeAsync(LoadUsernameAsync);
			      StateHasChanged();
		    }

        protected override void OnInitialized() {
            GridSize=9;
            PossibleValues=new List<int> {1,2,3,4,5,6,7,8,9};
            TimerService.OnTick+=TimerTick;
            IsGameActive=false;
            GenerateSudokuGame();
        }


        public async Task GenerateSudokuGame() {
            IsLoading=true;
            ElapsedTime=0;
            TimerService.Stop();
            StateHasChanged();

            GridValues=await SudokuService.GenerateSolvedSudokuAsync(GridSize);
            Solution=(int[,])GridValues.Clone();

            GridValues=await SudokuService.HideNumbersAsync(GridValues,GridSize,SudokuDifficulty());

            DisabledCells=Enumerable
               .Range(0,GridSize)
               .SelectMany(row => Enumerable.Range(0,GridSize)
                   .Where(col => GridValues[row,col]!=0)
                   .Select(col => (row,col)))
               .ToList();

            IsGameActive=true;
            IsLoading=false;
            TimerService.Start(1800);
            StateHasChanged();
        }

        public int SudokuDifficulty() {
            return CurrentDifficulty switch {
                Difficulty.Easy => _random.Next(20,25),
                Difficulty.Medium => _random.Next(40,45),
                Difficulty.Hard => _random.Next(55,57),
                _ => 0,
            };
        }

        public void TimerTick() {
            ElapsedTime++;
            Message=null;
            if(TimerService.RemainingTime==0) {
                EndGame(false);
            }

            InvokeAsync(StateHasChanged);
        }
      
        private void EndGame(bool won) {
            IsGameActive=false;
            TimerService.Stop();
            if(won) {
                Message="Correct solution. Solved in "+FormatTime(ElapsedTime);
            } else {
                Message="Ran out of time";
            }

            InvokeAsync(StateHasChanged);
        }
      
        public void IsCorrect() {
            if(IsGameActive) {
                if(GridValues!.Cast<int>().SequenceEqual(Solution!.Cast<int>())) {
                    EndGame(true);
                } else {
                    Message="Incorrect solution";
                }
            }
        }
      
        public void OnDifficultyChanged(ChangeEventArgs e) {
            if(Enum.TryParse(e.Value?.ToString(),true,out Difficulty parsedDifficulty)) {
                CurrentDifficulty=parsedDifficulty;
            }
        }
      
        public string FormatTime(int totalSeconds) {
            int minutes=totalSeconds/60;
            int seconds=totalSeconds%60;
            return $"{minutes:D2}:{seconds:D2}";
        }
      
        private bool IsCellDisabled(int row,int col) {
            if(!IsGameActive) {
                return true;
            }
            return DisabledCells!.Contains((row,col));
        }
      
        public void HandleCellClicked(int row,int col) {
            SelectedRow=row;
            SelectedCol=col;
        }
      
        public void HandleValueSelected(ChangeEventArgs args,int row,int col) {
            int value=int.Parse(args.Value.ToString());
            GridValues![row,col]=value;
        }

        public void Dispose() {
            AuthStateProvider.AuthenticationStateChanged-=OnAuthenticationStateChanged;
        }
    }
}
