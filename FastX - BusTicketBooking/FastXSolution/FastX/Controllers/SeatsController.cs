using FastX.Exceptions;
using FastX.Interfaces;
using FastX.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FastX.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("ReactPolicy")]
    public class SeatsController : ControllerBase
    {
        private readonly ISeatService _seatService;
        private readonly ILogger<SeatsController> _logger;
        private readonly IBusService _busService;
        private readonly IBookingService _bookingService;

        public SeatsController(ISeatService seatService,
            ILogger<SeatsController> logger,
            IBusService busService,
            IBookingService bookingService)
        {
            _seatService = seatService;
            _logger = logger;
            _busService = busService;
            _bookingService = bookingService;
        }
        [HttpGet("GetAvailableSeats")]
        public async Task<IActionResult> GetAvailableSeats(int busId)
        {
            try
            {
                await _bookingService.UpdateOngoingBookingsAndResetSeats();
                var availableSeats = await _seatService.GetAvailableSeats(busId);
                _logger.LogInformation("Retrieved the available seats");
                return Ok(availableSeats);

            }
            catch (BusNotFoundException ex)
            {
                _logger.LogError($"BusNotFoundException: {ex.Message}");
                return NotFound(ex.Message);
            }
            catch (NoSeatsAvailableException ex)
            {
                _logger.LogError($"NoSeatsAvailableException: {ex.Message}");
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An unexpected error occurred: {ex.Message}");
                return StatusCode(500, "Internal Server Error");

            }
        }

        [HttpGet("CheckWhetherSeatIsAvailableForBooking")]
        public async Task<IActionResult> CheckWhetherSeatIsAvailableForBooking(int busId, int seatId, DateTime date)
        {
            var ans = await _seatService.CheckWhetherSeatIsAvailableForBooking(busId, seatId, date);
            return Ok(ans);


        }

        [HttpGet("ChangeSeatAvailabilityForCancelledBookings")]
        public async Task<IActionResult> ChangeSeatAvailabilityForCancelledBookings()
        {
            await _seatService.ChangeSeatAvailabilityForCancelledBookings();
            return Ok();
        }



        [HttpGet("totalseats")]
        public async Task<IActionResult> GetTotalSeats(int busId)
        {
            try
            {
                int totalSeats = await _busService.GetTotalSeatsAsync(busId);
                var response = new
                {
                    BusId = busId,
                    TotalSeats = totalSeats,
                    Message = "Total seats retrieved successfully."
                };

                return Ok(response);
            }
            catch (BusNotFoundException ex)
            {
                var errorResponse = new
                {
                    Message = $"Bus not found: {ex.Message}"
                };

                return NotFound(errorResponse);
            }
            catch (Exception ex)
            {
                var errorResponse = new
                {
                    Message = $"An error occurred: {ex.Message}"
                };

                return StatusCode(500, errorResponse);
            }
        }
    }
}