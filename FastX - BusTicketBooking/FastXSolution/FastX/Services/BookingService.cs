using FastX.Contexts;
using FastX.Exceptions;
using FastX.Interfaces;
using FastX.Models;
using FastX.Models.DTOs;
using FastX.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Numerics;

namespace FastX.Services
{
    public class BookingService : IBookingService
    {
        private readonly IRepository<int, Ticket> _ticketRepository;
        private readonly IBookingRepository<int, Booking> _bookingRepository;
        // private readonly IBookingRepository<int, Booking> _booking2Repository;
        private readonly IRepository<int, User> _userRepository;
        private readonly ISeatService _seatService;
        private readonly ILogger<BookingService> _logger;
        private readonly FastXContext _context;
        //private readonly IRepository<int, BusRoute> _busRouteRepository;
        private readonly IPaymentService _paymentService;





        public BookingService(
            FastXContext context,
            IRepository<int, Ticket> ticketRepository,
            IBookingRepository<int, Booking> bookingRepository,
            IRepository<int, User> userRepository,
           ISeatService seatService,
           //IBookingRepository<int, Booking> booking2Repository,
        ILogger<BookingService> logger,
        //IRepository<int, BusRoute> busRouteRepository
        IPaymentService paymentService

            )
        {
            _ticketRepository = ticketRepository;
            _bookingRepository = bookingRepository;
            //_booking2Repository = booking2Repository;
            _userRepository = userRepository;
            _seatService = seatService;
            _logger = logger;
            _context = context;
            //_busRouteRepository = busRouteRepository;
            _paymentService = paymentService;
        }
        public async Task ChangeNoOfSeatsAsync(int id, int noOfSeats)
        {
            var booking = await _bookingRepository.GetAsync(id);
            if (booking != null)
            {
                booking.NumberOfSeats = noOfSeats;
                await _bookingRepository.Update(booking);

            }

        }

        public async Task<List<Booking>> GetBookingList()
        {
            var booking = await _bookingRepository.GetAsync();
            if (booking == null)
            {
                throw new BookingNotFoundException();
            }
            return booking;
        }

        
        private async Task<Booking> CreateNewBooking(int busId, int userId, DateTime travelDate, int numberOfSeats, List<int>seatIds)
        {
            var newBooking = new Booking
            {
                BookingDate = DateTime.Now,
                BookedForWhichDate = travelDate,
                BusId = busId,
                UserId = userId,
                NumberOfSeats = numberOfSeats,
                Status = "ongoing"
            };

            // Add the new booking to the context
            var addedBooking = await _bookingRepository.Add(newBooking);

          
            // Use the generated BookingId in subsequent operations
            int bookingId = addedBooking.BookingId;
            _logger.LogInformation($"BookingId: {bookingId}");

            //await CreateTicket(bookingId, seatId, busId);
            //await _seatService.ChangeSeatAvailablityAsync(seatId, busId);
            foreach (var seatId in seatIds)
            {
                await CreateTicket(bookingId, seatId, busId);
                await _seatService.ChangeSeatAvailablityAsync(seatId, busId);
            }
            return addedBooking;

        }

        private async Task CreateTicket(int bookingId, int seatId, int busId)
        {
            var seatPrice = await _seatService.GetSeatPriceAsync(seatId, busId);
            var newTicket = new Ticket
            {
                BookingId = bookingId,
                SeatId = seatId,
                BusId = busId,
                Price = seatPrice
            };

            await _ticketRepository.Add(newTicket);

            _logger.LogInformation($"Ticket created for SeatId: {seatId}, BookingId: {bookingId}");
        }







        
        public async Task<Booking> MakeBooking(int busId, List<int> seatIds, DateTime travelDate, int userId, int totalSeats)
        {
            try
            {
                await UpdateOngoingBookingsAndResetSeats();

                _logger.LogInformation($"Attempting to make booking for BusId: {busId}, SeatIds: {string.Join(",", seatIds)}, TravelDate: {travelDate}, UserId: {userId}");

                foreach (var seatId in seatIds)
                {
                    var seatStatus = await _seatService.CheckWhetherSeatIsAvailableForBooking(busId, seatId, travelDate);

                    if (!seatStatus)
                    {
                        _logger.LogError($"No seats available for booking. BusId: {busId}, SeatId: {seatId}, TravelDate: {travelDate}");
                        throw new NoSeatsAvailableException(); // Halt the booking process if a seat is not available
                    }
                }

                // If all seats are available, proceed to create a single booking for all the seats
                var createdBooking = await CreateNewBooking(busId, userId, travelDate, totalSeats, seatIds); // Use the first seat ID to create the booking
                _logger.LogInformation($"Booking successful. BookingId: {createdBooking.BookingId}, NumberOfSeats: {totalSeats}");
                return createdBooking;
            }
            catch (BusNotFoundException ex)
            {
                _logger.LogError($"Bus not found. Error: {ex.Message}");
                throw;
            }
            catch (NoSeatsAvailableException ex)
            {
                _logger.LogError($"No seats available. Error: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An unexpected error occurred. Error: {ex.Message}");
                throw new Exception("Internal Server Error");
            }
        }





        public async Task<Bus> GetBookingInfo(int bookingId, DateTime? bookedForWhichDate)
        {
            // Implement logic to get the required information based on infoType
            // For example:
            var booking = await _bookingRepository.GetAsync(bookingId);
            if (booking != null && booking.BookedForWhichDate.Date == bookedForWhichDate?.Date)
            {
                // Return the required information
                // Example: return booking?.Bus?.BusName;
                //var routeInfo = booking.Bus.BusRoute.
                //    Select(route => route.Route);
                
                _logger.LogInformation($"Bus Info. BusName:{booking.Bus.BusName}");
                return booking.Bus;
            }
            return null;
        }

        //public async Task<Bus> GetRouteInfo(int busId, DateTime? travelDate)
        //{
        //    // Implement logic to get the required information based on infoType
        //    // For example:
        //    var busRoute = await _busRouteRepository.GetAsync();
        //    if (busRoute != null && busRoute)
        //    {
        //        // Return the required information
        //        // Example: return booking?.Bus?.BusName;
        //        //var routeInfo = booking.Bus.BusRoute.
        //        //    Select(route => route.Route);

        //        _logger.LogInformation($"Bus Info. BusName:{booking.Bus.BusName}");
        //        return booking.Bus;
        //    }
        //    return null;
        //}
        public async Task<List<CompletedBookingDTO>> GetCompletedBookings(int userId)
        {
            //var users = await _userRepository.GetAsync();
            //var user = users.Where(user => user.UserId == userId).ToList();
            var user = await _userRepository.GetAsync(userId);

            if (user == null 
           
                //|| !user.Any()
                )
            {
                throw new NoSuchUserException();
            }

                    var completedBookings = user?.Bookings
            .Where(b => b.Status == "Complete")
            .Select(async b =>
            {
                var bus = await GetBookingInfo(b.BookingId, b.BookedForWhichDate);
                //var route = await GetRouteInfo(bus.BusId, b.BookedForWhichDate);
                return new CompletedBookingDTO
                {
                    BookingId = b.BookingId,
                    BusName = bus?.BusName,
                    BusType = bus?.BusType,
                    NumberOfSeats = b.NumberOfSeats,
                    BookedForWhichDate = b.BookedForWhichDate,
                    Origin = bus?.BusRoute?.FirstOrDefault()?.Route?.Origin,
                    Destination = bus?.BusRoute?.FirstOrDefault()?.Route?.Destination,
                    SeatNumbers = b.Tickets != null ? string.Join(",", b.Tickets.Select(t => t.SeatId)) : ""
                };
            })
        .Select(task => task.Result) // Wait for each async task to complete
        .ToList();
            //.Select(b => new CompletedBookingDTO
            //{
            //    bookingId = b.BookingId,
            //    BusName = b?.Bus?.BusName,
            //    BusType = b?.Bus?.BusType,
            //    NumberOfSeats = b?.NumberOfSeats ?? 0,
            //    BookedForWhichDate = b?.BookedForWhichDate,
            //    Origin = b?.Bus?.BusRoute?.FirstOrDefault()?.Route?.Origin,
            //    Destination = b?.Bus?.BusRoute?.FirstOrDefault()?.Route?.Destination,

            //    SeatNumbers = b?.Tickets != null ? string.Join(",", b.Tickets.Select(t => t.SeatId)) : ""
            //}).ToList();


            return completedBookings;
        }


            private async Task<Booking> DeleteAsync(int bookingId)
        {
            var booking = await _context.Bookings.FindAsync(bookingId);
            if (booking != null)
            {
                _context.Bookings.Remove(booking);
                await _context.SaveChangesAsync();
            }
            return booking;
        }

        public async Task UpdateOngoingBookingsAndResetSeats()
        {
            // Get the current time
            DateTime now = DateTime.Now;

            // Calculate the time 10 minutes ago
            DateTime tenMinutesAgo = now.AddMinutes(-10);

            // Retrieve all bookings from the repository
            var bookings = await _bookingRepository.GetAsync();

            // Filter ongoing bookings that meet the 10 minutes condition
            var ongoingBookings = bookings
                .Where(b => b.BookingDate <= tenMinutesAgo && b.Status == "ongoing")
                .ToList();

            // Update seat availability for each ticket in the ongoing bookings
            foreach (var booking in ongoingBookings)
            {
                foreach (var ticket in booking.Tickets)
                {
                    // Call ChangeSeatAvailablityAsync to update seat availability
                    await _seatService.ChangeSeatAvailablity(ticket.SeatId, ticket.BusId);
                }
                // Delete the booking
                await DeleteAsync(booking.BookingId);
            }
        }



        public async Task<List<CompletedBookingDTO>> GetUpcomingBookings(int userId)
        {
            var completedBookings = await GetCompletedBookings(userId);
            var today = DateTime.Today;

            var upcomingBookings = completedBookings
                .Where(b => b.BookedForWhichDate > today)
                .ToList();

            return upcomingBookings;
        }

        public async Task<List<CompletedBookingDTO>> GetCancelledBookings(int userId)
        {
            //var users = await _userRepository.GetAsync();
            //var user = users.Where(user => user.UserId == userId).ToList();
            var user = await _userRepository.GetAsync(userId);

            if (user == null

                //|| !user.Any()
                )
            {
                throw new NoSuchUserException();
            }

            var cancelledBookings = user?.Bookings
    .Where(b => b.Status == "cancelled" || b.Status == "refunded")
    .Select(async b =>
    {
        var bus = await GetBookingInfo(b.BookingId, b.BookedForWhichDate);
        //var route = await GetRouteInfo(bus.BusId, b.BookedForWhichDate);
        return new CompletedBookingDTO
        {
            Status = b.Status,
            BookingId = b.BookingId,
            BusName = bus?.BusName,
            BusType = bus?.BusType,
            NumberOfSeats = b.NumberOfSeats,
            BookedForWhichDate = b.BookedForWhichDate,
            Origin = bus?.BusRoute?.FirstOrDefault()?.Route?.Origin,
            Destination = bus?.BusRoute?.FirstOrDefault()?.Route?.Destination,
            SeatNumbers = b.Tickets != null ? string.Join(",", b.Tickets.Select(t => t.SeatId)) : ""
        };
    })
.Select(task => task.Result) // Wait for each async task to complete
.ToList();
            //.Select(b => new CompletedBookingDTO
            //{
            //    bookingId = b.BookingId,
            //    BusName = b?.Bus?.BusName,
            //    BusType = b?.Bus?.BusType,
            //    NumberOfSeats = b?.NumberOfSeats ?? 0,
            //    BookedForWhichDate = b?.BookedForWhichDate,
            //    Origin = b?.Bus?.BusRoute?.FirstOrDefault()?.Route?.Origin,
            //    Destination = b?.Bus?.BusRoute?.FirstOrDefault()?.Route?.Destination,

            //    SeatNumbers = b?.Tickets != null ? string.Join(",", b.Tickets.Select(t => t.SeatId)) : ""
            //}).ToList();


            return cancelledBookings;
        }


//        public async Task<List<CompletedBookingDTO>> GetCancelledBookings()
//        {
//            //var users = await _userRepository.GetAsync();
//            //var user = users.Where(user => user.UserId == userId).ToList();
//            var users = await _userRepository.GetAsync();

//            if (user == null

//                //|| !user.Any()
//                )
//            {
//                throw new NoSuchUserException();
//            }

//            var cancelledBookings = user?.Bookings
//    .Where(b => b.Status == "cancelled")
//    .Select(async b =>
//    {
//        var bus = await GetBookingInfo(b.BookingId, b.BookedForWhichDate);
//        //var route = await GetRouteInfo(bus.BusId, b.BookedForWhichDate);
//        return new CompletedBookingDTO
//        {
//            BookingId = b.BookingId,
//            BusName = bus?.BusName,
//            BusType = bus?.BusType,
//            NumberOfSeats = b.NumberOfSeats,
//            BookedForWhichDate = b.BookedForWhichDate,
//            Origin = bus?.BusRoute?.FirstOrDefault()?.Route?.Origin,
//            Destination = bus?.BusRoute?.FirstOrDefault()?.Route?.Destination,
//            SeatNumbers = b.Tickets != null ? string.Join(",", b.Tickets.Select(t => t.SeatId)) : ""
//        };
//    })
//.Select(task => task.Result) // Wait for each async task to complete
//.ToList();
//            //.Select(b => new CompletedBookingDTO
//            //{
//            //    bookingId = b.BookingId,
//            //    BusName = b?.Bus?.BusName,
//            //    BusType = b?.Bus?.BusType,
//            //    NumberOfSeats = b?.NumberOfSeats ?? 0,
//            //    BookedForWhichDate = b?.BookedForWhichDate,
//            //    Origin = b?.Bus?.BusRoute?.FirstOrDefault()?.Route?.Origin,
//            //    Destination = b?.Bus?.BusRoute?.FirstOrDefault()?.Route?.Destination,

//            //    SeatNumbers = b?.Tickets != null ? string.Join(",", b.Tickets.Select(t => t.SeatId)) : ""
//            //}).ToList();


//            return cancelledBookings;
//        }


        public async Task<List<CompletedBookingDTO>> GetPastBookings(int userId)
        {
            var completedBookings = await GetCompletedBookings(userId);
            var today = DateTime.Today;

            var pastBookings = completedBookings
                .Where(b => b.BookedForWhichDate < today)
                .ToList();

            return pastBookings;
        }


        public async Task<Booking> CancelBooking(int userId, int bookingId)
        {
            // Retrieve the booking from the repository based on the provided bookingId
            var booking = await _bookingRepository.GetAsync(bookingId);

            // Check if the booking exists and belongs to the provided userId
            if (booking != null && booking.UserId == userId)
            {
                // Update the booking status to "cancelled"
                booking.Status = "cancelled";

                // Save the changes to the database
                await _bookingRepository.Update(booking);

                return booking;
            }

            // Return false if the booking doesn't exist or doesn't belong to the provided userId

            throw new BookingNotFoundException();
        }


        

    }

}
