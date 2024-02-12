using FastX.Contexts;

using FastX.Exceptions;
using FastX.Interfaces;
using FastX.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace FastX.Repositories
{
    public class SeatRepository : ISeatRepository<int, Seat>
    {
        private readonly FastXContext _context;

        public SeatRepository(FastXContext context)
        {
            _context = context;
        }
        public Task<Seat> Add(Seat item)
        {
            throw new NotImplementedException();
        }

        public Task<Seat> Delete(int key)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Seat>> GetAsync()
        {
            var seats = await _context.Seats.ToListAsync();

            if (seats == null)
            {
                throw new NoSeatsAvailableException();
            }

            return seats;
        }




        public async Task<Seat> GetAsync(int key1, int key2)
        {
            var seats = await GetAsync();
            var seat = await _context.Seats
        .FirstOrDefaultAsync(e => e.BusId == key1 && e.SeatId == key2);

            if (seat != null)
                return seat;
            throw new NoSeatsAvailableException();
        }

        public async Task<Seat> GetAsync(int key)
        {
            var seats = await GetAsync();
            var seat = seats.FirstOrDefault(e => e.SeatId == key);
            if (seat != null)
                return seat;
            // throw new NoBookingsAvailableException();
            return null;
        }

        public async Task<Seat> Update(Seat item)
        {
            var seat = await GetAsync(item.SeatId);
            _context.Entry<Seat>(item).State = EntityState.Modified;
            _context.SaveChanges();
            return item;
        }
    }
}