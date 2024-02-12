//using FastX.Exceptions;
//using FastX.Interfaces;
//using FastX.Models;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;

//namespace FastX.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class TestController : ControllerBase
//    {


//private readonly IPaymentService _paymentService;
//        private readonly IBusOperatorService _busOperatorService;
//        private readonly IAmenityService _amenityService;
//        private readonly ILogger<TestController> _logger;
//private readonly IRouteeService _routeService;


//        public TestController(IBusOperatorService busOperatorService,
//IPaymentService paymentService,
//IRouteeService routeService
//        IAmenityService amenityService, ILogger<TestController> logger)
//        {
//            _busOperatorService = busOperatorService;
//            _amenityService = amenityService;
//_paymentService = paymentService;
//_routeService = routeService;
//            _logger = logger;
//        }


//---------------------Amenity CRUD

//        ////This method will add Amenity directly to the Amenity table
//        [HttpPost("AddAmenitiesDirectlyToAmenitiesTable")]
//        public async Task<IActionResult> AddAmenity(Amenity amenity)
//        {
//            try
//            {
//                var addedAmenity = await _amenityService.AddAmenity(amenity);
//                return Ok(addedAmenity);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "An error occurred while adding amenity.");
//                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
//            }
//        }


//       /// <summary>
//       /// /This method will delete Amenity directly from the amenity table
//       /// </summary>
//       /// <param name="id"></param>
//       /// <returns></returns>

//        [Authorize(Roles = "busoperator")]
//        [HttpDelete("amenities/{id}")]
//        public async Task<IActionResult> DeleteAmenity(int id)
//        {
//            try
//            {
//                var deletedAmenity = await _amenityService.DeleteAmenity(id);
//                return Ok(deletedAmenity);
//            }
//            catch (AmenitiesNotFoundException ex)
//            {
//                _logger.LogWarning(ex.Message);
//                return NotFound(ex.Message);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "An error occurred while deleting amenity with ID: {AmenityId}", id);
//                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
//            }
//        }

//        [Authorize(Roles = "busoperator")]
//        [HttpPut("amenities")]
//        public async Task<IActionResult> UpdateAmenity(int id, string name)
//        {
//            try
//            {
//                var updatedAmenity = await _amenityService.ChangeAmenityNameAsync(id, name);
//                return Ok(updatedAmenity);
//            }
//            catch (AmenitiesNotFoundException ex)
//            {
//                _logger.LogWarning(ex.Message);
//                return NotFound(ex.Message);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "An error occurred while updating amenity.");
//                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
//            }
//        }

//----------------------------------------------------

//----------------Route CRUD-------------------
//[HttpPost]
//public async Task<Routee> Post(Routee Routee)
//{
//    var addedRoutee = await _routeService.AddRoutee(Routee);
//    return addedRoutee;
//}



//[HttpGet]
//public async Task<List<Routee>> GetAll()
//{
//    var routee = await _routeService.GetRouteeList();
//    return routee;
//}

//[Route("/GetRouteById")]
//[HttpGet]
//public async Task<Routee> GetById(int id)
//{
//    var routee = await _routeService.GetRoutee(id);
//    return routee;
//}

//[HttpDelete]
//public async Task<Routee> Delete(int id)
//{
//    var routee = await _routeService.DeleteRoutee(id);
//    return routee;
//}



//--------------------------------------------------


//----------------Payment CRUD---------------------

//[HttpPost]
//public async Task<Payment>Post(Payment payment) 
//{
//    var addedPayment = await _paymentService.AddPayment(payment);
//    return addedPayment;
//}
//[HttpGet]
//public async Task<List<Payment>>GetAll()
//{
//    var Payment = await _paymentService.GetPaymentList();
//    return Payment;
//}
//[Route("/GetPaymentById")]
//[HttpGet]
//public async Task<Payment> GetById(int id)
//{
//    var payment = await _paymentService.GetPaymentBy(id);
//    return payment;
//}

////[HttpDelete]
////public async Task<Payment> Delete(int id)
////{
////    var payment = await _paymentService.DeletePayment(id);
////    return payment;
////}
///

//-----------------------

//    }
//}