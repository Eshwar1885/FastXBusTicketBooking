using FastX.Exceptions;
using FastX.Interfaces;
using FastX.Models;
using FastX.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FastX.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("ReactPolicy")]
    public class BusOperatorController : ControllerBase
    {
        private readonly IBusOperatorService _busOperatorService;
        private readonly IAmenityService _amenityService;
        private readonly ILogger<BusOperatorController> _logger;

        public BusOperatorController(IBusOperatorService busOperatorService, IAmenityService amenityService, ILogger<BusOperatorController> logger)
        {
            _busOperatorService = busOperatorService;
            _amenityService = amenityService;
            _logger = logger;
        }

        [Authorize(Roles = "busoperator")]
        //[Authorize(Roles = "admin")]

        [HttpGet("GetBusForBusOperator")]
        public async Task<IActionResult> GetBusesByOperatorId(int busOperatorId)
        {
            try
            {
                var buses = await _busOperatorService.GetAllBuses(busOperatorId);

                return Ok(buses);
            }
            catch (BusOperatorNotFoundException ex)
            {
                _logger.LogWarning(ex.Message);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving buses for bus operator ID: {BusOperatorId}", busOperatorId);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }








        //---------------------------
        // POST: api/Bus/AddAmenity 
        //this adds Amenity when BusId and Amenity name is given

        [Authorize(Roles = "busoperator")] //this will add into busamenities table
        [HttpPost("AddAmenityForBusByBusOperator")]
        public async Task<IActionResult> AddAmenity(int busId, string amenityName)
        {
            try
            {
                await _amenityService.AddAmenityToBus(busId, amenityName);
                return Ok("Amenity added successfully");
            }
            catch (BusNotFoundException ex)
            {
                _logger.LogError(ex, "Bus not found");
                return NotFound(ex.Message);
            }
            catch (AmenityAlreadyExistsException ex)
            {
                _logger.LogError(ex, "Amenity already exists for this bus");
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding amenity to bus");
                return StatusCode(500, "Internal server error");
            }
        }


        [Authorize(Roles = "busoperator")] //this will delete from busamenities table
        [HttpDelete("DeleteAmenityForBusByBusOperator")]
        public async Task<IActionResult> DeleteAmenity(int busId, string amenityName)
        {
            try
            {
                await _amenityService.DeleteAmenityFromBus(busId, amenityName);
                return Ok("Amenity deleted successfully");
            }
            catch (BusNotFoundException ex)
            {
                _logger.LogError(ex, "Bus not found");
                return NotFound(ex.Message);
            }
            catch (AmenitiesNotFoundException ex)
            {
                _logger.LogError(ex, "Amenity not found");
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting amenity from bus");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}