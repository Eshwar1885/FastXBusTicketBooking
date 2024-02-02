using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FastX.Models
{
    public class Bus
    {
        [Key]
        public int BusId { get; set; }
        public string BusName { get; set; }
        public string BusNumber { get; set; }
        public string BusType { get; set; }
        public int NumberOfSeats { get; set; }

        [ForeignKey("BusOperatorId")]
        public int BusOperatorId { get; set; }
        public BusOperator? BusOperator { get; set; }

        [ForeignKey("AmenityId")]
        public int AmenityId { get; set; }
        public ICollection<Amenities> Amenitiess { get; set; }

        [ForeignKey("SeatDetailsId")]
        public int SeatDetailsId { get; set; }
        public SeatDetails SeatDetails { get; set; }

    }
}
