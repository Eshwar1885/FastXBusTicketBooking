using FastX.Exceptions;
using FastX.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FastX.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeatsController : ControllerBase
    {
        private readonly ISeatService _seatService;
        private readonly ILogger<SeatsController> _logger;

        public SeatsController(ISeatService seatService,
            ILogger<SeatsController> logger)
        {
            _seatService = seatService;
            _logger = logger;
        }
        [HttpGet("GetAvailableSeats")]
        public async Task<IActionResult> GetAvailableSeats(int busId)
        {
            try
            {
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
    }
}