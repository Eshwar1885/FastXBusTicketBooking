using System.ComponentModel.DataAnnotations;

namespace FastX.Models
{
    public class Routee
    {
        [Key]
        public int RouteId { get; set; }
        public string? Origin { get; set; }
        public string? Destination { get; set; }
        public DateTime? DepartureTime { get; set; }

        public DateTime? ArrivalTime { get; set; }
        public DateTime? TravelDate { get; set; }
        ////public int? Duration { get; set; }

        // //public ICollection<Booking>? Bookings { get; set; }
        public ICollection<RouteStop>? RouteStops { get; set; }
        public ICollection<BusRoute>? BusRoute { get; set; }
    }
}