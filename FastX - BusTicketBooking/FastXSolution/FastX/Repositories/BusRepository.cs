using FastX.Contexts;
using FastX.Exceptions;
using FastX.Interfaces;
using FastX.Models;
using Microsoft.EntityFrameworkCore;

namespace FastX.Repositories
{
    public class BusRepository : IRepository<int, Bus>
    {
        private readonly FastXContext _context;
        private readonly ILogger<BusRepository> _logger;

        public BusRepository(FastXContext context, ILogger<BusRepository> logger)
        {
            _context = context;
            _logger = logger;
        }




        public async Task<Bus> Update(Bus item)
        {
            var bus = await GetAsync(item.BusId);
            _context.Entry<Bus>(item).State = EntityState.Modified;
            _context.SaveChanges();
            return item;
        }

        //--------------------------
        public async Task<Bus> Add(Bus bus)
        {
            try
            {
                _context.Buses.Add(bus);
                await _context.SaveChangesAsync();
                return bus;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding bus");
                throw; // Re-throw the exception for the caller to handle
            }
        }

        public async Task<Bus> Delete(int busId)
        {
            try
            {
                var bus = await _context.Buses.FindAsync(busId);
                if (bus == null)
                {
                    throw new BusNotFoundException();
                }
                _context.Buses.Remove(bus);
                await _context.SaveChangesAsync();
                return bus;
            }


            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting bus");
                throw; // Re-throw the exception for the caller to handle
            }
        }



        //public async Task<List<Bus>> GetBusesByCriteriaAsync(string origin, string destination, DateTime date)
        //{
        //    // Perform the database query to get buses based on origin, destination, and date
        //    return await _context.Buses
        //        .Where(b => b.Origin == origin && b.Destination == destination)
        //        // Add additional conditions based on your database schema
        //        .ToListAsync();
        //}

        //public async Task<List<Bus>> GetBusesByCriteriaAsyncWithBusType(string origin, string destination, DateTime date, string busType)
        //{
        //    var buses= await _context.Buses
        //        .Where(b => b.Origin == origin && b.Destination == destination&&b.BusType==busType).ToListAsync();
        //    return buses;
        //}

        public async Task<Bus> GetAsync(int key)
        {
            var buses = await GetAsync();
            var bus = await _context.Buses
        .Include(b => b.Seats)
        .FirstOrDefaultAsync(e => e.BusId == key);

            if (bus != null)
                return bus;
            throw new BusNotFoundException();
        }

        public async Task<List<Bus>> GetAsync()
        {
            var buses = await _context.Buses.Include(b => b.BusRoute).ThenInclude(b => b.Route).ToListAsync();
            //if (buses == null)
            //{
            //    throw new BusNotFoundException();  modified as below for test purpose
            //}
            if (buses == null || buses.Count == 0)
            {
                throw new BusNotFoundException();
            }

            return buses;
        }



    }
}