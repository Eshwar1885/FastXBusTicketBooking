using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FastX.Models
{
    public class Ticket
    {
        [Key]
        public int TicketId { get; set; }
        public int BookingId { get; set; }
        public string TicketNumber { get; set; }
        public decimal Price { get; set; }
        public bool IsConfirmed { get; set; }

        // Navigation Property
        public Booking Booking { get; set; }
    }

}
