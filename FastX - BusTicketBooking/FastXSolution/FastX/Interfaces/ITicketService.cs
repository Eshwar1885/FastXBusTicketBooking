using FastX.Models;

namespace FastX.Interfaces
{
    public interface ITicketService
    {
        public Task<Ticket> AddTicket(Ticket ticket);
        public Task<List<Ticket>> GetTicketList();
        public Task<Ticket> GetTicket(int id);
        public Task<Ticket> DeleteTicket(int id);

    }
}