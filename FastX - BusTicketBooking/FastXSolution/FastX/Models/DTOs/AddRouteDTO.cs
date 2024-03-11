namespace FastX.Models.DTOs
{
    public class AddRouteDTO
    {
        public string? Origin { get; set; }
        public int BusId { get; set; }
        public string? Destination { get; set; }
        public DateTime TravelDate { get; set; }
    }
}
