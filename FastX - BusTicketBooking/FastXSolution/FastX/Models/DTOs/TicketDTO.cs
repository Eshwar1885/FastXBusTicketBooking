using System;

namespace FastX.Models.DTOs
{
    public class TicketDTO
    {
        public int TicketId { get; set; }
        public string BusName { get; set; }
        public float? TicketPrice { get; set; }
        public int SeatNumber { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public DateTime? JourneyDate { get; set; }
    }
}
