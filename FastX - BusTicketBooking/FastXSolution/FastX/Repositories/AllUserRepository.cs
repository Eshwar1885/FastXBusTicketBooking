using FastX.Contexts;
using FastX.Exceptions;
using FastX.Interfaces;
using FastX.Models;
using Microsoft.EntityFrameworkCore;

namespace FastX.Repositories
{
    public class AllUserRepository : IRepository<string, AllUser>
    {
        private readonly FastXContext _context;

        public AllUserRepository(FastXContext context)
        {
            _context = context;
        }
        public async Task<AllUser> Add(AllUser item)
        {
            _context.Add(item);
            _context.SaveChanges();
            return item;
        }

        public async Task<AllUser> Delete(string key)
        {
            var allUser = await GetAsync(key);
            _context?.AllUsers.Remove(allUser);
            _context.SaveChanges();
            return allUser;
        }

        public async Task<AllUser> GetAsync(string key)
        {
            var allUser = _context.AllUsers.SingleOrDefault(u => u.Username == key);
            return allUser;
        }

        public async Task<List<AllUser>> GetAsync()
        {
            var allUsers = await _context.AllUsers.ToListAsync();
            return allUsers;
        }

        public async Task<AllUser> Update(AllUser item)
        {
            var allUser = await GetAsync(item.Username);
            if (allUser != null)
            {
                _context.Entry<AllUser>(item).State = EntityState.Modified;
                _context.SaveChanges();
                return item;
            }
            return null;
        }
    }
}
