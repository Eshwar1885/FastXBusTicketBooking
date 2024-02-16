using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FastX.Exceptions;
using FastX.Interfaces;
using FastX.Models;
using FastX.Models.DTOs;
using FastX.Services;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace FastX.Tests.Services
{
    [TestFixture]
    public class SeatServiceTest
    {
        private Mock<IRepository<int, Bus>> _mockBusRepository;
        private Mock<ISeatRepository<int, Seat>> _mockSeatRepository;
        private Mock<IRepository<int, Ticket>> _mockTicketRepository;
        private Mock<IRepository<int, Routee>> _mockRouteRepository;
        private Mock<IBookingRepository<int, Booking>> _mockBookingRepository;
        private Mock<ILogger<BusService>> _mockLogger;
        private SeatService _seatService;

        [SetUp]
        public void Setup()
        {
            _mockBusRepository = new Mock<IRepository<int, Bus>>();
            _mockSeatRepository = new Mock<ISeatRepository<int, Seat>>();
            _mockTicketRepository = new Mock<IRepository<int, Ticket>>();
            _mockRouteRepository = new Mock<IRepository<int, Routee>>();
            _mockBookingRepository = new Mock<IBookingRepository<int, Booking>>();
            _mockLogger = new Mock<ILogger<BusService>>();

            _seatService = new SeatService(
                _mockSeatRepository.Object,
                _mockBusRepository.Object,
                _mockTicketRepository.Object,
                _mockRouteRepository.Object,
                _mockBookingRepository.Object,
                _mockLogger.Object
            );
        }

        [Test]
        public async Task GetAvailableSeats_WithExistingBusAndAvailableSeats_ShouldReturnListOfSeatDTOForUser()
        {
            // Arrange
            int busId = 1;
            var mockBus = new Bus { BusId = busId, Seats = new List<Seat> { new Seat { SeatId = 1, IsAvailable = true, SeatPrice = 10.0f } } };
            _mockBusRepository.Setup(repo => repo.GetAsync(busId)).ReturnsAsync(mockBus);

            // Act
            var result = await _seatService.GetAvailableSeats(busId);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<List<SeatDTOForUser>>(result);
            Assert.AreEqual(mockBus.Seats.Count, result.Count);
        }

        [Test]
        public void GetAvailableSeats_WithNonExistingBus_ShouldThrowBusNotFoundException()
        {
            // Arrange
            int busId = 1;
            _mockBusRepository.Setup(repo => repo.GetAsync(busId)).ThrowsAsync(new BusNotFoundException());

            // Act & Assert
            Assert.ThrowsAsync<BusNotFoundException>(() => _seatService.GetAvailableSeats(busId));
        }

        //[Test]
        //public void GetAvailableSeats_WithNoAvailableSeats_ShouldThrowNoSeatsAvailableException()
        //{
        //    // Arrange
        //    int busId = 1;
        //    var mockBus = new Bus { BusId = busId, Seats = new List<Seat> { new Seat { SeatId = 1, IsAvailable = false, SeatPrice = 10.0f } } };
        //    _mockBusRepository.Setup(repo => repo.GetAsync(busId)).Returns(Task.FromResult(mockBus)); // Wrap mockBus in Task.FromResult

        //    // Act & Assert
        //    var exception = Assert.Throws<NoSeatsAvailableException>(() => _seatService.GetAvailableSeats(busId));
        //    Assert.NotNull(exception);
        //}





        // Add more test cases for other methods as needed
    }
}
