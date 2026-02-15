using System.Text.Json;

namespace PurchaseOrderSystem.Data
{
    public class JsonRepository<T> where T: class
    {
        private readonly string _filePath;

        public JsonRepository(string filePath)
        {
            _filePath = filePath;
        }

        // Read All Data
        public List<T> GetAll()
        {
            if (!File.Exists(_filePath))
            {
                return new List<T>();
            }

            var json = File.ReadAllText(_filePath);

            return JsonSerializer.Deserialize<List<T>>(json)
                ?? new List<T>();
        }

        // Save all data
        public void SaveAll(List<T> data)
        {
            var json = JsonSerializer.Serialize(data, 
                new JsonSerializerOptions {  WriteIndented = true});

            File.WriteAllText(_filePath, json);
        }
    }
}
