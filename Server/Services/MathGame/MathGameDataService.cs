using Projektas.Server.Interfaces.MathGame;

namespace Projektas.Server.Services.MathGame
{
    public class MathGameDataService : IMathGameDataService
    {
        private readonly string _filepath;

        public MathGameDataService(string filepath)
        {
            _filepath = filepath;
        }
        public void SaveData(int data)
        {
            using (StreamWriter writer = new StreamWriter(_filepath, append: true))
            {
                writer.WriteLine(data);
            }
        }

        public List<int> LoadData()
        {
            List<int> data = new();

            if (!File.Exists(_filepath))
            {
                return data;
            }

            using (StreamReader reader = new StreamReader(_filepath))
            {
                string? line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (int.TryParse(line, out int number))
                    {
                        data.Add(number);
                    }
                }
            }
            return data;
        }
    }
}
