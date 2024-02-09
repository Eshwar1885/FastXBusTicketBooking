using FastX.Contexts;
using FastX.Interfaces;
using FastX.Models;

namespace FastX.Repositories
{
    public class AdminRepository : IRepository<int, Admin>
    {
        private readonly FastXContext _context;

        public AdminRepository(FastXContext context)
        {
            _context = context;
        }
        public async Task<Admin> Add(Admin item)
        {
            _context.Add(item);
            _context.SaveChanges();
            return item;
        }

        public Task<Admin> Delete(int key)
        {
            throw new NotImplementedException();
        }

        public Task<List<Admin>> GetAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Admin> GetAsync(int key)
        {
            throw new NotImplementedException();
        }

        public Task<Admin> Update(Admin item)
        {
            throw new NotImplementedException();
        }
    }
}
