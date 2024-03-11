using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FastX.Models
{
    public class Seat
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int SeatId { get; set; }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int BusId { get; set; }
        public float SeatPrice { get; set; }
        [ForeignKey("BusId")]
        // public int BusId { get; set; }
        public Bus? Bus { get; set; }
        public bool? IsAvailable { get; set; }
    }

}