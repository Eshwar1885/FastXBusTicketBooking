using FastX.Exceptions;
using FastX.Interfaces;
using FastX.Models;
using FastX.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FastX.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("ReactPolicy")]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;
        private readonly ITicketService _ticketService;
        private readonly ILogger<BookingController> _logger;
        private readonly ISeatService _seatService;
        private readonly IBusOperatorService _busOperatorService;



        public BookingController(IBookingService bookingService,
            ITicketService ticketService,
            ILogger<BookingController> logger,
            ISeatService seatService,
            IBusOperatorService busOperatorService)
        {
            _bookingService = bookingService;
            //_seatService = seatService;
            _ticketService = ticketService;
            _logger = logger;
            _seatService = seatService;
            _busOperatorService = busOperatorService;
        }

        //[HttpPost]
        //public async Task<IActionResult> MakeBooking(int busId, List<int>seatIds, DateTime travelDate, int userId, int totalSeats)
        //{
        //    try
        //    {
        //        _logger.LogInformation($"Attempting to make booking for BusId: {busId}, SeatIds: {string.Join(",", seatIds)}, TravelDate: {travelDate}, UserId: {userId}");

        //        await _bookingService.MakeBooking(busId, seatIds, travelDate, userId, totalSeats);

        //        _logger.LogInformation("Booking successful.");
        //        return Ok();
        //    }
        //    catch (BusNotFoundException ex)
        //    {
        //        _logger.LogError($"Bus not found. BusId: {busId}. Error: {ex.Message}");
        //        return NotFound("Bus not found");
        //    }
        //    catch (NoSeatsAvailableException ex)
        //    {
        //        _logger.LogError($"Seat not found. SeatId: {seatId}, BusId: {busId}. Error: {ex.Message}");
        //        return NotFound("Seat not found");
        //    }

        //    catch (Exception ex)
        //    {
        //        _logger.LogError($"An unexpected error occurred. Error: {ex.Message}");
        //        return StatusCode(500, "Internal Server Error");
        //    }
        //}



        [HttpPost]
        public async Task<ActionResult<Booking>> MakeBooking(BookingDTO booking)
        {
            try
            {
                //_logger.LogInformation($"Attempting to make booking for BusId: {busId}, SeatIds: {string.Join(",", seatIds)}, TravelDate: {travelDate}, UserId: {userId}");

                var bookingDetails = await _bookingService.MakeBooking(booking.BusId, booking.SeatIds, booking.TravelDate, booking.UserId, booking.TotalSeats);

                _logger.LogInformation("Booking successful.");
                return Ok(bookingDetails);
            }
            catch (BusNotFoundException ex)
            {
                //_logger.LogError($"Bus not found. BusId: {busId}. Error: {ex.Message}");
                return NotFound("Bus not found");
            }
            catch (NoSeatsAvailableException ex)
            {
                //_logger.LogError($"No seats available for booking. BusId: {busId}, Error: {ex.Message}");
                return NotFound("No seats available for booking");
            }
            catch (Exception ex)
            {
                //_logger.LogError($"An unexpected error occurred. Error: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }




        [HttpGet("completed/{userId}")]
        public async Task<ActionResult<List<CompletedBookingDTO>>> GetCompletedBookings(int userId)
        {
            try
            {
                var completedBookings = await _bookingService.GetCompletedBookings(userId);
                return Ok(completedBookings);
            }
            catch (NoSuchUserException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        //[Authorize(Roles = "user")]
        [HttpGet("upcoming/{userId}")]
        public async Task<ActionResult<List<CompletedBookingDTO>>> GetUpcomingBookings(int userId)
        {
            try
            {
                var upcomingBookings = await _bookingService.GetUpcomingBookings(userId);
                return Ok(upcomingBookings);
            }
            catch (NoSuchUserException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("past/{userId}")]
        public async Task<ActionResult<List<CompletedBookingDTO>>> GetPastBookings(int userId)
        {
            try
            {
                var pastBookings = await _bookingService.GetPastBookings(userId);
                return Ok(pastBookings);
            }
            catch (NoSuchUserException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("UpdateOngoingBookingsAndResetSeats")]
        public async Task<ActionResult> UpdateOngoingBookingsAndResetSeats()
        {
            await _bookingService.UpdateOngoingBookingsAndResetSeats();


            return Ok();
        }

        [HttpGet("ChangeStatus")]
        public async Task<ActionResult>ChangeStatus(int seatId, int busId)
        {
            await _seatService.ChangeSeatAvailablity(seatId,busId);


            return Ok();
        }


        //[HttpGet("ChangeStatusToCancel")]
        //public async Task<ActionResult<Booking>> CancelBooking(int userId, int bookingId)
        //{
        //    var booking = await _bookingService.CancelBooking(userId, bookingId);
        //    return Ok(booking);

        //}


        [HttpGet("getcancelled/{userId}")]
        public async Task<ActionResult<List<CompletedBookingDTO>>> GetCancelledBookings(int userId)
        {
            try
            {
                var cancelledBookings = await _bookingService.GetCancelledBookings(userId);
                return Ok(cancelledBookings);
            }
            catch (BookingNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("cancel")]
        public async Task<ActionResult> CancelBooking(int bookingId, int userId)
        {
            try
            {
                var cancelledBooking = await _bookingService.CancelBooking(userId, bookingId);
                await _seatService.ChangeSeatAvailabilityForCancelledBookings();
                return Ok();
            }
            catch (BookingNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        //[HttpGet("cancel")]
        //public async Task<ActionResult<RefundDTO>> CancelBooking(int bookingId, int userId)
        //{
        //    try
        //    {
        //        var cancelledBooking = await _bookingService.CancelBooking(userId, bookingId);
        //        await _seatService.ChangeSeatAvailabilityForCancelledBookings();
        //        var refund = await _busOperatorService.RefundRequest(userId);
        //        return Ok(refund);
        //    }
        //    catch (BookingNotFoundException ex)
        //    {
        //        return NotFound(ex.Message);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, ex.Message);
        //    }
        //}






    }
}