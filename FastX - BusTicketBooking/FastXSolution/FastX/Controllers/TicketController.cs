using FastX.Interfaces;
using FastX.Models.DTOs;
using FastX.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FastX.Exceptions;

namespace FastX.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly ITicketService _ticketService;
        private readonly ILogger<TicketController> _logger;

        public TicketController(ITicketService ticketService, ILogger<TicketController> logger)
        {
            _ticketService = ticketService; ;
            _logger = logger;
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetTicketsForUser(int userId)
        {
            try
            {
                var tickets = await _ticketService.GetTicketsForUser(userId);
                return Ok(tickets);
            }
            catch (NoTicketsAvailableException)
            {
                return NotFound("No tickets found for the specified user.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while processing the request for user with ID: {userId}");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        // Add other controller actions for updating, deleting, and listing tickets
    }
}