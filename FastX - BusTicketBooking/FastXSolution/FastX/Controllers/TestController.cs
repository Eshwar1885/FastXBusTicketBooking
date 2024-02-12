using FastX.Exceptions;
using FastX.Interfaces;
using FastX.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FastX.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IBusOperatorService _busOperatorService;
        private readonly IAmenityService _amenityService;
        private readonly ILogger<TestController> _logger;

        public TestController(IBusOperatorService busOperatorService, IAmenityService amenityService, ILogger<TestController> logger)
        {
            _busOperatorService = busOperatorService;
            _amenityService = amenityService;
            _logger = logger;
        }

        ////This method will add Amenity directly to the Amenity table

        //[HttpPost("AddAmenitiesDirectlyToAmenitiesTable")]
        //public async Task<IActionResult> AddAmenity(Amenity amenity)
        //{
        //    try
        //    {
        //        var addedAmenity = await _amenityService.AddAmenity(amenity);
        //        return Ok(addedAmenity);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "An error occurred while adding amenity.");
        //        return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
        //    }
        //}


        ////This method will delete Amenity directly from the amenity table

        //[Authorize(Roles = "busoperator")]
        //[HttpDelete("amenities/{id}")]
        //public async Task<IActionResult> DeleteAmenity(int id)
        //{
        //    try
        //    {
        //        var deletedAmenity = await _amenityService.DeleteAmenity(id);
        //        return Ok(deletedAmenity);
        //    }
        //    catch (AmenitiesNotFoundException ex)
        //    {
        //        _logger.LogWarning(ex.Message);
        //        return NotFound(ex.Message);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "An error occurred while deleting amenity with ID: {AmenityId}", id);
        //        return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
        //    }
        //}

        //[Authorize(Roles = "busoperator")]
        //[HttpPut("amenities")]
        //public async Task<IActionResult> UpdateAmenity(int id, string name)
        //{
        //    try
        //    {
        //        var updatedAmenity = await _amenityService.ChangeAmenityNameAsync(id, name);
        //        return Ok(updatedAmenity);
        //    }
        //    catch (AmenitiesNotFoundException ex)
        //    {
        //        _logger.LogWarning(ex.Message);
        //        return NotFound(ex.Message);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "An error occurred while updating amenity.");
        //        return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
        //    }
        //}


    }
}