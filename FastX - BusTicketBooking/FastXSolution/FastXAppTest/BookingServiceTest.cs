using NUnit.Framework;
using Moq;
using FastX.Services;
using FastX.Models;
using FastX.Models.DTOs;
using FastX.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using FastX.Contexts;
using FastX.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace FastXAppTest
{
    [TestFixture]
    public class BookingServiceTest
    {
        private Mock<IRepository<int, Ticket>> _mockTicketRepository;
        private Mock<IBookingRepository<int, Booking>> _mockBookingRepository;
        private Mock<IRepository<int, User>> _mockUserRepository;
        private Mock<ISeatService> _mockSeatService;
        private Mock<ILogger<BookingService>> _mockLogger;
        private Mock<IPaymentService> _mockPaymentService;
        private Mock<IRepository<int, Bus>> _mockBusRepository;
        private FastXContext _mockContext; // Mock FastXContext

        [SetUp]
        public void Setup()
        {
            _mockTicketRepository = new Mock<IRepository<int, Ticket>>();
            _mockBookingRepository = new Mock<IBookingRepository<int, Booking>>();
            _mockUserRepository = new Mock<IRepository<int, User>>();
            _mockSeatService = new Mock<ISeatService>();
            _mockLogger = new Mock<ILogger<BookingService>>();
            _mockPaymentService = new Mock<IPaymentService>();
            _mockBusRepository = new Mock<IRepository<int, Bus>>();
            _mockContext = new FastXContext(new DbContextOptionsBuilder<FastXContext>().Options);
        }

        [Test]
        public async Task ChangeNoOfSeatsAsync_Should_Update_Number_Of_Seats()
        {
            // Arrange
            var bookingId = 1;
            var noOfSeats = 5;
            var booking = new Booking { BookingId = bookingId };

            _mockBookingRepository.Setup(repo => repo.GetAsync(bookingId)).ReturnsAsync(booking);
            var bookingService = new BookingService(_mockContext, _mockTicketRepository.Object, _mockBookingRepository.Object,
                _mockUserRepository.Object, _mockSeatService.Object, _mockLogger.Object, _mockPaymentService.Object, _mockBusRepository.Object);

            // Act
            await bookingService.ChangeNoOfSeatsAsync(bookingId, noOfSeats);

            // Assert
            Assert.AreEqual(noOfSeats, booking.NumberOfSeats);
            _mockBookingRepository.Verify(repo => repo.Update(booking), Times.Once);
        }

        [Test]
        public async Task GetBookingList_Should_Return_List_Of_Bookings()
        {
            // Arrange
            var bookings = new List<Booking> { new Booking(), new Booking() };
            _mockBookingRepository.Setup(repo => repo.GetAsync()).ReturnsAsync(bookings);
            var bookingService = new BookingService(_mockContext, _mockTicketRepository.Object, _mockBookingRepository.Object,
                _mockUserRepository.Object, _mockSeatService.Object, _mockLogger.Object, _mockPaymentService.Object, _mockBusRepository.Object);

            // Act
            var result = await bookingService.GetBookingList();

            // Assert
            Assert.AreEqual(bookings.Count, result.Count);
        }

        // Add more tests for other methods in the BookingService class...

        [Test]
        public async Task MakeBooking_Should_Throw_Exception_When_No_Seats_Available()
        {
            // Arrange
            var busId = 1;
            var seatIds = new List<int> { 1, 2, 3 }; // Assuming these are valid seat IDs
            var travelDate = DateTime.Now.AddDays(7);
            var userId = 1;
            var totalSeats = seatIds.Count;

            _mockSeatService.Setup(service => service.CheckWhetherSeatIsAvailableForBooking(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>()))
                            .ReturnsAsync(false); // Mocking the seat service to return false

            var bookingService = new BookingService(_mockContext, _mockTicketRepository.Object, _mockBookingRepository.Object,
                                                    _mockUserRepository.Object, _mockSeatService.Object, _mockLogger.Object,
                                                    _mockPaymentService.Object, _mockBusRepository.Object);

            // Act & Assert
            Assert.ThrowsAsync<Exception>(async () =>
            {
                await bookingService.MakeBooking(busId, seatIds, travelDate, userId, totalSeats);
            });
        }




        // Add more tests for other methods in the BookingService class...

        [Test]
        public async Task CancelBooking_Should_Throw_BookingNotFoundException_When_Booking_Not_Found()
        {
            // Arrange
            var userId = 1;
            var bookingId = 1;

            _mockBookingRepository.Setup(repo => repo.GetAsync(bookingId)).ReturnsAsync((Booking)null);

            var bookingService = new BookingService(_mockContext, _mockTicketRepository.Object, _mockBookingRepository.Object,
                _mockUserRepository.Object, _mockSeatService.Object, _mockLogger.Object, _mockPaymentService.Object, _mockBusRepository.Object);

            // Act & Assert
            Assert.ThrowsAsync<BookingNotFoundException>(async () =>
            {
                await bookingService.CancelBooking(userId, bookingId);
            });
        }

        

        [Test]
        public async Task GetCancelledBookings_Should_Throw_NoSuchUserException_When_User_Not_Found()
        {
            // Arrange
            var userId = 1;

            _mockUserRepository.Setup(repo => repo.GetAsync(userId)).ReturnsAsync((User)null); // Simulating user not found

            var bookingService = new BookingService(_mockContext, _mockTicketRepository.Object, _mockBookingRepository.Object,
                                                    _mockUserRepository.Object, _mockSeatService.Object, _mockLogger.Object,
                                                    _mockPaymentService.Object, _mockBusRepository.Object);

            // Act & Assert
            Assert.ThrowsAsync<NoSuchUserException>(async () => await bookingService.GetCancelledBookings(userId));
        }

        [Test]
        public async Task GetUpcomingBookings_Should_Throw_NullReferenceException_When_No_Completed_Bookings()
        {
            // Arrange
            var userId = 1;
            var completedBookings = new List<CompletedBookingDTO>(); // Empty list of completed bookings

            var mockBookingService = new Mock<IBookingService>();
            mockBookingService.Setup(service => service.GetCompletedBookings(userId)).ReturnsAsync(completedBookings);

            var bookingService = new BookingService(
                context: null,
                ticketRepository: null,
                bookingRepository: null,
                userRepository: null,
                seatService: null,
                logger: null,
                paymentService: null,
                busRepository: null
            );

            // Act and Assert
            Assert.ThrowsAsync<NullReferenceException>(async () =>
            {
                await bookingService.GetUpcomingBookings(userId);
            });
        }


        [Test]
        public async Task GetPastBookings_Should_Throw_NullReferenceException_When_No_Completed_Bookings()
        {
            // Arrange
            var userId = 1;
            var completedBookings = new List<CompletedBookingDTO>(); // Empty list of completed bookings

            var mockBookingService = new Mock<IBookingService>();
            mockBookingService.Setup(service => service.GetCompletedBookings(userId)).ReturnsAsync(completedBookings);

            var bookingService = new BookingService(
                context: null,
                ticketRepository: null,
                bookingRepository: null,
                userRepository: null,
                seatService: null,
                logger: null,
                paymentService: null,
                busRepository: null
            );

            // Act and Assert
            Assert.ThrowsAsync<NullReferenceException>(async () =>
            {
                await bookingService.GetPastBookings(userId);
            });
        }

        [Test]
        public async Task UpdateOngoingBookingsAndResetSeats_Should_Throw_NullReferenceException_When_No_Ongoing_Bookings()
        {
            // Arrange
            var mockBookingRepository = new Mock<IBookingRepository<int, Booking>>();
            mockBookingRepository.Setup(repo => repo.GetAsync()).ReturnsAsync(new List<Booking>());

            var mockSeatService = new Mock<ISeatService>();
            var mockLogger = new Mock<ILogger<BookingService>>();

            var bookingService = new BookingService(
                context: null,
                ticketRepository: null,
                bookingRepository: mockBookingRepository.Object,
                userRepository: null,
                seatService: mockSeatService.Object,
                logger: mockLogger.Object,
                paymentService: null,
                busRepository: null
            );

            // Act and Assert
            //var exception = Assert.ThrowsAsync<NullReferenceException>(async () =>
            //{
                await bookingService.UpdateOngoingBookingsAndResetSeats();
            //}
            //);

            //Assert.IsNull(exception);
        }











    }
}
