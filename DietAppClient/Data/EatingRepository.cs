using DietAppClient.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DietAppClient.Helpers;

namespace DietAppClient.Data
{
    public class EatingRepository : IEatingRepository
    {
        string filePath;
        private List<Eating> eatings;

        public EatingRepository()
        {
            filePath = System.IO.Path.Combine(FileSystem.Current.AppDataDirectory, "eatings.json");
            eatings = JsonParser.ReadJson(new List<Eating>(), filePath);
        }

        public Eating Read(string id)
        {
            return eatings.FirstOrDefault(t => t.Id == id);
        }

        public IEnumerable<Eating> ReadAll()
        {
            return eatings;
        }

        public void Create(Eating eating)
        {
            eatings.Add(eating);
            JsonParser.WriteJson(eatings, filePath);
        }

        public void Update(Eating eating)
        {
            var old = eatings.FirstOrDefault(t => t.Id == eating.Id);

            old.Food = eating.Food;
            old.Fat = eating.Fat;
            old.Protein = eating.Protein;
            old.Carbohydrate = eating.Carbohydrate;
            old.Date = eating.Date;
            File.Delete(filePath);
            JsonParser.WriteJson(eatings, filePath);
        }

        public void Delete(string id)
        {
            eatings.Remove(Read(id));
            File.Delete(filePath);
            JsonParser.WriteJson(eatings, filePath);
        }

    }
}
