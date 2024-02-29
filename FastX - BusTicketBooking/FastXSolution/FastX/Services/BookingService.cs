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


        public BookingService(
            IRepository<int, Ticket> ticketRepository,
            IBookingRepository<int, Booking> bookingRepository,
            IRepository<int, User> userRepository,
           ISeatService seatService,
           IBookingRepository<int, Booking> booking2Repository,
        ILogger<BookingService> logger)
        {
            _ticketRepository = ticketRepository;
            _bookingRepository = bookingRepository;
            //_booking2Repository = booking2Repository;
            _userRepository = userRepository;
            _seatService = seatService;
            _logger = logger;
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

        //public async Task MakeBooking(int busId, int seatId, DateTime travelDate, int userId)
        //{
        //    int noOfSeats = 0;
        //    var seatStatus=await _seatService.CheckWhetherSeatIsAvailableForBooking(busId, seatId, travelDate);
        //    if (seatStatus == false)
        //    {
        //        throw new NoSeatsAvailableException();
        //    }

        //    var ongoingBooking = await _booking2Repository.GetOngoingBookingAsync(busId, userId, travelDate);
        //    int bookingId = ongoingBooking.BookingId;
        //    int noOfSeatsPresent = ongoingBooking.NumberOfSeats;
        //    if (ongoingBooking == null)
        //    {
        //        var newBooking = new Booking
        //        {
        //            BookingDate = DateTime.Now,
        //            BookedForWhichDate = travelDate,
        //            BusId = busId,
        //            UserId = userId,
        //            NumberOfSeats = noOfSeats + 1,
        //            Status = "ongoing"


        //        };
        //        var addBooking = await _bookingRepository.Add(newBooking);

        //    }
        //    else
        //    {
        //       await  ChangeNoOfSeatsAsync(bookingId, noOfSeatsPresent+1);
        //    }

        //    //var newBooking = new Booking
        //    //{
        //    //    BookingDate = DateTime.Now,
        //    //    BookedForWhichDate = travelDate,
        //    //    BusId = busId,
        //    //    UserId = userId,
        //    //    NumberOfSeats = noOfSeats+1,
        //    //    Status="ongoing"


        //    //};
        //    //var ongoingBooking = await _bookingRepository.Add(newBooking);
        //}

        //private async Task<Booking> CreateNewBooking(int busId, int userId, DateTime travelDate, int numberOfSeats, int seatId)
        //{
        //    var newBooking = new Booking
        //    {
        //        BookingDate = DateTime.Now,
        //        BookedForWhichDate = travelDate,
        //        BusId = busId,
        //        UserId = userId,
        //        NumberOfSeats = numberOfSeats,
        //        Status = "ongoing"
        //    };
        //    await _seatService.ChangeSeatAvailablityAsync(seatId);
        //    return await _bookingRepository.Add(newBooking);

        //}


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







        //public async Task MakeBooking(int busId, int seatId, DateTime travelDate, int userId, int totalSeats)
        //{
        //    try
        //    {
        //        _logger.LogInformation($"Attempting to make booking for BusId: {busId}, SeatId: {seatId}, TravelDate: {travelDate}, UserId: {userId}");

        //        var seatStatus = await _seatService.CheckWhetherSeatIsAvailableForBooking(busId, seatId, travelDate);

        //        if (!seatStatus)
        //        {
        //            _logger.LogError($"No seats available for booking. BusId: {busId}, SeatId: {seatId}, TravelDate: {travelDate}");
        //            throw new NoSeatsAvailableException();
        //        }

        //        //var ongoingBooking = await _bookingRepository.GetOngoingBookingAsync(busId, userId, travelDate);

        //        var createdBooking = await CreateNewBooking(busId, userId, travelDate, totalSeats, seatId);
        //        _logger.LogInformation($"Added seat to existing booking. BookingId:, NewNumberOfSeats: " +
        //            $"{totalSeats}");

        //        // Now, create a ticket for the newly added seat
        //        //await CreateTicket(BookingId, seatId, busId);

        //    }
        //    catch (BusNotFoundException ex)
        //    {
        //        _logger.LogError($"Bus not found. Error: {ex.Message}");
        //        throw;
        //    }
        //    catch (NoSeatsAvailableException ex)
        //    {
        //        _logger.LogError($"No seats available. Error: {ex.Message}");
        //        throw;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError($"An unexpected error occurred. Error: {ex.Message}");
        //        throw new Exception("Internal Server Error");
        //    }
        //}




        public async Task<Booking> MakeBooking(int busId, List<int> seatIds, DateTime travelDate, int userId, int totalSeats)
        {
            try
            {
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






        public async Task<List<CompletedBookingDTO>> GetCompletedBookings(int userId)
        {
            var users = await _userRepository.GetAsync();
            var user = users.Where(user => user.UserId == userId).ToList();
            if (user == null || !user.Any())
            {
                throw new NoSuchUserException();
            }

                    var completedBookings = user[0]?.Bookings
            .Where(b => b.Status == "Complete")
            .Select(b => new CompletedBookingDTO
            {
                BusName = b?.Bus?.BusName,
                BusType = b?.Bus?.BusType,
                NumberOfSeats = b?.NumberOfSeats ?? 0,
                BookedForWhichDate = b?.BookedForWhichDate,
                Origin = b?.Bus?.BusRoute?.FirstOrDefault()?.Route?.Origin,
                Destination = b?.Bus?.BusRoute?.FirstOrDefault()?.Route?.Destination,
                SeatNumbers = b?.Tickets != null ? string.Join(",", b.Tickets.Select(t => t.SeatId)) : ""
            }).ToList();

            return completedBookings;
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

        public async Task<List<CompletedBookingDTO>> GetPastBookings(int userId)
        {
            var completedBookings = await GetCompletedBookings(userId);
            var today = DateTime.Today;

            var pastBookings = completedBookings
                .Where(b => b.BookedForWhichDate < today)
                .ToList();

            return pastBookings;
        }

    }

}
