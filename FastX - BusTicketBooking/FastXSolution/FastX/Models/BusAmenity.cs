using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FastX.Models
{
    public class BusAmenity
    {
        [Key]
        public int BusAmenityId { get; set; }
        [ForeignKey("BusId")]
        public int BusId { get; set; }
        [ForeignKey("AmenityId")]
        public int AmenityId { get; set; }

        // Navigation Properties
        public Bus? Bus { get; set; }
        public Amenity? Amenity { get; set; }
    }
}
