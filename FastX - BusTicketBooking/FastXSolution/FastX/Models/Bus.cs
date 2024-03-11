using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FastX.Models
{
    public class Bus
    {
        [Key]
        public int BusId { get; set; }
        public string? BusName { get; set; }
        public string? BusType { get; set; }
        public int TotalSeats { get; set; }
        // Foreign Key
        [ForeignKey("BusOperatorId")]
        public int BusOperatorId { get; set; }
        // Navigation Property
        public BusOperator? BusOperator { get; set; }
        public ICollection<Booking>? Bookings { get; set; }
        public ICollection<Seat>? Seats { get; set; }
        public ICollection<BusAmenity>? BusAmenities { get; set; }

        public ICollection<BusRoute>? BusRoute { get; set; }

    }
}