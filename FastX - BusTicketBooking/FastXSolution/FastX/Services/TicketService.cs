using FastX.Exceptions;
using FastX.Interfaces;
using FastX.Models;
using FastX.Models.DTOs;
using FastX.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Net.Sockets;

namespace FastX.Services
{
    public class TicketService : ITicketService
    {
        private IRepository<int, Ticket> _ticketRepository;
        private readonly ILogger<TicketService> _logger;
        private readonly IRepository<int, Bus> _busRepository;

        public TicketService(IRepository<int, Ticket> ticketRepository, IRepository<int,Bus> busRepository,
            ILogger<TicketService> logger)
        {
            _ticketRepository = ticketRepository;
            _logger = logger;
            _busRepository = busRepository;

        }
        public async Task<Ticket> AddTicket(Ticket ticket)
        {
            if (ticket == null)
            {
                throw new ArgumentNullException(nameof(ticket));
            }

            try
            {
                return await _ticketRepository.Add(ticket);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding ticket");
                throw;
            }
        }

        public async Task<Ticket> DeleteTicket(int id)
        {
            try
            {
                return await _ticketRepository.Delete(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while deleting ticket with ID: {id}");
                throw;
            }
        }

        public async Task<Ticket> GetTicket(int id)
        {
            try
            {
                return await _ticketRepository.GetAsync(id);
            }
            catch (NoTicketsAvailableException ex)
            {
                _logger.LogError(ex, $"Ticket with ID: {id} not found");
                throw;
            }
        }

        public async Task<List<Ticket>> GetTicketList()
        {
            try
            {
                return await _ticketRepository.GetAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting list of tickets");
                throw;
            }
        }

        //public async Task<List<TicketDTO>> GetTicketsForUser(int userId)
        //{
        //    try
        //    {
        //        var tickets = await _ticketRepository.GetAsync();

        //        if (tickets == null || !tickets.Any())
        //        {
        //            throw new NoTicketsAvailableException();
        //        }

        //        var userTickets = tickets.Where(t => t.Booking != null && t.Booking.UserId == userId).ToList();

        //        if (!userTickets.Any())
        //        {
        //            throw new NoTicketsAvailableException();
        //        }

        //        return userTickets.Select(ticket => new TicketDTO
        //        {
        //            TicketId = ticket.TicketId,
        //            BusName = ticket.Booking.Bus?.BusName,
        //            TicketPrice = ticket.Price ?? 0,
        //            SeatNumber = ticket.SeatId,
        //            Origin = ticket.Booking.Bus?.BusRoute?.Select(Routee)
        //            Destination = ticket.Booking.Bus?.Route?.Destination
        //        }).ToList();
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, $"An error occurred while retrieving tickets for user with ID: {userId}");
        //        throw;
        //    }
        //}

        public async Task<List<TicketDTO>> GetTicketsForUser(int userId)
        {
            try
            {
                var tickets = await _ticketRepository.GetAsync();

                if (tickets == null || !tickets.Any())
                {
                    throw new NoTicketsAvailableException();
                }

                var userTickets = tickets.Where(t => t.Booking != null && t.Booking.UserId == userId).ToList();

                if (!userTickets.Any())
                {
                    throw new NoTicketsAvailableException();
                }

                var ticketDTOs = new List<TicketDTO>();

                foreach (var ticket in userTickets)
                {
                    var bus = await _busRepository.GetAsync(ticket.BusId);
                    if (bus != null && bus.BusRoute != null && bus.BusRoute.Any())
                    {
                        foreach (var busRoute in bus.BusRoute)
                        {
                            var route = busRoute.Route;
                            if (route != null)
                            {
                                var ticketDTO = new TicketDTO
                                {
                                    TicketId = ticket.TicketId,
                                    BusName = bus.BusName,
                                    TicketPrice = ticket.Price ?? 0,
                                    SeatNumber = ticket.SeatId,
                                    Origin = route.Origin,
                                    Destination = route.Destination,
                                    JourneyDate = ticket.Booking.BookedForWhichDate,
                                };
                                ticketDTOs.Add(ticketDTO);
                            }
                        }
                    }
                }

                return ticketDTOs;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while retrieving tickets for user with ID: {userId}");
                throw;
            }
        }
    }
}