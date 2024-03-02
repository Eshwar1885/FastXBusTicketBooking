using FastX.Models;
using FastX.Models.DTOs;

namespace FastX.Interfaces
{
    public interface ISeatService
    {
        public Task<Seat> AddSeat(Seat seat);
        public Task<List<Seat>> GetSeatList();
        public Task<Seat> GetSeat(int id);
        public Task<Seat> DeleteSeat(int id);

        public Task<List<SeatDTOForUser>> GetAvailableSeats(int busId);
        public Task ChangeJourneyStatus();
        public Task<bool> CheckWhetherSeatIsAvailableForBooking(int busId, int seatId, DateTime date);
        public Task ChangeSeatAvailablityAsync(int seatId, int busId);
        public Task ChangeSeatAvailablity(int seatId, int busId);


        public Task<float> GetSeatPriceAsync(int seatId, int busId);
        public Task ChangeSeatAvailabilityForCancelledBookings();

    }
}
