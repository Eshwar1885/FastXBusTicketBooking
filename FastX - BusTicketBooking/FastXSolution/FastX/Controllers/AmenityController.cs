using FastX.Exceptions;
using FastX.Interfaces;
using FastX.Models;
using FastX.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FastX.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AmenityController : ControllerBase
    {

        // private readonly IBusService _busService;
        private readonly IAmenityService _amenityService;
        private readonly ILogger _logger;

        public AmenityController(IAmenityService amenityService, ILogger<AmenityController> logger)
        {
            _amenityService = amenityService;
            _logger = logger;
        }
        //Gets amenity directly from amenity table


        [HttpGet("GetAmenityFromAmenityTable")]
        public async Task<IActionResult> GetAmenities()
        {
            try
            {
                var amenities = await _amenityService.GetAmenityList();

                if (amenities == null || amenities.Count == 0)
                {
                    return NotFound("No amenities found.");
                }

                return Ok(amenities);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving amenities.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }

        //[HttpGet("bus/{busId}/amenities")]
        //public async Task<ActionResult<List<Amenity>>> GetBusAmenities(int busId)
        //{
        //    try
        //    {
        //        // Call the service method to retrieve bus amenities
        //        var amenityDtos = await _amenityService.GetBusAmenitiesAsync(busId);

        //        // Return the list of amenity DTOs
        //        return Ok(amenityDtos);
        //    }
        //    catch (BusNotFoundException ex)
        //    {
        //        return NotFound(ex.Message);
        //    }
        //    catch (AmenitiesNotFoundException ex)
        //    {
        //        return NotFound(ex.Message);
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log the exception for debugging purposes
        //        _logger.LogError(ex, "An error occurred while processing GetBusAmenities.");
        //        return StatusCode(500, "An error occurred while processing your request.");
        //    }
        // }




    }

}