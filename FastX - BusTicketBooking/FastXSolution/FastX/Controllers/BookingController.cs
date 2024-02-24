using FastX.Exceptions;
using FastX.Interfaces;
using FastX.Models;
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

        [HttpPost]
        public async Task<IActionResult> MakeBooking(int busId, int seatId, DateTime travelDate, int userId)
        {
            try
            {
                _logger.LogInformation($"Attempting to make booking for BusId: {busId}, SeatId: {seatId}, TravelDate: {travelDate}, UserId: {userId}");

                await _bookingService.MakeBooking(busId, seatId, travelDate, userId);

                _logger.LogInformation("Booking successful.");
                return Ok();
            }
            catch (BusNotFoundException ex)
            {
                _logger.LogError($"Bus not found. BusId: {busId}. Error: {ex.Message}");
                return NotFound("Bus not found");
            }
            catch (NoSeatsAvailableException ex)
            {
                _logger.LogError($"Seat not found. SeatId: {seatId}, BusId: {busId}. Error: {ex.Message}");
                return NotFound("Seat not found");
            }

            catch (Exception ex)
            {
                _logger.LogError($"An unexpected error occurred. Error: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }


    }
}