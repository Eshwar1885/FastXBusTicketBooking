using FastX.Exceptions;
using FastX.Interfaces;
using FastX.Models;
using FastX.Repositories;
using System.Numerics;

namespace FastX.Services
{
    public class BookingService : IBookingService
    {
        private readonly IRepository<int, Ticket> _ticketRepository;
        private readonly IBookingRepository<int, Booking> _bookingRepository;
        // private readonly IBookingRepository<int, Booking> _booking2Repository;
        //private readonly IPaymentRepository _paymentRepository;
        private readonly ISeatService _seatService;
        private readonly ILogger<BookingService> _logger;

        public BookingService(
            IRepository<int, Ticket> ticketRepository,
            IBookingRepository<int, Booking> bookingRepository,
           // IPaymentRepository paymentRepository,
           ISeatService seatService,
           IBookingRepository<int, Booking> booking2Repository,
        ILogger<BookingService> logger)
        {
            _ticketRepository = ticketRepository;
            _bookingRepository = bookingRepository;
            //_booking2Repository = booking2Repository;
            //_paymentRepository = paymentRepository;
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

        private async Task<Booking> CreateNewBooking(int busId, int userId, DateTime travelDate, int numberOfSeats, int seatId)
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
            await _seatService.ChangeSeatAvailablityAsync(seatId);
            return await _bookingRepository.Add(newBooking);

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

        public async Task MakeBooking(int busId, int seatId, DateTime travelDate, int userId)
        {
            try
            {
                _logger.LogInformation($"Attempting to make booking for BusId: {busId}, SeatId: {seatId}, TravelDate: {travelDate}, UserId: {userId}");

                var seatStatus = await _seatService.CheckWhetherSeatIsAvailableForBooking(busId, seatId, travelDate);

                if (!seatStatus)
                {
                    _logger.LogError($"No seats available for booking. BusId: {busId}, SeatId: {seatId}, TravelDate: {travelDate}");
                    throw new NoSeatsAvailableException();
                }

                var ongoingBooking = await _bookingRepository.GetOngoingBookingAsync(busId, userId, travelDate);

                if (ongoingBooking == null)
                {
                    var createdBooking = await CreateNewBooking(busId, userId, travelDate, 1, seatId);

                    _logger.LogInformation($"Booking successful. BusId: {busId}, SeatId: {seatId}, TravelDate: {travelDate}, UserId: {userId}");

                    // Now, create a ticket for the booked seat
                    await CreateTicket(createdBooking.BookingId, seatId, busId);
                }
                else
                {
                    await ChangeNoOfSeatsAsync(ongoingBooking.BookingId, ongoingBooking.NumberOfSeats + 1);
                    await _seatService.ChangeSeatAvailablityAsync(seatId);
                    _logger.LogInformation($"Added seat to existing booking. BookingId: {ongoingBooking.BookingId}, NewNumberOfSeats: " +
                        $"{ongoingBooking.NumberOfSeats + 1}");

                    // Now, create a ticket for the newly added seat
                    await CreateTicket(ongoingBooking.BookingId, seatId, busId);
                }
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
        //latest code before seat booke exception

        //public async Task MakeBooking(int busId, int seatId, DateTime travelDate, int userId)
        //{
        //    try
        //    {
        //        _logger.LogInformation($"Attempting to make booking for BusId: {busId}, SeatId: {seatId}, TravelDate: {travelDate}, UserId: {userId}");

        //        var seatStatus = await _seatService.CheckWhetherSeatIsAvailableForBooking(busId, seatId, travelDate);

        //        if (!seatStatus)
        //        {
        //            _logger.LogError($"Seat {seatId} is not available for booking on BusId: {busId}, TravelDate: {travelDate}");
        //            throw new NoSeatsAvailableException();
        //        }

        //        var isAlreadyBooked = await _bookingRepository.IsSeatAlreadyBookedAsync(busId, seatId, travelDate);

        //        if (isAlreadyBooked)
        //        {
        //            _logger.LogError($"Seat {seatId} is already booked on BusId: {busId}, TravelDate: {travelDate}");
        //            throw new SeatAlreadyBookedException();
        //        }

        //        var ongoingBooking = await _bookingRepository.GetOngoingBookingAsync(busId, userId, travelDate);

        //        if (ongoingBooking == null)
        //        {
        //            var createdBooking = await CreateNewBooking(busId, userId, travelDate, 1);

        //            _logger.LogInformation($"Booking successful. BusId: {busId}, SeatId: {seatId}, TravelDate: {travelDate}, UserId: {userId}");

        //            // Now, create a ticket for the booked seat
        //            await CreateTicket(createdBooking.BookingId, seatId, busId);
        //        }
        //        else
        //        {
        //            await ChangeNoOfSeatsAsync(ongoingBooking.BookingId, ongoingBooking.NumberOfSeats + 1);
        //            _logger.LogInformation($"Added seat to existing booking. BookingId: {ongoingBooking.BookingId}, NewNumberOfSeats: " +
        //                $"{ongoingBooking.NumberOfSeats + 1}");

        //            // Now, create a ticket for the newly added seat
        //            await CreateTicket(ongoingBooking.BookingId, seatId, busId);
        //        }
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
        //    catch (SeatAlreadyBookedException ex)
        //    {
        //        _logger.LogError($"Seat already booked. Error: {ex.Message}");
        //        throw;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError($"An unexpected error occurred. Error: {ex.Message}");
        //        throw new Exception("Internal Server Error");
        //    }
        //}


        //    public async Task MakeBooking(int busId, int seatId, DateTime travelDate, int userId)
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

        //        var ongoingBooking = await _bookingRepository.GetOngoingBookingAsync(busId, userId, travelDate);

        //        if (ongoingBooking == null)
        //        {
        //            var newBooking = new Booking
        //            {
        //                BookingDate = DateTime.Now,
        //                BookedForWhichDate = travelDate,
        //                BusId = busId,
        //                UserId = userId,
        //                NumberOfSeats = 1, // Assuming you want to book one seat initially
        //                Status = "ongoing"
        //            };


        //            await _bookingRepository.Add(newBooking);
        //            await _seatService.ChangeSeatAvailablityAsync(seatId);

        //            _logger.LogInformation($"Booking successful. BusId: {busId}, SeatId: {seatId}, TravelDate: {travelDate}, UserId: {userId}");
        //        }
        //        else
        //        {
        //            await ChangeNoOfSeatsAsync(ongoingBooking.BookingId, ongoingBooking.NumberOfSeats + 1);
        //            _logger.LogInformation($"Added seat to existing booking. BookingId: {ongoingBooking.BookingId}, NewNumberOfSeats: {ongoingBooking.NumberOfSeats + 1}");
        //        }
        //    }
        //    catch (NoSeatsAvailableException ex)
        //    {
        //        _logger.LogError($"No seats available. Error: {ex.Message}");
        //        throw; // Re-throw the exception for the controller to handle
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError($"An unexpected error occurred. Error: {ex.Message}");
        //        throw new Exception("Internal Server Error");
        //    }
        //}


    }

}