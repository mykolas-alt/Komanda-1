using Projektas.Server.Services.MathGame;

namespace Projektas.Tests.Services.MathGameTests
{
    public class MathGameDataServiceTests
    {
        [Fact]
        public void SaveData_ShouldWriteDataToFile()
        {
            string tempFilePath = Path.GetTempFileName();
            var service = new MathGameDataService(tempFilePath);
            int testData = 42;

            service.SaveData(testData);

            string[] lines = File.ReadAllLines(tempFilePath);
            Assert.Contains(testData.ToString(), lines);

            File.Delete(tempFilePath);
        }

        [Fact]
        public void LoadData_ShouldReturnListOfIntegers()
        {
            string tempFilePath = Path.GetTempFileName();
            File.WriteAllLines(tempFilePath, new[] { "1", "2", "3" });
            var service = new MathGameDataService(tempFilePath);

            List<int> result = service.LoadData();

            Assert.Equal(new List<int> { 1, 2, 3 }, result);

            File.Delete(tempFilePath);
        }

        [Fact]
        public void LoadData_ShouldReturnEmptyListIfFileDoesNotExist()
        {
            string tempFilePath = Path.GetTempFileName();
            File.Delete(tempFilePath); // makes sure the file is not existing
            var service = new MathGameDataService(tempFilePath);

            List<int> result = service.LoadData();

            Assert.Empty(result);
        }
    }
}
