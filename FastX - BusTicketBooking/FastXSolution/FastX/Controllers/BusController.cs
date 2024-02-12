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
    public class BusController : Controller
    {
        private readonly IBusService _busService;
        private readonly ILogger<BusController> _logger;

        public BusController(IBusService busService, ILogger<BusController> logger)
        {
            _busService = busService;
            _logger = logger;
        }

        //[Authorize(Roles = "busoperator")]
        //[HttpPost]
        //public async Task<Bus> Post(Bus Bus)
        //{
        //    var addedBus = await _adminService.AddBus(Bus);
        //    return addedBus;
        //}

        //[HttpGet]
        //public async Task<List<Bus>> GetAll()
        //    var Bus = await _busService.GetBusList();
        //   return Bus;
        //}

        //[Route("/GetBusById")]
        //[HttpGet]
        //public async Task<Bus> GetById(int id)
        //{
        //    var Bus = await _adminService.GetBus(id);
        //    return Bus;
        //}


        //[Authorize(Roles = "admin")]
        //[Authorize(Roles = "busoperator")]
        //[HttpDelete]
        //public async Task<Bus> Delete(int id)
        //{
        //    var Bus = await _adminService.DeleteBus(id);
        //    return Bus;
        //}


        //-------------------------------

        [Authorize(Roles = "busoperator")]
        [HttpPost("AddBusByBusOperator")]
        public async Task<IActionResult> AddBus(string busName, string busType, int totalSeats, int busOperatorId)
        {
            try
            {
                var addedBus = await _busService.AddBus(busName, busType, totalSeats, busOperatorId);
                return Ok(addedBus);
            }
            catch (BusOperatorNotFoundException ex)
            {
                _logger.LogWarning(ex.Message);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding bus");
                return StatusCode(500, "Internal server error");
            }
        }

        //--------------------------------

        [Authorize(Roles = "busoperator")]
        [HttpDelete("DeleteBusByBusOperator")]
        public async Task<IActionResult> DeleteBus(int busId)
        {
            try
            {
                var result = await _busService.DeleteBus(busId);
                if (result != null)
                    return Ok("Bus deleted successfully");
                else
                    return NotFound("Bus not found");
            }
            catch (BusNotFoundException ex)
            {
                _logger.LogWarning(ex.Message);
                return NotFound(ex.Message);
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting bus");
                return StatusCode(500, "Internal server error");
            }
        }





    }
}
