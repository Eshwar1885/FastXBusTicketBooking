using System.ComponentModel.DataAnnotations;

namespace FastX.Models
{
    public class Routee
    {
        [Key]
        public int RouteId { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public decimal Fare { get; set; }

        // Navigation Property
        public ICollection<RouteStop> RouteStops { get; set; }
    }
}
