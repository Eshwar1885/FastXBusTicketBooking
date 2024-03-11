namespace FastX.Models.DTOs
{
    public class BookingDTO
    {
        public int BusId { get; set; }
        public List<int> SeatIds { get; set; }
        public DateTime TravelDate { get; set; }
        public int UserId { get; set; }
        public int TotalSeats { get; set; }
    }

}
