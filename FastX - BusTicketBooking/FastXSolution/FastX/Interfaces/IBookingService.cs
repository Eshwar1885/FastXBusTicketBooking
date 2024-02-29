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



    }

}