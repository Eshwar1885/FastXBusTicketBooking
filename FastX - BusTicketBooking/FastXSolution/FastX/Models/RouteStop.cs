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
        [ForeignKey("StopId")]
        public int StopId { get; set; }

        // Navigation Properties
        public Routee? Routee { get; set; }
        public Stop? Stop { get; set; }
    }

}