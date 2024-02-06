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
        public DateTime BookingDate { get; set; }
        public int NumberOfSeats { get; set; }
        public decimal TotalPrice { get; set; }

        // Foreign Keys
        [ForeignKey("User")]
        public int UserId { get; set; }
        [ForeignKey("Bus")]
        public int BusId { get; set; }

        // Navigation Properties
        public User User { get; set; }
        public Bus Bus { get; set; }
        public Payment Payment { get; set; }
    }
}