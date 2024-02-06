using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace FastX.Models
{
    public class RouteStop
    {
        [Key]
        public int RouteStopId { get; set; }
        public int RouteId { get; set; }
        public int StopId { get; set; }
        public int SequenceNumber { get; set; }

        // Navigation Properties
        public Routee Routee { get; set; }
        public Stop Stop { get; set; }
    }

}
