using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.Sockets;
using System.Reflection.PortableExecutable;

namespace FastX.Models
{
    public class Booking
    {
        [Key]
        public int BookingId { get; set; }
        public DateTime? BookingDate { get; set; }
        public int NumberOfSeats { get; set; }
        public string? Status { get; set; }
        public DateTime BookedForWhichDate { get; set; }
        // Foreign Keys
        [ForeignKey("UserId")]
        public int UserId { get; set; }
        public User? User { get; set; }
        [ForeignKey("BusId")]
        public int BusId { get; set; }
        public Bus? Bus { get; set; }
        public ICollection<Ticket>? Tickets { get; set; }

    }
}