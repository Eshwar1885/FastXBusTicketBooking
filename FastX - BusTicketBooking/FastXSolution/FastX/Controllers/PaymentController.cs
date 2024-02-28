using FastX.Exceptions;
using FastX.Interfaces;
using FastX.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FastX.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("ReactPolicy")]
    public class PaymentController : Controller
    {
        private readonly IPaymentService _paymentService;
        private readonly ILogger<PaymentController> _logger;

        public PaymentController(
            IPaymentService paymentService,
            ILogger<PaymentController> logger)
        {
            _paymentService = paymentService;
            _logger = logger;
        }

        //[HttpPost("create-payment/{bookingId}")]
        //public async Task<IActionResult> CreatePayment(int bookingId)
        //{
        //    try
        //    {
        //        await _paymentService.CreatePayment(bookingId);

        //        return Ok("Payment created successfully");
        //    }
        //    catch (BookingNotFoundException ex)
        //    {
        //        _logger.LogError($"Booking not found. Error: {ex.Message}");
        //        return NotFound("Booking not found");
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError($"An unexpected error occurred. Error: {ex.Message}");
        //        return StatusCode(500, "Internal Server Error");
        //    }
        //}



        [HttpPost("create-payment")]
        public async Task<IActionResult> CreatePayment(Payment payment)
        {
            try
            {
                await _paymentService.CreatePayment(payment.BookingId);

                return Ok("Payment created successfully");
            }
            catch (BookingNotFoundException ex)
            {
                _logger.LogError($"Booking not found. Error: {ex.Message}");
                return NotFound("Booking not found");
            }
            catch (Exception ex)
            {
                _logger.LogError($"An unexpected error occurred. Error: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }





    }
}