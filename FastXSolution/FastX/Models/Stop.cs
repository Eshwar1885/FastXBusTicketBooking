using System.ComponentModel.DataAnnotations;

namespace FastX.Models
{
    public class Stop
    {
        [Key]
        public int StopId { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }

        public ICollection<RouteStop> RouteStops { get; set; }

    }

}
