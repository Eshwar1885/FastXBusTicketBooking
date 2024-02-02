using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FastX.Models
{
    public class SeatDetails
    {
        [Key]
        public int Id { get; set; }
        public int SeatNo { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }


        [ForeignKey("BusId")]
        public int BusId { get; set; }

    }
}
