using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FastX.Models
{
    public class Amenities
    {
        [Key]
        public int AmenityId { get; set; }
        public string AmenityName { get; set; }

        [ForeignKey("BusId")]
        public int BusId { get; set; }
        public Bus? Bus { get; set; }

    }
}
