using FastX.Interfaces;
using FastX.Models;
using FastX.Models.DTOs;
using FastX.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FastX.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("ReactPolicy")]

    public class RefundController : ControllerBase
    {
        private readonly IBusOperatorService _busOperatorService;
        private readonly ILogger<RefundController> _logger;


        public RefundController(ILogger<RefundController> logger, IBusOperatorService busOperatorService)  
        {
            _busOperatorService = busOperatorService;
            _logger = logger;
        }

        //[HttpGet("RefundRequest")]
        //public async Task<ActionResult<RefundDTO>> RefundRequest([FromQuery]int userId) 
        //{
        //    var refund =await _busOperatorService.RefundRequest(userId);
        //    return Ok(refund);


        //}

        [HttpGet("RefundRequest")]
        public async Task<ActionResult<List<RefundDTO>>> RefundRequest()
        {
            var refund = await _busOperatorService.GetRefundDetailsForCancelledBookings();
            return Ok(refund);
        }
    }
}
