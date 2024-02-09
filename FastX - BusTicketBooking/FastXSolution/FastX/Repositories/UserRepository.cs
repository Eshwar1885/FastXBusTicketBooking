using FastX.Contexts;
using FastX.Interfaces;
using FastX.Models;

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

        public Task<List<User>> GetAsync()
        {
            throw new NotImplementedException();
        }

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
