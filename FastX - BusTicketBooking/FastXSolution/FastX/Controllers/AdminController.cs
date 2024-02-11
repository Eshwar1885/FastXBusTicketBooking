//using FastX.Exceptions;
//using FastX.Interfaces;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;

//namespace FastX.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class AdminController : ControllerBase
//    {
//        private readonly IBusOperatorService _busOperatorService;
//        private readonly IAdminService _adminService;
//        private readonly IBusOperatorService _userService;
//        private readonly IBusOperatorService _allUserService;


//        private readonly ILogger<BusOperatorController> _logger;

//        public AdminController(IBusOperatorService busOperatorService, ILogger<BusOperatorController> logger)
//        {
//            _busOperatorService = busOperatorService;
//            _logger = logger;
//        }
//        public async Task<IActionResult> DeleteAmenity(int busId, string amenityName)
//        {
//            try
//            {
//                await _amenityService.DeleteAmenityFromBus(busId, amenityName);
//                return Ok("Amenity deleted successfully");
//            }
//            catch (NoSuchBusException ex)
//            {
//                _logger.LogError(ex, "Bus not found");
//                return NotFound(ex.Message);
//            }
//            catch (AmenitiesNotFoundException ex)
//            {
//                _logger.LogError(ex, "Amenity not found");
//                return NotFound(ex.Message);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "An error occurred while deleting amenity from bus");
//                return StatusCode(500, "Internal server error");
//            }
//        }
//    }

//}
