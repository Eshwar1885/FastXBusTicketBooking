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
    public class BusController : ControllerBase
    {
        private readonly IBusService _busService;
        private readonly ILogger<BusController> _logger;

        public BusController(IBusService busService,
            ILogger<BusController> logger)
        {
            _busService = busService;
            _logger = logger;
        }

        //[Authorize(Roles = "busoperator")]
        [HttpPost("AddBusByBusOperator")]
        public async Task<IActionResult> AddBus([FromBody]AddBusAndSeatsDTO bus)
        {
            try
            {
                await _busService.AddBus(bus.BusName, bus.BusType, bus.TotalSeats, bus.BusOperatorId, bus.SeatPrice);
                return Ok();
            }
            catch (BusOperatorNotFoundException ex)
            {
                _logger.LogWarning(ex.Message);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding bus");
                return StatusCode(500, "Internal server error");
            }
        }

        //[Authorize(Roles = "busoperator")]
        [HttpDelete("DeleteBusByBusOperator")]
        public async Task<IActionResult> DeleteBus(int busId)
        {
            try
            {
                var result = await _busService.DeleteBus(busId);
                if (result != null)
                    return Ok("Bus deleted successfully");
                else
                    return NotFound("Bus not found");
            }
            catch (BusNotFoundException ex)
            {
                _logger.LogWarning(ex.Message);
                return NotFound(ex.Message);
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting bus");
                return StatusCode(500, "Internal server error");
            }
        }


        //[HttpGet("search")]
        //public async Task<ActionResult<List<BusDTOForUser>>> SearchBusesAsync(string origin, string destination, DateTime date)
        //{
        //    try
        //    {
        //        var availableBuses = await _busService.GetAvailableBuses(origin, destination, date);
        //        _logger.LogInformation("Successfully retrieved available buses.");

        //        return Ok(availableBuses);
        //    }

        //    catch (BusNotFoundException ex)
        //    {
        //        _logger.LogError($"BusNotFoundException: {ex.Message}");
        //        return NotFound(ex.Message);
        //    }

        //    catch (Exception ex)
        //    {
        //        _logger.LogError($"An unexpected error occurred: {ex.Message}");
        //        return StatusCode(500, "Internal Server Error");
        //    }
        //}

        //=========frnt end=====correct one uncomment
        [HttpGet("search")]
        public async Task<ActionResult<List<BusDTOForUser>>> SearchBusesAsync([FromQuery] BusInputDTO bus)
        {
            try
            {
                var availableBuses = await _busService.GetAvailableBuses(bus.Origin, bus.Destination, bus.TravelDate);
                _logger.LogInformation("Successfully retrieved available buses.");

                return Ok(availableBuses);
            }

            catch (BusNotFoundException ex)
            {
                _logger.LogError($"BusNotFoundException: {ex.Message}");
                return NotFound(ex.Message);
            }

            catch (Exception ex)
            {
                _logger.LogError($"An unexpected error occurred: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }



        //[HttpGet("search")]

        //public async Task<ActionResult<List<BusDtoForUser>>> SearchBusesAsync(string origin, string destination, DateTime date)
        //{
        //    try
        //    {
        //        var busDtos = await _busService.SearchBusesAsync(origin, destination, date);
        //        return Ok(busDtos);
        //    }
        //    catch (BusNotFoundException ex)
        //    {
        //        return NotFound(ex.Message);
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log the exception for debugging purposes
        //        Console.WriteLine(ex.Message);
        //        return StatusCode(500, "An error occurred while processing your request.");
        //    }
        //}
        //[HttpGet("searchWithBusType")]

        //public async Task<ActionResult<List<BusDTOForUser>>> SearchBusesWithTypeAsync(string origin, string destination, DateTime date, string busType)
        //{
        //    try
        //    {
        //        var busDtos = await _busService.GetAvailableBuses(origin, destination, date, busType);
        //        return Ok(busDtos);
        //    }
        //    catch (BusNotFoundException ex)
        //    {
        //        return NotFound(ex.Message);
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log the exception for debugging purposes
        //        Console.WriteLine(ex.Message);
        //        return StatusCode(500, "An error occurred while processing your request.");
        //    }
        //}


        [HttpGet("searchWithBusType")]
        public async Task<ActionResult<List<BusDTOForUser>>> SearchBusesWithTypeAsync([FromQuery] string origin, [FromQuery] string destination, [FromQuery] DateTime date, [FromQuery] string busType)
        {
            try
            {
                var busDtos = await _busService.GetAvailableBuses(origin, destination, date, busType);
                return Ok(busDtos);
            }
            catch (BusNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes
                Console.WriteLine(ex.Message);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }




    }
}
