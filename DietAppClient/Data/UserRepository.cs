using DietAppClient.Helpers;
using DietAppClient.Models;

namespace DietAppClient.Data
{
    public class UserRepository : IUserRepository
    {
        string filePath;
        private User user;

        public UserRepository()
        {
            filePath = System.IO.Path.Combine(FileSystem.Current.AppDataDirectory, "user.json");
            user = JsonParser.ReadJson(new User() { Date = DateTime.Now }, filePath);
        }

        public User Read()
        {
            return user;
        }

        public void Update(User u)
        {
            user.Name = u.Name;
            user.Age = u.Age;
            user.Sex = u.Sex;
            user.Height = u.Height;
            user.Weight = u.Weight;
            user.FreeTimeActivity = u.FreeTimeActivity;
            user.WorkActivity = u.WorkActivity;
            user.Date = DateTime.Now;
            JsonParser.WriteJson(user, filePath);
        }

    }
}
