using DietAppClient.Helpers;
using DietAppClient.Models;

namespace DietAppClient.Data
{
    public class RecordRepository : IRecordRepository
    {
        string filePath;
        private List<Record> records;

        public RecordRepository()
        {
            filePath = System.IO.Path.Combine(FileSystem.Current.AppDataDirectory, "records.json");
            records = JsonParser.ReadJson(new List<Record>(), filePath);
        }

        public Record Read(string id)
        {
            return records.FirstOrDefault(t => t.Id == id);
        }

        public IEnumerable<Record> ReadAll()
        {
            return records;
        }

        public void Create(Record record)
        {
            records.Add(record);
            JsonParser.WriteJson(records, filePath);
        }

        public void Update(Record record)
        {
            var old = records.FirstOrDefault(t => t.Id == record.Id);
            old.Calories = record.Calories;
            old.Date = record.Date;
            File.Delete(filePath);
            JsonParser.WriteJson(record, filePath);
        }

        public void Delete(string id)
        {
            records.Remove(Read(id));
            File.Delete(filePath);
            JsonParser.WriteJson(records, filePath);
        }
    }
}
