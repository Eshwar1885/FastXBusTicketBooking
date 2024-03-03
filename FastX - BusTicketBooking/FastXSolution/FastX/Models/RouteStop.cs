using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace FastX.Models
{
    public class RouteStop
    {
        [Key]
        public int RouteStopId { get; set; }
        [ForeignKey("RouteId")]
        public int RouteId { get; set; }
        public Routee? Route { get; set; } // Navigation property for associated route
        [ForeignKey("StopId")]
        public int StopId { get; set; }
        public Stop? Stop { get; set; }
    }

}