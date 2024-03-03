//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using FastX.Exceptions;
//using FastX.Interfaces;
//using FastX.Models;
//using FastX.Models.DTOs;
//using FastX.Services;
//using Microsoft.Extensions.Logging;
//using Moq;
//using NUnit.Framework;

//namespace FastX.Tests.Services
//{
//    [TestFixture]
//    public class SeatServiceTest
//    {
//        private Mock<IRepository<int, Bus>> _mockBusRepository;
//        private Mock<ISeatRepository<int, Seat>> _mockSeatRepository;
//        private Mock<IRepository<int, Ticket>> _mockTicketRepository;
//        private Mock<IRepository<int, Routee>> _mockRouteRepository;
//        private Mock<IBookingRepository<int, Booking>> _mockBookingRepository;
//        private Mock<ILogger<BusService>> _mockLogger;
//        private SeatService _seatService;

//        [SetUp]
//        public void Setup()
//        {
//            _mockBusRepository = new Mock<IRepository<int, Bus>>();
//            _mockSeatRepository = new Mock<ISeatRepository<int, Seat>>();
//            _mockTicketRepository = new Mock<IRepository<int, Ticket>>();
//            _mockRouteRepository = new Mock<IRepository<int, Routee>>();
//            _mockBookingRepository = new Mock<IBookingRepository<int, Booking>>();
//            _mockLogger = new Mock<ILogger<BusService>>();

//            _seatService = new SeatService(
//                _mockSeatRepository.Object,
//                _mockBusRepository.Object,
//                _mockTicketRepository.Object,
//                _mockRouteRepository.Object,
//                _mockBookingRepository.Object,
//                _mockLogger.Object
//            );
//        }

//        [Test]
//        public async Task GetAvailableSeats_WithExistingBusAndAvailableSeats_ShouldReturnListOfSeatDTOForUser()
//        {
//            // Arrange
//            int busId = 1;
//            var mockBus = new Bus { BusId = busId, Seats = new List<Seat> { new Seat { SeatId = 1, IsAvailable = true, SeatPrice = 10.0f } } };
//            _mockBusRepository.Setup(repo => repo.GetAsync(busId)).ReturnsAsync(mockBus);

//            // Act
//            var result = await _seatService.GetAvailableSeats(busId);

//            // Assert
//            Assert.IsNotNull(result);
//            Assert.IsInstanceOf<List<SeatDTOForUser>>(result);
//            Assert.AreEqual(mockBus.Seats.Count, result.Count);
//        }

//        [Test]
//        public void GetAvailableSeats_WithNonExistingBus_ShouldThrowBusNotFoundException()
//        {
//            // Arrange
//            int busId = 1;
//            _mockBusRepository.Setup(repo => repo.GetAsync(busId)).ThrowsAsync(new BusNotFoundException());

//            // Act & Assert
//            Assert.ThrowsAsync<BusNotFoundException>(() => _seatService.GetAvailableSeats(busId));
//        }






//        // Add more test cases for other methods as needed
//    }
//}




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
    public class SeatServiceTest
    {
        private Mock<IRepository<int, Bus>> _mockBusRepository;
        private Mock<IBookingRepository<int, Booking>> _mockBookingRepository;
        private Mock<IRepository<int, Routee>> _mockRouteRepository;
        private Mock<IRepository<int, Ticket>> _mockTicketRepository;
        private Mock<ISeatRepository<int, Seat>> _mockSeatRepository;
        private Mock<ILogger<BusService>> _mockLogger;
        private SeatService _seatService;

        [SetUp]
        public void Setup()
        {
            _mockBusRepository = new Mock<IRepository<int, Bus>>();
            _mockBookingRepository = new Mock<IBookingRepository<int, Booking>>();
            _mockRouteRepository = new Mock<IRepository<int, Routee>>();
            _mockTicketRepository = new Mock<IRepository<int, Ticket>>();
            _mockSeatRepository = new Mock<ISeatRepository<int, Seat>>();
            _mockLogger = new Mock<ILogger<BusService>>();
            _seatService = new SeatService(
                _mockSeatRepository.Object,
                _mockBusRepository.Object,
                _mockTicketRepository.Object,
                _mockRouteRepository.Object,
                _mockBookingRepository.Object,
                _mockLogger.Object);
        }

        [Test]
        public async Task ChangeSeatAvailability_Should_Set_Seat_Availability_To_True()
        {
            // Arrange
            int seatId = 1;
            int busId = 1;
            var seat = new Seat { SeatId = seatId, IsAvailable = false };
            _mockSeatRepository.Setup(repo => repo.GetAsync(busId, seatId)).ReturnsAsync(seat);
            _mockSeatRepository.Setup(repo => repo.Update(seat)).ReturnsAsync(seat);

            // Act
            await _seatService.ChangeSeatAvailablity(seatId, busId);

            // Assert
            Assert.IsTrue(seat.IsAvailable);
        }


        [Test]
        public async Task ChangeSeatAvailabilityAsync_Should_Set_Seat_Availability_To_False()
        {
            // Arrange
            int seatId = 1;
            int busId = 1;
            var seat = new Seat { SeatId = seatId, IsAvailable = true };
            _mockSeatRepository.Setup(repo => repo.GetAsync(busId, seatId)).ReturnsAsync(seat);
            _mockSeatRepository.Setup(repo => repo.Update(seat)).ReturnsAsync(seat);

            // Act
            await _seatService.ChangeSeatAvailablityAsync(seatId, busId);

            // Assert
            Assert.IsFalse(seat.IsAvailable);
        }

        //[Ignore("seat")]
        //[Test]
        //public async Task ChangeSeatAvailabilityForCancelledBookings_Should_Change_Seat_Availability()
        //{
        //    // Arrange
        //    var cancelledBooking = new Booking { Status = "cancelled" };
        //    var ticket = new Ticket { SeatId = 1, BusId = 1, Booking = cancelledBooking };
        //    var cancelledBookings = new List<Booking> { cancelledBooking };
        //    _mockBookingRepository.Setup(repo => repo.GetAsync()).ReturnsAsync(cancelledBookings);
        //    _mockTicketRepository.Setup(repo => repo.GetAsync()).ReturnsAsync(new List<Ticket> { ticket });

        //    // Act
        //    await _seatService.ChangeSeatAvailabilityForCancelledBookings();

        //    // Assert
        //    _mockSeatRepository.Verify(repo => repo.Update(It.IsAny<Seat>()), Times.Once);
        //}

        [Test]
        public async Task GetAvailableSeats_Should_Return_Available_Seats()
        {
            // Arrange
            int busId = 1;
            var seats = new List<Seat>
            {
                new Seat { SeatId = 1, IsAvailable = true },
                new Seat { SeatId = 2, IsAvailable = false },
                new Seat { SeatId = 3, IsAvailable = true }
            };
            _mockBusRepository.Setup(repo => repo.GetAsync(busId)).ReturnsAsync(new Bus { Seats = seats });

            // Act
            var result = await _seatService.GetAvailableSeats(busId);

            // Assert
            Assert.AreEqual(2, result.Count);
        }

        [Test]
        public async Task CheckWhetherSeatIsAvailableForBooking_Should_Return_True_If_Seat_Available()
        {
            // Arrange
            int busId = 1;
            int seatId = 1;
            DateTime date = DateTime.Now;
            var seat = new Seat { SeatId = seatId, IsAvailable = true };
            _mockBusRepository.Setup(repo => repo.GetAsync(busId)).ReturnsAsync(new Bus());
            _mockSeatRepository.Setup(repo => repo.GetAsync(busId, seatId)).ReturnsAsync(seat);
            _mockTicketRepository.Setup(repo => repo.GetAsync()).ReturnsAsync(new List<Ticket>());

            // Act
            var result = await _seatService.CheckWhetherSeatIsAvailableForBooking(busId, seatId, date);

            // Assert
            Assert.IsTrue(result);
        }



        [Test]
        public async Task CheckWhetherSeatIsAvailableForBooking_Should_Return_False_If_Seat_Already_Booked()
        {
            // Arrange
            int busId = 1;
            int seatId = 1;
            DateTime date = DateTime.Now;
            var seat = new Seat { SeatId = seatId, IsAvailable = true };
            var ticket = new Ticket { SeatId = seatId, BusId = busId, Booking = new Booking { BusId = busId, BookedForWhichDate = date, Status = "complete" } };
            _mockBusRepository.Setup(repo => repo.GetAsync(busId)).ReturnsAsync(new Bus());
            _mockSeatRepository.Setup(repo => repo.GetAsync(busId, seatId)).ReturnsAsync(seat);
            _mockTicketRepository.Setup(repo => repo.GetAsync()).ReturnsAsync(new List<Ticket> { ticket });

            // Act
            var result = await _seatService.CheckWhetherSeatIsAvailableForBooking(busId, seatId, date);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task GetSeatPriceAsync_Should_Return_Seat_Price()
        {
            // Arrange
            int seatId = 1;
            int busId = 1;
            var seat = new Seat { SeatId = seatId, SeatPrice = 50.0f };
            _mockSeatRepository.Setup(repo => repo.GetAsync(busId, seatId)).ReturnsAsync(seat);

            // Act
            var result = await _seatService.GetSeatPriceAsync(seatId, busId);

            // Assert
            Assert.AreEqual(50.0f, result);
        }

        [Test]
        public async Task GetSeatPriceAsync_Should_Throw_NoSeatsAvailableException_If_Seat_Not_Found()
        {
            // Arrange
            int seatId = 1;
            int busId = 1;
            Seat seat = null;
            _mockSeatRepository.Setup(repo => repo.GetAsync(busId, seatId)).ReturnsAsync(seat);

            // Act & Assert
            Assert.ThrowsAsync<NoSeatsAvailableException>(() => _seatService.GetSeatPriceAsync(seatId, busId));
        }

    }
}
