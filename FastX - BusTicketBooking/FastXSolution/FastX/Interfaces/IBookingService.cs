using FastX.Models;

namespace FastX.Interfaces
{
    public interface IBookingService
    {
        public Task ChangeNoOfSeatsAsync(int id, int noOfSeats);
        public Task<Booking> MakeBooking(int busId, List<int>seatIds, DateTime travelDate, int userId, int totalSeats);

    }

}