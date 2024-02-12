using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FastX.Models
{
    public class BusOperator
    {
        [Key]
        public int BusOperatorId { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string? Name { get; set; }
        public string? Gender { get; set; }
        public string? ContactNumber { get; set; }
        public string? Address { get; set; }

        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }

        [ForeignKey("Username")]
        public AllUser? AllUser { get; set; }

        // Navigation Property
        public ICollection<Bus>? Buses { get; set; }
    }
}
