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

//        public AdminController(IBusOperatorService busOperatorService, IAdminService adminService, ILogger<BusOperatorController> logger)
//        {
//            _busOperatorService = busOperatorService;
//            _logger = logger;
//            _adminService = adminService;
//        }
//        [HttpDelete("{username}")]
//        public async Task<IActionResult> DeleteUser(string username)
//        {
//            try
//            {
//                await _userService.DeleteUserAsync(username);
//                return Ok();
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, ex.Message);
//            }
//        }

//        [HttpDelete("{username}")]
//        public async Task<IActionResult> DeleteBusOperator(string username)
//        {
//            try
//            {
//                await _busOperatorService.DeleteBusOperatorAsync(username);
//                return Ok();
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, ex.Message);
//            }
//        }
//    }

//}
