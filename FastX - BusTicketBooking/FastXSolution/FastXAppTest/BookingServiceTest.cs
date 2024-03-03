////using FastX.Exceptions;
////using FastX.Interfaces;
////using FastX.Models;
////using FastX.Services;
////using Microsoft.Extensions.Logging;
////using Microsoft.Extensions.Logging;
////using Moq;
////using NUnit.Framework;
////using System;
////using System.Threading.Tasks;

////namespace FastX.Tests.Services
////{
////    [TestFixture]
////    public class BookingServiceTest
////    {
////        private Mock<IRepository<int, Ticket>> _mockTicketRepository;
////        private Mock<IBookingRepository<int, Booking>> _mockBookingRepository;
////        private Mock<ISeatService> _mockSeatService;
////        private Mock<ILogger<BookingService>> _mockLogger;
////        private BookingService _bookingService;


////        [SetUp]
////        public void Setup()
////        {
////            _mockTicketRepository = new Mock<IRepository<int, Ticket>>();
////            _mockBookingRepository = new Mock<IBookingRepository<int, Booking>>();
////            _mockSeatService = new Mock<ISeatService>();
////            _mockLogger = new Mock<ILogger<BookingService>>();
////            _bookingService = new BookingService(
////                _mockTicketRepository.Object,
////                _mockBookingRepository.Object,
////                _mockSeatService.Object,
////                _mockBookingRepository.Object, // This parameter seems redundant, you might want to review it
////                _mockLogger.Object
////            );
////        }


////        [Test]
////        public void MakeBooking_NoSeatsAvailable_ShouldThrowNoSeatsAvailableException()
////        {
////            // Arrange
////            _mockSeatService.Setup(service => service.CheckWhetherSeatIsAvailableForBooking(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>()))
////                .ReturnsAsync(false);

////            // Act & Assert
////            var ex = Assert.ThrowsAsync<NoSeatsAvailableException>(() => _bookingService.MakeBooking(1, 1, DateTime.Now, 1));
////            Assert.NotNull(ex);
////        }

////        [Test]
////        public void MakeBooking_SeatsAvailable_ShouldCreateBooking()
////        {
////            // Arrange
////            _mockSeatService.Setup(service => service.CheckWhetherSeatIsAvailableForBooking(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>()))
////                .ReturnsAsync(true);
////            _mockBookingRepository.Setup(repo => repo.GetOngoingBookingAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>()))
////                .ReturnsAsync((Booking)null); // Simulating no ongoing booking

////            // Act
////            _bookingService.MakeBooking(1, 1, DateTime.Now, 1);

////            // Assert
////            _mockBookingRepository.Verify(repo => repo.Add(It.IsAny<Booking>()), Times.Once);
////        }

////        // Add more test cases for other scenarios...




////        // Add more test cases for other scenarios...
////    }
////}


//using FastX.Contexts;
//using FastX.Exceptions;
//using FastX.Interfaces;
//using FastX.Models;
//using FastX.Repositories;
//using FastX.Services;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Logging;
//using Moq;
//using Moq.Protected;
//using NUnit.Framework;
//using System;
//using System.Threading.Tasks;

//namespace FastX.Tests.Services
//{
//    [TestFixture]
//    public class BookingServiceTest
//    {
//        private Mock<IRepository<int, Ticket>> _mockTicketRepository;
//        private Mock<IBookingRepository<int, Booking>> _mockBookingRepository;
//        private Mock<IRepository<int, User>> _mockUserRepository;
//        private Mock<ISeatService> _mockSeatService;
//        private Mock<ILogger<BookingService>> _mockLogger;
//        private Mock<IPaymentService> _mockPaymentService;
//        private FastXContext _context;
//        private BookingService _bookingService;


//        private Mock<IBookingRepository<int, Booking>> _bookingRepositoryMock;
//        private Mock<ILogger<BookingService>> _loggerMock;

//        [SetUp]
//        public void Setup()
//        {
//            _mockTicketRepository = new Mock<IRepository<int, Ticket>>();
//            _mockBookingRepository = new Mock<IBookingRepository<int, Booking>>();
//            _mockUserRepository = new Mock<IRepository<int, User>>();
//            _mockSeatService = new Mock<ISeatService>();
//            _mockLogger = new Mock<ILogger<BookingService>>();
//            _mockPaymentService = new Mock<IPaymentService>();
//            _bookingRepositoryMock = new Mock<IBookingRepository<int, Booking>>();
//            _loggerMock = new Mock<ILogger<BookingService>>();
//            _context = new FastXContext(new DbContextOptionsBuilder<FastXContext>()
//                .UseInMemoryDatabase(databaseName: "TestDatabase")
//                .Options);
//            _bookingService = new BookingService(
//                _context,
//                _mockTicketRepository.Object,
//                _mockBookingRepository.Object,
//                _mockUserRepository.Object,
//                _mockSeatService.Object,
//                _mockLogger.Object,
//                _mockPaymentService.Object

//            );
//        }

//        [Test]
//        public async Task ChangeNoOfSeatsAsync_ValidBookingId_ShouldUpdateNumberOfSeats()
//        {
//            // Arrange
//            int bookingId = 1;
//            int newNumberOfSeats = 5;
//            var mockBooking = new Booking { BookingId = bookingId, NumberOfSeats = 3 }; // Existing booking with 3 seats
//            _mockBookingRepository.Setup(repo => repo.GetAsync(bookingId)).ReturnsAsync(mockBooking);

//            // Act
//            await _bookingService.ChangeNoOfSeatsAsync(bookingId, newNumberOfSeats);

//            // Assert
//            Assert.AreEqual(newNumberOfSeats, mockBooking.NumberOfSeats); // Verify if the number of seats is updated correctly
//            _mockBookingRepository.Verify(repo => repo.Update(mockBooking), Times.Once); // Verify if Update method is called once
//        }

//        [Test]
//        public async Task ChangeNoOfSeatsAsync_InvalidBookingId_ShouldNotUpdateNumberOfSeats()
//        {
//            // Arrange
//            int invalidBookingId = 100;
//            int newNumberOfSeats = 5;
//            _mockBookingRepository.Setup(repo => repo.GetAsync(invalidBookingId)).ReturnsAsync((Booking)null); // Simulating no booking with the provided ID

//            // Act
//            await _bookingService.ChangeNoOfSeatsAsync(invalidBookingId, newNumberOfSeats);

//            // Assert
//            _mockBookingRepository.Verify(repo => repo.Update(It.IsAny<Booking>()), Times.Never); // Verify if Update method is never called
//        }

//        [Test]
//        public async Task GetBookingList_NoBookingsFound_ShouldThrowBookingNotFoundException()
//        {
//            // Arrange
//            _mockBookingRepository.Setup(repo => repo.GetAsync()).ReturnsAsync((List<Booking>)null);

//            // Act & Assert
//            var ex = Assert.ThrowsAsync<BookingNotFoundException>(() => _bookingService.GetBookingList());
//            Assert.NotNull(ex);
//        }

//        [Test]
//        public async Task GetBookingList_BookingsFound_ShouldReturnListOfBookings()
//        {
//            // Arrange
//            var expectedBookings = new List<Booking> { new Booking(), new Booking() };
//            _mockBookingRepository.Setup(repo => repo.GetAsync()).ReturnsAsync(expectedBookings);

//            // Act
//            var actualBookings = await _bookingService.GetBookingList();

//            // Assert
//            Assert.AreEqual(expectedBookings, actualBookings);
//        }
//        [Test]
//        public async Task CreateNewBooking_ValidInputs_ShouldCreateBookingAndTickets()
//        {
//            // Arrange
//            var busId = 1;
//            var userId = 1;
//            var travelDate = DateTime.Now;
//            var numberOfSeats = 2;
//            var seatIds = new List<int> { 1, 2 };

//            var expectedBookingId = 1;
//            var addedBooking = new Booking { BookingId = expectedBookingId };
//            _mockBookingRepository.Setup(repo => repo.Add(It.IsAny<Booking>())).ReturnsAsync(addedBooking);

//            var expectedSeatPrice = 50; // Sample seat price
//            _mockSeatService.Setup(service => service.GetSeatPriceAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(expectedSeatPrice);

//            // Act
//            var result = await _bookingService.CreateNewBooking(busId, userId, travelDate, numberOfSeats, seatIds);

//            // Assert
//            Assert.NotNull(result);
//            Assert.AreEqual(expectedBookingId, result.BookingId);
//            _mockBookingRepository.Verify(repo => repo.Add(It.IsAny<Booking>()), Times.Once);
//            _mockTicketRepository.Verify(repo => repo.Add(It.IsAny<Ticket>()), Times.Exactly(seatIds.Count));
//            _mockSeatService.Verify(service => service.ChangeSeatAvailablityAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Exactly(seatIds.Count));
//        }

//        [Test]
//        public async Task CreateNewBooking_InvalidSeatPrice_ShouldThrowException()
//        {
//            // Arrange
//            var busId = 1;
//            var userId = 1;
//            var travelDate = DateTime.Now;
//            var numberOfSeats = 2;
//            var seatIds = new List<int> { 1, 2 };

//            _mockBookingRepository.Setup(repo => repo.Add(It.IsAny<Booking>())).ReturnsAsync(new Booking());
//            _mockSeatService.Setup(service => service.GetSeatPriceAsync(It.IsAny<int>(), It.IsAny<int>())).ThrowsAsync(new Exception("Failed to get seat price"));

//            // Act & Assert
//            var ex = Assert.ThrowsAsync<Exception>(() => _bookingService.CreateNewBooking(busId, userId, travelDate, numberOfSeats, seatIds));
//            Assert.NotNull(ex);
//        }

//        //[Test]
//        //public async Task MakeBooking_AllSeatsAvailable_ShouldCreateBooking()
//        //{
//        //    // Arrange
//        //    int busId = 1;
//        //    List<int> seatIds = new List<int> { 1, 2, 3 }; // Assuming seat IDs
//        //    DateTime travelDate = DateTime.Now.AddDays(1); // Assuming travel date is tomorrow
//        //    int userId = 123; // Assuming user ID
//        //    int totalSeats = 3; // Assuming total number of seats to book

//        //    // Mock SeatService to return true for all seats (available)
//        //    _mockSeatService.Setup(service => service.CheckWhetherSeatIsAvailableForBooking(busId, It.IsAny<int>(), travelDate))
//        //        .ReturnsAsync(true);

//        //    // Mock CreateNewBooking method to return a Booking object
//        //    var mockBooking = new Booking { BookingId = 1 }; // Assuming booking is successfully created
//        //    _mockBookingRepository.Setup(repo => repo.Add(It.IsAny<Booking>())).ReturnsAsync(mockBooking);

//        //    // Act
//        //    var result = await _bookingService.MakeBooking(busId, seatIds, travelDate, userId, totalSeats);

//        //    // Assert
//        //    Assert.NotNull(result);
//        //    Assert.AreEqual(mockBooking, result);
//        //}

//        //[Test]
//        //public void MakeBooking_NoSeatsAvailable_ShouldThrowNoSeatsAvailableException()
//        //{
//        //    // Arrange
//        //    int busId = 1;
//        //    List<int> seatIds = new List<int> { 1, 2, 3 }; // Assuming seat IDs
//        //    DateTime travelDate = DateTime.Now.AddDays(1); // Assuming travel date is tomorrow
//        //    int userId = 123; // Assuming user ID
//        //    int totalSeats = 3; // Assuming total number of seats to book

//        //    // Mock SeatService to return false for all seats (not available)
//        //    _mockSeatService.Setup(service => service.CheckWhetherSeatIsAvailableForBooking(busId, It.IsAny<int>(), travelDate))
//        //        .ReturnsAsync(false);

//        //    // Act & Assert
//        //    var ex = Assert.ThrowsAsync<NoSeatsAvailableException>(() => _bookingService.MakeBooking(busId, seatIds, travelDate, userId, totalSeats));
//        //    Assert.NotNull(ex);
//        //}


//        [Test]
//        public async Task GetBookingInfo_Returns_Bus_When_Booking_Exists_And_BookedForWhichDate_Matches()
//        {
//            // Arrange
//            int bookingId = 1;
//            DateTime bookedForWhichDate = DateTime.Today;
//            var booking = new Booking
//            {
//                BookingId = bookingId,
//                BookedForWhichDate = bookedForWhichDate,
//                Bus = new Bus { BusName = "Test Bus" }
//            };
//            _bookingRepositoryMock.Setup(repo => repo.GetAsync(bookingId)).ReturnsAsync(booking);

//            // Act
//            var result = await _bookingService.GetBookingInfo(bookingId, bookedForWhichDate);

//            // Assert
//            Assert.IsNotNull(result);
//            Assert.AreEqual("Test Bus", result.BusName);
//        }

//        [Test]
//        public async Task GetBookingInfo_Returns_Null_When_Booking_Not_Found()
//        {
//            // Arrange
//            int bookingId = 1;
//            DateTime bookedForWhichDate = DateTime.Today;
//            _bookingRepositoryMock.Setup(repo => repo.GetAsync(bookingId)).ReturnsAsync((Booking)null);

//            // Act
//            var result = await _bookingService.GetBookingInfo(bookingId, bookedForWhichDate);

//            // Assert
//            Assert.IsNull(result);
//        }

//        [Test]
//        public async Task GetBookingInfo_Returns_Null_When_BookedForWhichDate_Mismatch()
//        {
//            // Arrange
//            int bookingId = 1;
//            DateTime bookedForWhichDate = DateTime.Today;
//            var booking = new Booking
//            {
//                BookingId = bookingId,
//                BookedForWhichDate = bookedForWhichDate.AddDays(1), // Different date
//                Bus = new Bus { BusName = "Test Bus" }
//            };
//            _bookingRepositoryMock.Setup(repo => repo.GetAsync(bookingId)).ReturnsAsync(booking);

//            // Act
//            var result = await _bookingService.GetBookingInfo(bookingId, bookedForWhichDate);

//            // Assert
//            Assert.IsNull(result);
//        }

//        [Test]
//        public async Task GetCompletedBookings_Returns_Completed_Bookings_For_Valid_User()
//        {
//            // Arrange
//            int userId = 1;
//            var user = new User
//            {
//                UserId = userId,
//                Bookings = new List<Booking>
//            {
//                new Booking
//                {
//                    BookingId = 1,
//                    BookedForWhichDate = DateTime.Today,
//                    Status = "Complete",
//                    NumberOfSeats = 2,
//                    Tickets = new List<Ticket>
//                    {
//                        new Ticket { SeatId = 1 },
//                        new Ticket { SeatId = 2 }
//                    }
//                },
//                new Booking
//                {
//                    BookingId = 2,
//                    BookedForWhichDate = DateTime.Today.AddDays(-1), // Past booking, should not be included
//                    Status = "Complete",
//                    NumberOfSeats = 1,
//                    Tickets = new List<Ticket>
//                    {
//                        new Ticket { SeatId = 3 }
//                    }
//                },
//                new Booking
//                {
//                    BookingId = 3,
//                    BookedForWhichDate = DateTime.Today.AddDays(1), // Future booking, should not be included
//                    Status = "Pending", // Not complete, should not be included
//                    NumberOfSeats = 3,
//                    Tickets = new List<Ticket>
//                    {
//                        new Ticket { SeatId = 4 },
//                        new Ticket { SeatId = 5 },
//                        new Ticket { SeatId = 6 }
//                    }
//                }
//            }
//            };
//            _mockUserRepository.Setup(repo => repo.GetAsync(userId)).ReturnsAsync(user);

//            // Mock the GetBookingInfo method to return a valid Bus object
//            _bookingService.GetBookingInfo = async (bookingId, _) => new Bus
//            {
//                BusName = "Test Bus",
//                BusType = "Test Type"
//            };

//            // Act
//            var completedBookings = await _bookingService.GetCompletedBookings(userId);

//            // Assert
//            Assert.IsNotNull(completedBookings);
//            //Assert.AreEqual(1, completedBookings.Count);
//            //var completedBooking = completedBookings.First();
//            //Assert.AreEqual(1, completedBooking.BookingId);
//            //Assert.AreEqual("Test Bus", completedBooking.BusName);
//            //Assert.AreEqual("Test Type", completedBooking.BusType);
//            //Assert.AreEqual(2, completedBooking.NumberOfSeats);
//            //Assert.AreEqual(DateTime.Today, completedBooking.BookedForWhichDate);
//            //Assert.AreEqual("", completedBooking.Origin);
//            //Assert.AreEqual("", completedBooking.Destination);
//            //Assert.AreEqual("1,2", completedBooking.SeatNumbers);
//        }
//        [Test]
//        public async Task UpdateOngoingBookingsAndResetSeats_Deletes_Ongoing_Bookings_And_Resets_Seats()
//        {
//            // Arrange
//            var now = DateTime.Now;
//            var tenMinutesAgo = now.AddMinutes(-10);

//            // Mock ongoing bookings
//            var ongoingBookings = new List<Booking>
//        {
//        new Booking { BookingDate = tenMinutesAgo, Status = "ongoing", Tickets = new List<Ticket> { new Ticket { SeatId = 1, BusId = 1 } } },
//        new Booking { BookingDate = tenMinutesAgo, Status = "ongoing", Tickets = new List<Ticket> { new Ticket { SeatId = 2, BusId = 1 }, new Ticket { SeatId = 3, BusId = 1 } } }
//        };

//            _mockBookingRepository.Setup(repo => repo.GetAsync()).ReturnsAsync(ongoingBookings);

//            // Act
//            await _bookingService.UpdateOngoingBookingsAndResetSeats();

//            // Assert
//            // Verify that ChangeSeatAvailablityAsync is called for each ticket
//            _mockSeatService.Verify(service => service.ChangeSeatAvailablity(It.IsAny<int>(), It.IsAny<int>()), Times.Exactly(3));

//            // Verify that DeleteAsync is called for each ongoing booking
//            _mockBookingRepository.Verify(repo => repo.Delete(It.IsAny<int>()), Times.Exactly(2));

//        }


//    }
//}

