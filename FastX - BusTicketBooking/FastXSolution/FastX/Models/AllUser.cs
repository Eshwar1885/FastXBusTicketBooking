using System.ComponentModel.DataAnnotations;

namespace FastX.Models
{
    public class AllUser
    {
        [Key]
        public string Username { get; set; }
        public byte[] Password { get; set; }
        public string Role { get; set; }
        public byte[] Key { get; set; }


        // Specific properties for each role
        public User? User { get; set; }
        public BusOperator? BusOperator { get; set; }
        public Admin? Admin { get; set; }
    }

}
