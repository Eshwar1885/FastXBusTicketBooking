using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace FastX.Models
{
    public class RouteDetails
    {
        [Key]
        public int RouteDetailsId { get; set; }

        public string Origin { get; set; }
        public string Destination { get; set; }
        public float? Duration { get; set; }



        public ICollection<Stops> RouteStops { get; set; }
    }
}
