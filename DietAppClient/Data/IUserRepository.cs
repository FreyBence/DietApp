using DietAppClient.Models;

namespace DietAppClient.Data
{
    public interface IUserRepository
    {
        User Read();
        void Update(User u);
    }
}