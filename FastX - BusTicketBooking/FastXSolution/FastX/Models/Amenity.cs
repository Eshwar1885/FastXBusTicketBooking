using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FastX.Models
{
    public class Amenity
    {
        [Key]
        public int AmenityId { get; set; }
        //[Required(ErrorMessage = "Name is required")]
        public string? Name { get; set; }
        public ICollection<BusAmenity>? BusAmenities { get; set; }

    }

}
