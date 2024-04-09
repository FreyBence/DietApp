using DietAppClient.Models;

namespace DietAppClient.Data
{
    public interface IRecordRepository
    {
        void Create(Record record);
        void Delete(string id);
        Record Read(string id);
        IEnumerable<Record> ReadAll();
        void Update(Record record);
    }
}