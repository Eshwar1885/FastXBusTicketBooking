using FastX.Exceptions;
using FastX.Interfaces;
using FastX.Models;
using FastX.Models.DTOs;
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

        public BookingController(IBookingService bookingService,
            ITicketService ticketService,
            ILogger<BookingController> logger)
        {
            _bookingService = bookingService;
            //_seatService = seatService;
            _ticketService = ticketService;
            _logger = logger;
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



    }
}