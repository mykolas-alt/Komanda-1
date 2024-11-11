namespace Projektas.Client.Pages
{
    using System;
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Components;
    public partial class Sudoku
    {
        private int GridSize { get; set; }
        private int[,] GridValues { get; set; }
        private List<int> PossibleValues { get; set; }

        private int SelectedRow { get; set; }
        private int SelectedCol { get; set; }


        protected override void OnInitialized()
        {
            GridSize = 9;
            GridValues = new int[GridSize, GridSize];
            PossibleValues = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            GenerateSudoku();
        }

        private void GenerateSudoku()
        {
            int[,] board = new int[9, 9];
            Random random = new Random();
            int row, col;
            int num;

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    board[i, j] = 0;
                }
            }

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    row = random.Next(9);
                    col = random.Next(9);
                    num = random.Next(9) + 1;


                    board[row, col] = num;
                }
            }

            GridValues = board;
        }

        protected void HandleCellClicked(int row, int col)
        {
            SelectedRow = row;
            SelectedCol = col;
        }

        protected void HandleValueSelected(ChangeEventArgs args, int row, int col)
        {
            int value = int.Parse(args.Value.ToString());
            GridValues[row, col] = value;
        }
    }
}
