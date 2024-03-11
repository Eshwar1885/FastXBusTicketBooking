using System.ComponentModel.DataAnnotations.Schema;

namespace FastX.Models.DTOs
{
    public class SeatDTOForUser
    {
        public int SeatId { get; set; }
        public float SeatPrice { get; set; }
        public bool? IsAvailable { get; set; }
    }
}