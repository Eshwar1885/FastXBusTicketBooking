namespace FastX.Models.DTOs
{
    public class BusDTOForUser
    {
        public int BusId { get; set; }
        public string BusName { get; set; }
        public string? BusType { get; set; }
        public int? TotalSeats { get; set; }
        public string? Origin { get; set; }
        public string? Destination { get; set; }
    }
}