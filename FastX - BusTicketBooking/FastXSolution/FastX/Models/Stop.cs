using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FastX.Models
{
    public class Stop
    {
        [Key]
        public int StopId { get; set; }

        //[Required(ErrorMessage = "Name is required")]
        public string? Name { get; set; }
        public ICollection<RouteStop>? RouteStops { get; set; }


    }

}