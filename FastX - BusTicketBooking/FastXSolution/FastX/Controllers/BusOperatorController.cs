using FastX.Exceptions;
using FastX.Interfaces;
using FastX.Models;
using FastX.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FastX.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
        [Authorize(Roles = "admin")]

        [HttpGet("{busOperatorId}/buses")]
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
        [Authorize(Roles = "busoperator")]
        [HttpPost("amenities")]
        public async Task<IActionResult> AddAmenity(Amenity amenity)
        {
            try
            {
                var addedAmenity = await _amenityService.AddAmenity(amenity);
                return Ok(addedAmenity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding amenity.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }

        //[Authorize(Roles = "busoperator")]
        [HttpDelete("amenities/{id}")]
        public async Task<IActionResult> DeleteAmenity(int id)
        {
            try
            {
                var deletedAmenity = await _amenityService.DeleteAmenity(id);
                return Ok(deletedAmenity);
            }
            catch (AmenitiesNotFoundException ex)
            {
                _logger.LogWarning(ex.Message);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting amenity with ID: {AmenityId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }

        [Authorize(Roles = "busoperator")]
        [HttpPut("amenities")]
        public async Task<IActionResult> UpdateAmenity(int id, string name)
        {
            try
            {
                var updatedAmenity = await _amenityService.ChangeAmenityNameAsync(id, name);
                return Ok(updatedAmenity);
            }
            catch (AmenitiesNotFoundException ex)
            {
                _logger.LogWarning(ex.Message);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating amenity.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }
        //---------------------------
        // POST: api/Bus/AddAmenity 
        //this adds amenity when busid and amenity name is given
        [HttpPost("AddAmenityForBus")]
        public async Task<IActionResult> AddAmenity(int busId, string amenityName)
        {
            try
            {
                await _amenityService.AddAmenityToBus(busId, amenityName);
                return Ok("Amenity added successfully");
            }
            catch (NoSuchBusException ex)
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
    }
}
