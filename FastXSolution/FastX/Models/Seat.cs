using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FastX.Models
{
    public class Seat
    {
        [Key]
        public int SeatId { get; set; }
        public int BusId { get; set; }
        public bool IsAvailable { get; set; }

        // Navigation Property
        public Bus Bus { get; set; }
    }

}
