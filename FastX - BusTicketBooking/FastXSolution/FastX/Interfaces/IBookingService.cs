using FastX.Models;
using FastX.Models.DTOs;

namespace FastX.Interfaces
{
    public interface IBookingService
    {
        public Task ChangeNoOfSeatsAsync(int id, int noOfSeats);
        public Task<Booking> MakeBooking(int busId, List<int>seatIds, DateTime travelDate, int userId, int totalSeats);
        public Task<List<CompletedBookingDTO>> GetCompletedBookings(int userId);
        public Task<List<CompletedBookingDTO>> GetUpcomingBookings(int userId);
        public Task<List<CompletedBookingDTO>> GetPastBookings(int userId);
        public Task UpdateOngoingBookingsAndResetSeats();
        public Task<List<Booking>> GetBookingList();
        public Task<Booking> CancelBooking(int userId, int bookingId);
        public Task<List<CompletedBookingDTO>> GetCancelledBookings(int userId);
        public Task<Bus> GetBookingInfo(int bookingId, DateTime? bookedForWhichDate);










    }

}