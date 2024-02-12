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
            _logger = logger;      // ???? logger added in repository
        }


        //public async Task<Bus> Add(Bus item)
        //{
        //    _context.Add(item);
        //    _context.SaveChanges();
        //    return item;
        //}

        //public async Task<Bus> Delete(int key)
        //{
        //    var bus=await GetAsync(key);
        //    _context?.Buses.Remove(bus);
        //    _context.SaveChanges();
        //    return bus;
        //}

        public async Task<Bus> GetAsync(int key)
        {
            var buses = await GetAsync();
            var bus=buses.FirstOrDefault(e => e.BusId == key);
            if (bus != null)
                return bus;
            throw new BusNotFoundException();
        }

        public async Task<List<Bus>> GetAsync()
        {
            var buses = _context.Buses.Include(e => e.BusAmenities).ToList();
            if (buses == null)
            {
                throw new BusNotFoundException();
            }
            return buses;
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


    }
}
