using FastX.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FastX.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AmenityController : ControllerBase
    {
        private readonly IAmenityService _amenityService;
        private readonly ILogger<AmenityController> _logger;

        public AmenityController(IAmenityService amenityService, ILogger<AmenityController> logger)
        {
            _amenityService = amenityService;
            _logger = logger;
        }
        [HttpGet]
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
    }

}
