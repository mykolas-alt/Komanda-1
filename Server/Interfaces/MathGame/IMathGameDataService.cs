namespace Projektas.Server.Interfaces.MathGame
{
    public interface IMathGameDataService
    {
        public void SaveData(int data);
        public List<int> LoadData();
    }
}
