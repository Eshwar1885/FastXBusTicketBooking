using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FastX.Models
{
    public class BusRoute
    {
        [Key]
        public int BusRouteId { get; set; }
        public string? JourneyStatus { get; set; }
        [ForeignKey("RouteId")]
        public int RouteId { get; set; }
        public Routee? Route { get; set; } // Navigation property for associated route
        [ForeignKey("BusId")]
        public int BusId { get; set; }
        public Bus? Bus { get; set; }
    }
}