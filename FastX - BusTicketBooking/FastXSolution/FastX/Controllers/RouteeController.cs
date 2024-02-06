using FastX.Interfaces;
using FastX.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Numerics;

namespace FastX.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RouteeController : ControllerBase
    {
        private readonly IRouteeService _adminService;


        public RouteeController(IRouteeService adminService)
        {
            _adminService = adminService;
        }
        [HttpPost]
        public async Task<Routee> Post(Routee Routee)
        {
            var addedRoutee = await _adminService.AddRoutee(Routee);
            return addedRoutee;
        }

    }
}
