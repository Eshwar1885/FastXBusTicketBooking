using System.ComponentModel.DataAnnotations;

namespace FastX.Models
{
    public class Routee
    {
        [Key]
        public int RouteId { get; set; }

        //[Required(ErrorMessage = "Origin is required")]
        public string? Origin { get; set; }

        //[Required(ErrorMessage = "Destination is required")]
        public string? Destination { get; set; }
        public DateTime? DepartureTime { get; set; }
        public DateTime? ArrivalTime { get; set; }
        public DateTime? TravelDate { get; set; }
        public ICollection<RouteStop>? RouteStops { get; set; }
        public ICollection<BusRoute>? BusRoute { get; set; }
    }
}