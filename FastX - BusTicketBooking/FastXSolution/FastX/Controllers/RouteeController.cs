using FastX.Exceptions;
using FastX.Interfaces;
using FastX.Models;
using FastX.Models.DTOs;
using FastX.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Numerics;

namespace FastX.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("ReactPolicy")]
    public class RouteeController : ControllerBase
    {
        private readonly IRouteeService _routeService;
        private readonly ILogger<RouteeController> _logger;

        public RouteeController( IRouteeService routeeService, ILogger<RouteeController> logger)
        {
            _routeService = routeeService;
            _logger = logger;
        }

        [HttpPost("AddRouteForBusByBusOperator")]
        public async Task<IActionResult> AddRoutee([FromBody] AddRouteDTO routee)
        {
            try
            {
                await _routeService.AddRouteeToBus(routee.BusId, routee.Origin, routee.Destination, routee.TravelDate);
                return Ok("BusRoute added successfully");
            }
            catch (BusNotFoundException ex)
            {
                _logger.LogError(ex, "Bus not found");
                return NotFound(ex.Message);
            }
            //    catch (AmenityAlreadyExistsException ex)
            //    {
            //        _logger.LogError(ex, "Amenity already exists for this bus");
            //        return Conflict(ex.Message);
            //    }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding amenity to bus");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}