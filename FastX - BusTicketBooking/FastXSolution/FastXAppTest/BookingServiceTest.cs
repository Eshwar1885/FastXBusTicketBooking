using FastX.Exceptions;
using FastX.Interfaces;
using FastX.Models;
using FastX.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace FastX.Tests.Services
{
    [TestFixture]
    public class BookingServiceTest
    {
        private Mock<IRepository<int, Ticket>> _mockTicketRepository;
        private Mock<IBookingRepository<int, Booking>> _mockBookingRepository;
        private Mock<ISeatService> _mockSeatService;
        private Mock<ILogger<BookingService>> _mockLogger;
        private BookingService _bookingService;


        [SetUp]
        public void Setup()
        {
            _mockTicketRepository = new Mock<IRepository<int, Ticket>>();
            _mockBookingRepository = new Mock<IBookingRepository<int, Booking>>();
            _mockSeatService = new Mock<ISeatService>();
            _mockLogger = new Mock<ILogger<BookingService>>();
            _bookingService = new BookingService(
                _mockTicketRepository.Object,
                _mockBookingRepository.Object,
                _mockSeatService.Object,
                _mockBookingRepository.Object, // This parameter seems redundant, you might want to review it
                _mockLogger.Object
            );
        }


        [Test]
        public void MakeBooking_NoSeatsAvailable_ShouldThrowNoSeatsAvailableException()
        {
            // Arrange
            _mockSeatService.Setup(service => service.CheckWhetherSeatIsAvailableForBooking(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>()))
                .ReturnsAsync(false);

            // Act & Assert
            var ex = Assert.ThrowsAsync<NoSeatsAvailableException>(() => _bookingService.MakeBooking(1, 1, DateTime.Now, 1));
            Assert.NotNull(ex);
        }

        [Test]
        public void MakeBooking_SeatsAvailable_ShouldCreateBooking()
        {
            // Arrange
            _mockSeatService.Setup(service => service.CheckWhetherSeatIsAvailableForBooking(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>()))
                .ReturnsAsync(true);
            _mockBookingRepository.Setup(repo => repo.GetOngoingBookingAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>()))
                .ReturnsAsync((Booking)null); // Simulating no ongoing booking

            // Act
            _bookingService.MakeBooking(1, 1, DateTime.Now, 1);

            // Assert
            _mockBookingRepository.Verify(repo => repo.Add(It.IsAny<Booking>()), Times.Once);
        }

        // Add more test cases for other scenarios...




        // Add more test cases for other scenarios...
    }
}
