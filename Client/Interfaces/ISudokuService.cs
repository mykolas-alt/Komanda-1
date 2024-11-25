namespace Projektas.Client.Interfaces
{
    public interface ISudokuService
    {
        Task<int[,]> GenerateSolvedSudokuAsync(int gridSize);
        Task<int[,]> HideNumbersAsync(int[,] grid, int gridSize, int difficulty);
    }

}
