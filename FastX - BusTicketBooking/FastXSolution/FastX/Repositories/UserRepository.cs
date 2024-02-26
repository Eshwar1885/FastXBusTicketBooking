using FastX.Contexts;
using FastX.Interfaces;
using FastX.Models;
using Microsoft.EntityFrameworkCore;

namespace FastX.Repositories
{
    public class UserRepository : IRepository<int, User>
    {
        private readonly FastXContext _context;

        public UserRepository(FastXContext context)
        {
            _context = context;
        }
        public async Task<User> Add(User item)
        {
            _context.Add(item);
            _context.SaveChanges();
            return item;
        }

        public Task<User> Delete(int key)
        {
            throw new NotImplementedException();
        }

        public async Task<List<User>> GetAsync()
        {
            var users = await _context.Users.ToListAsync();
            return users;
        }

        //public async Task<User> GetAsync(string key)
        //{
        //    var user = _context.Users.SingleOrDefault(u => u.Username == key);
        //    return user;

        //}

        public Task<User> GetAsync(int key)
        {
            throw new NotImplementedException();
        }

        public Task<User> Update(User item)
        {
            throw new NotImplementedException();
        }
    }
}
