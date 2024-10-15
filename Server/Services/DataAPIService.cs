namespace Projektas.Server.Services
{
    public class DataAPIService
    {
        private readonly string _filepath;

        public DataAPIService(string filepath)
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
                while ((line = reader.ReadLine())!= null)
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
