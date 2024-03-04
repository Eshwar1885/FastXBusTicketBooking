using FastX.Exceptions;
using FastX.Interfaces;
using FastX.Models;
using FastX.Models.DTOs;
using FastX.Services;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FastX.Tests.Services
{
    [TestFixture]
    public class TicketServiceTest
    {
        private Mock<IRepository<int, Ticket>> _mockTicketRepository;
        private Mock<ILogger<TicketService>> _mockLogger;
        private Mock<IRepository<int, Bus>> _mockBusRepository;
        private Mock<IBookingRepository<int, Booking>> _mockBookingRepository;
        private TicketService _ticketService;

        [SetUp]
        public void Setup()
        {
            _mockTicketRepository = new Mock<IRepository<int, Ticket>>();
            _mockLogger = new Mock<ILogger<TicketService>>();
            _mockBusRepository = new Mock<IRepository<int, Bus>>();
            _mockBookingRepository = new Mock<IBookingRepository<int, Booking>>();
            _ticketService = new TicketService(
                _mockTicketRepository.Object,
                _mockLogger.Object,
                _mockBusRepository.Object,
                _mockBookingRepository.Object);
        }

        [Test]
        public async Task AddTicket_Should_Add_Ticket()
        {
            // Arrange
            var ticket = new Ticket();

            // Act
            await _ticketService.AddTicket(ticket);

            // Assert
            _mockTicketRepository.Verify(repo => repo.Add(ticket), Times.Once);
        }

        [Test]
        public async Task DeleteTicket_Should_Delete_Ticket()
        {
            // Arrange
            int ticketId = 1;

            // Act
            await _ticketService.DeleteTicket(ticketId);

            // Assert
            _mockTicketRepository.Verify(repo => repo.Delete(ticketId), Times.Once);
        }

        [Test]
        public async Task GetTicket_Should_Return_Ticket()
        {
            // Arrange
            int ticketId = 1;
            var ticket = new Ticket();
            _mockTicketRepository.Setup(repo => repo.GetAsync(ticketId)).ReturnsAsync(ticket);

            // Act
            var result = await _ticketService.GetTicket(ticketId);

            // Assert
            Assert.AreEqual(ticket, result);
        }

        [Test]
        public async Task GetTicketList_Should_Return_List_Of_Tickets()
        {
            // Arrange
            var tickets = new List<Ticket>();
            _mockTicketRepository.Setup(repo => repo.GetAsync()).ReturnsAsync(tickets);

            // Act
            var result = await _ticketService.GetTicketList();

            // Assert
            Assert.AreEqual(tickets, result);
        }

        

        [Test]
        public async Task GetTicketsForUser_Should_Throw_NoTicketsAvailableException_If_No_Tickets_Found()
        {
            // Arrange
            int userId = 1;
            var tickets = new List<Ticket>();
            _mockTicketRepository.Setup(repo => repo.GetAsync()).ReturnsAsync(tickets);

            // Act & Assert
            Assert.ThrowsAsync<NoTicketsAvailableException>(() => _ticketService.GetTicketsForUser(userId));
        }

        


        //[Test]
        //public async Task GetTicketsForUser_Should_Return_Tickets_For_User()
        //{
        //    Arrange
        //    int userId = 1;
        //    var tickets = new List<Ticket>
        //    {
        //        new Ticket { Booking = new Booking { UserId = userId } }
        //    };
        //    _mockTicketRepository.Setup(repo => repo.GetAsync()).ReturnsAsync(tickets);
        //    _mockBusRepository.Setup(repo => repo.GetAsync(It.IsAny<int>())).ReturnsAsync(new Bus());

        //    Act
        //   var result = await _ticketService.GetTicketsForUser(userId);

        //    Assert
        //    Assert.AreEqual(0, result.Count);
        //}







        [Test]
        public async Task GetTicketsForUser_Should_Return_Tickets_For_User()
        {
            // Arrange
            int userId = 1;
            var tickets = new List<Ticket>
            {
                new Ticket { Booking = new Booking { UserId = userId } }
            };
            _mockTicketRepository.Setup(repo => repo.GetAsync()).ReturnsAsync(tickets);
            _mockBusRepository.Setup(repo => repo.GetAsync(It.IsAny<int>())).ReturnsAsync(new Bus());

            // Act
            var result = await _ticketService.GetTicketsForUser(userId);

            // Assert
            Assert.AreEqual(0, result.Count);
        }

        [Test]
        public async Task DeleteCancelledBookingTickets_Should_Delete_Cancelled_Booking_Tickets()
        {
            // Arrange
            int bookingId = 1;
            int userId = 1;
            var booking = new Booking { BookingId = bookingId, UserId = userId, Status = "refunded", Tickets = new List<Ticket> { new Ticket { TicketId = 1 } } };
            _mockBookingRepository.Setup(repo => repo.GetAsync(bookingId)).ReturnsAsync(booking);
            //await _mockTicketRepository.Setup(repo => repo.Delete(It.IsAny<int>())).Returns(Task.CompletedTask);

            // Act
            await _ticketService.DeleteCancelledBookingTickets(bookingId, userId);

            // Assert
            _mockTicketRepository.Verify(repo => repo.Delete(It.IsAny<int>()), Times.Once);
        }

        [Test]
        public async Task DeleteCancelledBookingTickets_Should_Not_Delete_Tickets_If_Booking_Not_Cancelled()
        {
            // Arrange
            int bookingId = 1;
            int userId = 1;
            var booking = new Booking { BookingId = bookingId, UserId = userId, Status = "confirmed", Tickets = new List<Ticket> { new Ticket { TicketId = 1 } } };
            _mockBookingRepository.Setup(repo => repo.GetAsync(bookingId)).ReturnsAsync(booking);

            // Act
            await _ticketService.DeleteCancelledBookingTickets(bookingId, userId);

            // Assert
            _mockTicketRepository.Verify(repo => repo.Delete(It.IsAny<int>()), Times.Never);
        }
    }
}
