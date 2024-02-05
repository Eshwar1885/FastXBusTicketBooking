using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FastX.Models
{
    public class Payment
    {
        [Key]
        public int PaymentId { get; set; }
        public int BookingId { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }

        // Navigation Property
        public Booking Booking { get; set; }
    }
}
