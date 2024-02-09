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
        public async Task<LoginUserDTO> Register(RegisterUserDTO user)
        {
            var result = await _allUserService.Register(user);
            return result;
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
