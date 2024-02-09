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

        public BusRepository(FastXContext context)
        {
            _context = context;
        }
        public async Task<Bus> Add(Bus item)
        {
            _context.Add(item);
            _context.SaveChanges();
            return item;
        }

        public async Task<Bus> Delete(int key)
        {
            var bus=await GetAsync(key);
            _context?.Buses.Remove(bus);
            _context.SaveChanges();
            return bus;
        }

        public async Task<Bus> GetAsync(int key)
        {
            var buses = await GetAsync();
            var bus=buses.FirstOrDefault(e => e.BusId == key);
            if (bus != null)
                return bus;
            throw new NoSuchBusException();
        }

        public async Task<List<Bus>> GetAsync()
        {
            var buses = _context.Buses.Include(e => e.BusAmenities).ToList();
            if (buses == null)
            {
                throw new NoSuchBusException();
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
    }
}
