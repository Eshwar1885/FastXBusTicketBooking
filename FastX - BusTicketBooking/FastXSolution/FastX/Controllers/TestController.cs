using FastX.Exceptions;
using FastX.Interfaces;
using FastX.Models;
using FastX.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FastX.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("ReactPolicy")]
    public class TestController : ControllerBase
    {


        private readonly IPaymentService _paymentService;
        private readonly IBusOperatorService _busOperatorService;
        private readonly IAmenityService _amenityService;
        private readonly ILogger<TestController> _logger;
        private readonly IRouteeService _routeService;
        private readonly ITicketService _ticketService;


        public TestController(IBusOperatorService busOperatorService,
            IPaymentService paymentService,
            IRouteeService routeService,
        IAmenityService amenityService, ILogger<TestController> logger, ITicketService ticketService)
        {
            _busOperatorService = busOperatorService;
            _amenityService = amenityService;
            _paymentService = paymentService;
            _routeService = routeService;
            _logger = logger;
            _ticketService = ticketService;
        }


        //---------------------Amenity CRUD

        ////This method will add Amenity directly to the Amenity table
        [Route("AddAmenitiesDirectlyToAmenitiesTable")]
        [HttpPost]
        public async Task<IActionResult> AddAmenity(Amenity amenity)
        {
            try
            {
                var addedAmenity = await _amenityService.AddAmenity(amenity);
                return Ok(addedAmenity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding amenity.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }



        [Route("DeleteAmenitiesDirectlyToAmenitiesTable")]
        //[Authorize(Roles = "busoperator")]
        [HttpDelete]
        public async Task<IActionResult> DeleteAmenity(int id)
        {
            try
            {
                var deletedAmenity = await _amenityService.DeleteAmenity(id);
                return Ok(deletedAmenity);
            }
            catch (AmenitiesNotFoundException ex)
            {
                _logger.LogWarning(ex.Message);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting amenity with ID: {AmenityId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }
        [Route("UpdateAmenityNameDirectlyToAmenitiesTable")]
        //[Authorize(Roles = "busoperator")]
        [HttpPut]
        public async Task<IActionResult> UpdateAmenity(int id, string name)
        {
            try
            {
                var updatedAmenity = await _amenityService.ChangeAmenityNameAsync(id, name);
                return Ok(updatedAmenity);
            }
            catch (AmenitiesNotFoundException ex)
            {
                _logger.LogWarning(ex.Message);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating amenity.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }

        //----------------------------------------------------

        //----------------Route CRUD-------------------

        [Route("AddRoutesDirectlyToRouteTable")]
        [HttpPost]
        public async Task<Routee> Post(Routee Routee)
        {
            var addedRoutee = await _routeService.AddRoutee(Routee);
            return addedRoutee;
        }


        //[Route("GetRoutesDirectlyFromRouteTable")]
        //[HttpGet]
        //public async Task<List<Routee>> GetAll()
        //{
        //    var routee = await _routeService.GetRouteeList();
        //    return routee;
        //}
        //[Route("GetRoutesByIdDirectlyFromRouteTable")]

        //[HttpGet]
        //public async Task<Routee> GetById(int id)
        //{
        //    var routee = await _routeService.GetRoutee(id);
        //    return routee;
        //}
        //[Route("DeleteRoutesDirectlyFromRouteTable")]
        //[HttpDelete]
        //public async Task<Routee> Delete(int id)
        //{
        //    var routee = await _routeService.DeleteRoutee(id);
        //    return routee;
        //}



        //--------------------------------------------------


        //----------------Payment CRUD---------------------

        [Route("AddPaymentDirectlyToPaymentTable")]
        [HttpPost]
        public async Task<Payment> Post(Payment payment)
        {
            var addedPayment = await _paymentService.AddPayment(payment);
            return addedPayment;
        }
        [Route("GetPaymentDirectlyFromPaymentTable")]
        [HttpGet]
        public async Task<List<Payment>> GetPayment()
        {
            var Payment = await _paymentService.GetPaymentList();
            return Payment;
        }
        [Route("/GetPaymentByIdFromPaymentTable")]
        [HttpGet]
        public async Task<Payment> GetPaymentById(int id)
        {
            var payment = await _paymentService.GetPaymentBy(id);
            return payment;
        }
        //[Route("DeletePaymentDirectlyToPaymentTable")]
        //[HttpDelete]
        //public async Task<Payment> DeletePayment(int id)
        //{
        //    var payment = await _paymentService.DeletePayment(id);
        //    return payment;
        //}


        //---------
        //ticket crud --------------

        [Route("AddTicketDirectlyToTicketTable")]
        [HttpPost]
        public async Task<Ticket> PostTicket(Ticket ticket)
        {
            var addedTicket = await _ticketService.AddTicket(ticket);
            return addedTicket;
        }
        [Route("GetTicketDirectlyFromTicketTable")]
        [HttpGet]
        public async Task<List<Ticket>> GetTicket()
        {
            var ticket = await _ticketService.GetTicketList();
            return ticket;
        }
        [Route("/GetTicketByIdFromTicketTable")]
        [HttpGet]
        public async Task<Ticket> GetTicketById(int id)
        {
            var ticket = await _ticketService.GetTicket(id);
            return ticket;
        }
        [Route("DeleteTicketDirectlyFromTicketTable")]
        [HttpDelete]
        public async Task<Ticket> DeleteTicket(int id)
        {
            var ticket = await _ticketService.DeleteTicket(id);
            return ticket;
        }
        //-----------------------
    }
}