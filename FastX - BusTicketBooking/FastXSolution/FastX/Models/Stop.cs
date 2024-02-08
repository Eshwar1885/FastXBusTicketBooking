using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FastX.Models
{
    public class Stop
    {
        [Key]
        public int StopId { get; set; }
        public string? Name { get; set; }
        public ICollection<RouteStop>? RouteStops { get; set; }
        //public string? Location { get; set; }
        //[ForeignKey("RouteId")]
        //public int RouteId { get; set; }
        //public Routee? Routee { get; set; }

    }

}