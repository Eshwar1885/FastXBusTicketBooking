using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FastX.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        public string? Password { get; set; }
        public string? Name { get; set; }
        public string? Gender { get; set; }
        public string? ContactNumber { get; set; }
        public string? Address { get; set; }
        public string Username { get; set; }
        [ForeignKey("Username")]
        public AllUser? AllUser { get; set; }

        // Navigation Property
        public ICollection<Booking>? Bookings { get; set; }
    }
}
