using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FastX.Models
{
    public class Seat
    {
        [Key]
        public int SeatId { get; set; }
        public float SeatPrice { get; set; }
        [ForeignKey("BusId")]
        public int BusId { get; set; }
        public Bus? Bus { get; set; }

        public bool? IsAvailable { get; set; }

        // Navigation Property
        //public Booking? Booking { get; set; }


    }

}