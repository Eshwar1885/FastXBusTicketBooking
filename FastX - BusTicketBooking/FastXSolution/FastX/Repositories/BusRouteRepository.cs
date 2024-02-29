using FastX.Contexts;
using FastX.Interfaces;
using FastX.Models;
using Microsoft.EntityFrameworkCore;

namespace FastX.Repositories
{
    public class BusRouteRepository : IRepository<int, BusRoute>
    {
        private readonly FastXContext _context;
        private readonly ILogger<BusRouteRepository> _logger;

        public BusRouteRepository(FastXContext context, ILogger<BusRouteRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<BusRoute> Add(BusRoute item)
        {
            try
            {
                _context.BusRoute.Add(item);
                await _context.SaveChangesAsync();
                return item;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding busroute");
                throw; // Re-throw the exception for the caller to handle
            }
        }

        public Task<BusRoute> Delete(int key)
        {
            throw new NotImplementedException();
        }

        public Task<List<BusRoute>> GetAsync()
        {
            throw new NotImplementedException();
        }

        public Task<BusRoute> GetAsync(int key)
        {
            throw new NotImplementedException();
        }

        public Task<BusRoute> Update(BusRoute item)
        {
            throw new NotImplementedException();
        }
    }
}
