using FastX.Contexts;
using FastX.Interfaces;
using FastX.Models;
using Microsoft.EntityFrameworkCore;

namespace FastX.Repositories
{
    public class BookingRepository : IBookingRepository<int, Booking>
    {
        private readonly FastXContext _context;

        public BookingRepository(FastXContext context)
        {
            _context = context;
        }
        public async Task<Booking> Add(Booking item)
        {
            _context.Add(item);
            _context.SaveChanges();
            return item;
        }

        public async Task<Booking> Delete(int key)
        {
            var booking = await GetAsync(key);
            _context.Bookings.Remove(booking);
            _context.SaveChanges();
            return booking;
        }

        public async Task<List<Booking>> GetAsync()
        {
            var bookings = await _context.Bookings.ToListAsync(); //
            return bookings;
        }

        public async Task<Booking> GetAsync(int key)
        {
            var bookings = await GetAsync();
            var booking = bookings.FirstOrDefault(e => e.BookingId == key);
            if (booking != null)
                return booking;
            // throw new NoBookingsAvailableException();
            return null;
        }

        public async Task<Booking?> GetOngoingBookingAsync(int busId, int userId, DateTime travelDate)
        {
            return await _context.Bookings
           .Where(b => b.BusId == busId && b.UserId == userId && b.BookedForWhichDate == travelDate && b.Status == "ongoing")
           .FirstOrDefaultAsync();
        }

        public async Task<Booking> Update(Booking item)
        {
            var booking = await GetAsync(item.BookingId);
            _context.Entry<Booking>(item).State = EntityState.Modified;
            _context.SaveChanges();
            return item;
        }

    }
}