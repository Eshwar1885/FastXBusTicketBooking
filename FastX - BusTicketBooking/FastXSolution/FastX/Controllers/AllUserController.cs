using FastX.Exceptions;
using FastX.Interfaces;
using FastX.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FastX.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AllUserController : ControllerBase
    {
        private readonly IAllUserService _allUserService;
        private readonly ILogger<AllUserController> _logger;

        public AllUserController(IAllUserService allUserService, ILogger<AllUserController> logger)
        {
            _allUserService = allUserService;
            _logger = logger;
        }
        [Route("Register")]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterUserDTO user)
        {
            //--------------------------
            try
            {
                var result = await _allUserService.Register(user);
                return Ok(result);
            }
            catch (UserAlreadyExistsException due)
            {
                _logger.LogError(due, "user already exists");
                return Conflict("Username already exists");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while registering");
                return StatusCode(500, "An error occurred while registering user");
            }
            //---------------------------

            //var result = await _allUserService.Register(user);     ////prev code
            //return result;                                                //// prev code
        }

        [Route("Login")]
        [HttpPost]
        public async Task<ActionResult<LoginUserDTO>> Login(LoginUserDTO user)
        {
            try
            {
                var result = await _allUserService.Login(user);
                return Ok(result);
            }
            catch (InvlidUserException iuse)
            {
                _logger.LogCritical(iuse.Message);
                return Unauthorized("Invalid username or password");
            }

        }
    }
}
