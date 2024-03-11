using NUnit.Framework;
using Moq;
using FastX.Interfaces;
using FastX.Models;
using FastX.Models.DTOs;
using FastX.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FastX.Exceptions;

namespace FastX.Tests
{
    [TestFixture]
    public class BusOperatorServiceTest
    {
        private Mock<IRepository<int, Bus>> _mockBusRepository;
        private Mock<ILogger<BusService>> _mockLogger;
        private Mock<IPaymentService> _mockPaymentService;
        private Mock<IBookingService> _mockBookingService;
        private Mock<IRepository<int, User>> _mockUserRepository;
        private Mock<IBookingRepository<int, Booking>> _mockBookingRepository;
        private Mock<ITicketService> _mockTicketService;

        private IBusOperatorService _busOperatorService;

        [SetUp]
        public void Setup()
        {
            _mockBusRepository = new Mock<IRepository<int, Bus>>();
            _mockLogger = new Mock<ILogger<BusService>>();
            _mockPaymentService = new Mock<IPaymentService>();
            _mockBookingService = new Mock<IBookingService>();
            _mockUserRepository = new Mock<IRepository<int, User>>();
            _mockBookingRepository = new Mock<IBookingRepository<int, Booking>>();
            _mockTicketService = new Mock<ITicketService>();

            _busOperatorService = new BusOperatorService(
                _mockBusRepository.Object,
                _mockLogger.Object,
                _mockPaymentService.Object,
                _mockBookingService.Object,
                _mockUserRepository.Object,
                _mockBookingRepository.Object,
                _mockTicketService.Object
            );
        }

        [Test]
        public async Task GetAllBuses_Should_Return_List()
        {
            // Arrange
            var expectedBuses = new List<Bus>
            {
                new Bus { BusId = 1, BusName = "Bus1", BusType = "Type1", BusOperatorId = 1 },
                new Bus { BusId = 2, BusName = "Bus2", BusType = "Type2", BusOperatorId = 1 }
            };

            var busOperatorId = 1;

            _mockBusRepository.Setup(repo => repo.GetAsync())
                .ReturnsAsync(expectedBuses);

            // Act
            var result = await _busOperatorService.GetAllBuses(busOperatorId);

            // Assert
            Assert.AreEqual(expectedBuses.Count, result.Count);
            Assert.IsTrue(result.All(b => b.BusOperatorId == busOperatorId));
        }

        [Test]
        public async Task GetAllBuses_Should_Throw_BusOperatorNotFoundException()
        {
            // Arrange
            var busOperatorId = 1;

            _mockBusRepository.Setup(repo => repo.GetAsync())
                .ReturnsAsync(new List<Bus>());

            // Act & Assert
            Assert.ThrowsAsync<BusOperatorNotFoundException>(() => _busOperatorService.GetAllBuses(busOperatorId));
        }

        

        [Test]
        public async Task GetRefundDetailsForCancelledBookings_Should_Throw_NoSuchUserException()
        {
            // Arrange
            _mockUserRepository.Setup(repo => repo.GetAsync())
                .ReturnsAsync(new List<User>());

            // Act & Assert
            Assert.ThrowsAsync<NoSuchUserException>(() => _busOperatorService.GetRefundDetailsForCancelledBookings());
        }

        [Test]
        public async Task AcceptRefund_Should_Update_Booking_Status_And_Delete_Tickets()
        {
            // Arrange
            var bookingId = 1;
            var userId = 1;

            var mockBooking = new Booking { BookingId = bookingId };

            _mockBookingRepository.Setup(repo => repo.GetAsync(bookingId))
                .ReturnsAsync(mockBooking);

            // Act
            await _busOperatorService.AcceptRefund(bookingId, userId);

            // Assert
            Assert.AreEqual("refunded", mockBooking.Status);
            _mockTicketService.Verify(service => service.DeleteCancelledBookingTickets(bookingId, userId), Times.Once);
        }
    }
}
