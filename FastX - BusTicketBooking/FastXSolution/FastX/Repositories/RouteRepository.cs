using FastX.Contexts;
using FastX.Interfaces;
using FastX.Models;
using Microsoft.EntityFrameworkCore;

namespace FastX.Repositories
{
    public class RouteRepository : IRepository<int, Routee>
    {
        private readonly FastXContext _context;
        public RouteRepository(FastXContext context)
        {
            _context = context;
        }

        public async Task<Routee> Add(Routee item)
        {
            _context.Add(item);
            _context.SaveChanges();
            return item;
        }

        public Task<Routee> Delete(int key)
        {
            throw new NotImplementedException();
        }

        public Task<List<Routee>> GetAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Routee> GetAsync(int key)
        {
            throw new NotImplementedException();
        }

        public Task<Routee> Update(Routee item)
        {
            throw new NotImplementedException();
        }
    }
}