using DietAppClient.Models;

namespace DietAppClient.Data
{
    public interface IEatingRepository
    {
        void Create(Eating eating);
        void Delete(string id);
        Eating Read(string id);
        IEnumerable<Eating> ReadAll();
        void Update(Eating eating);
    }
}