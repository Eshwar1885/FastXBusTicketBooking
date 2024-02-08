using FastX.Contexts;
using FastX.Exceptions;
using FastX.Interfaces;
using FastX.Models;
using Microsoft.EntityFrameworkCore;

namespace FastX.Repositories
{
    public class TicketRepository : IRepository<int, Ticket>
    {
        private readonly FastXContext _context;

        public TicketRepository(FastXContext context)
        {
            _context = context;
        }
        public async Task<Ticket> Add(Ticket item)
        {
            _context.Add(item);
            _context.SaveChanges();
            return item;
        }

        public async Task<Ticket> Delete(int key)
        {
            var ticket = await GetAsync(key);
            _context?.Tickets.Remove(ticket);
            _context.SaveChanges();
            return ticket;
        }

        public async Task<List<Ticket>> GetAsync()
        {
            var tickets = _context.Tickets.Include(e => e.Booking).ToList(); //
            return tickets;
        }

        public async Task<Ticket> GetAsync(int key)
        {
            var tickets = await GetAsync();
            var ticket = tickets.FirstOrDefault(e => e.TicketId == key);
            if (ticket != null)
                return ticket;
            throw new NoTicketsAvailableException();
        }

        public async Task<Ticket> Update(Ticket item)
        {
            var ticket = await GetAsync(item.TicketId);
            _context.Entry<Ticket>(item).State = EntityState.Modified;
            _context.SaveChanges();
            return item;
        }
    }
}
