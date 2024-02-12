namespace FastX.Interfaces
{
    public interface IBookingService
    {
        public Task ChangeNoOfSeatsAsync(int id, int noOfSeats);
        public Task MakeBooking(int busId, int seatId, DateTime travelDate, int userId);
    }

}