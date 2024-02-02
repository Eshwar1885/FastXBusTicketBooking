using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FastX.Models
{
    public class Payment
    {
        [Key]
        public int PaymentId { get; set; }
        [Required(ErrorMessage = "Amount has to be provided")]
        public float Amount { get; set; }
        public string Status { get; set; }
        public string? PaymentMethod { get; set; }

        //public int UserId { get; set; }
        //[ForeignKey("UserId")]
        //public User? User { get; set; }

        public int TicketId { get; set; }
        [ForeignKey("TicketId")]
        public TicketBooking? TicketBooking { get; set; }

    }
}
