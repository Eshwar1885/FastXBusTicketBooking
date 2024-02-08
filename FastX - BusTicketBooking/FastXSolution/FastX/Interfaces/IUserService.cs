using FastX.Models;
using System.Numerics;

namespace FastX.Interfaces
{
    public interface IUserService
    {
        public Task<User> AddUser(User user);

        public Task<List<User>> GetUserList();
        public Task<User> GetUser(int id);

        public Task<User> DeleteUser(int id);


    }
}
