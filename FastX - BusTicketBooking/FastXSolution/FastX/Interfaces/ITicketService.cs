using FastX.Models;
using FastX.Models.DTOs;

namespace FastX.Interfaces
{
    public interface ITicketService
    {
        public Task<Ticket> AddTicket(Ticket ticket);
        public Task<List<Ticket>> GetTicketList();
        public Task<Ticket> GetTicket(int id);
        public Task<Ticket> DeleteTicket(int id);
        public Task<List<TicketDTO>> GetTicketsForUser(int id );
        public Task DeleteCancelledBookingTickets(int bookingId, int userId);


    }
}