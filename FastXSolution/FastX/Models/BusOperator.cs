using System.ComponentModel.DataAnnotations;

namespace FastX.Models
{
    public class BusOperator
    {
        [Key]
        public int BusOperatorId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }

        public ICollection<Bus> Buses { get; set; }
    }

}
