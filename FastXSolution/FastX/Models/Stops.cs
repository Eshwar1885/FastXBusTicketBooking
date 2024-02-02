using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FastX.Models
{
    public class Stops
    {
        [Key]
        public int StopId { get; set; }
        public string Name { get; set; }


        public int RouteDetailsId { get; set; }

        [ForeignKey("RouteDetailsId")]
        public RouteDetails? RouteDetails { get; set; }

    }
}
