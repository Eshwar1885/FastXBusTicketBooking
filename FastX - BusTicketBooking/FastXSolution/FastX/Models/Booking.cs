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
        public int? NumberOfSeats { get; set; }
        //public float? TotalPrice { get; set; }
        public string? Status { get; set; }

        // Foreign Keys
        [ForeignKey("UserId")]
        public int UserId { get; set; }
        public User? User { get; set; }


        [ForeignKey("BusId")]
        public int BusId { get; set; }
        public Bus? Bus { get; set; }



        public int RouteId { get; set; }
        [ForeignKey("RouteId")]
        public Routee? Routee { get; set; }


        // Navigation Properties
        //[ForeignKey("PaymentId")]
        //public int PaymentId { get; set; }
        //public Payment? Payments { get; set; }
        public ICollection<Ticket>? Tickets { get; set; }

    }
}