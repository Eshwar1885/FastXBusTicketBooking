using FastX.Interfaces;
using FastX.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FastX.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : Controller
    {
        private readonly IPaymentService _adminService;

        public PaymentController(IPaymentService adminService)
        {
            _adminService = adminService;
        }
        //[HttpPost]
        //public async Task<Payment>Post(Payment payment) 
        //{
        //    var addedPayment = await _adminService.AddPayment(payment);
        //    return addedPayment;
        //}
        //[HttpGet]
        //public async Task<List<Payment>>GetAll()
        //{
        //    var Payment = await _adminService.GetPaymentList();
        //    return Payment;
        //}
        //[Route("/GetPaymentById")]
        //[HttpGet]
        //public async Task<Payment> GetById(int id)
        //{
        //    var Payment = await _adminService.GetPaymentBy(id);
        //    return Payment;
        //}

        ////[HttpDelete]
        ////public async Task<Payment> Delete(int id)
        ////{
        ////    var Payment = await _adminService.DeletePayment(id);
        ////    return Payment;
        ////}


    }
}
