using FastX.Exceptions;
using FastX.Interfaces;
using FastX.Models;
using FastX.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Net.Sockets;

namespace FastX.Services
{
    public class TicketService : ITicketService
    {
        private IRepository<int, Ticket> _ticketRepository;
        private readonly ILogger<TicketService> _logger;
        // private IRepository<int, BusOperator> _busOperatorRepository;

        //private readonly ILogger<RouteeService> _logger;
        public TicketService(IRepository<int, Ticket> ticketRepository,
            //IRepository<int, BusOperator> busOperatorRepository, 
            ILogger<TicketService> logger)
        {
            _ticketRepository = ticketRepository;
            //_busOperatorRepository = busOperatorRepository;
            _logger = logger;

        }
        public Task<Ticket> AddTicket(Ticket ticket)
        {
            throw new NotImplementedException();
        }

        public Task<Ticket> DeleteTicket(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Ticket> GetTicket(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Ticket>> GetTicketList()
        {
            var ticket = await _ticketRepository.GetAsync();
            //if (ticket == null)
            //{
            //    throw new NoTicketsAvailableException();
            //}
            return ticket;
        }
    }
}