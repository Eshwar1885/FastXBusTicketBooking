using System.ComponentModel.DataAnnotations.Schema;

namespace FastX.Models.DTOs
{
    public class AddBusAndSeatsDTO
    {
        public string? BusName { get; set; }
        public string? BusType { get; set; }
        public int TotalSeats { get; set; }
        public int BusOperatorId { get; set; }
        public int SeatPrice { get; set; }

    }
}
