using DietAppClient.Models;
using Microsoft.Maui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DietAppClient.Helpers;

namespace DietAppClient.Data
{

    public class UserRepository : IUserRepository
    {
        string filePath;
        private User user;

        public UserRepository()
        {
            filePath = System.IO.Path.Combine(FileSystem.Current.AppDataDirectory, "user.json");
            user = JsonParser.ReadJson(new User(), filePath);
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
            JsonParser.WriteJson(user, filePath);
        }

    }
}
