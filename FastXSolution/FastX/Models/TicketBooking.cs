using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.Sockets;

namespace FastX.Models
{
    public class TicketBooking
    {
        [Key]
        public int TicketBookingId { get; set; }

        [ForeignKey("UserId")]
        public int UserId { get; set; }
        public User? User { get; set; }

        [ForeignKey("BusId")]
        public int BusId { get; set; }
        public Bus? Bus { get; set; }

        public int SeatsBooked { get; set; }
        public float TotalPrice { get; set; }
        public DateTime DateBooked { get; set; }

    }
}