using FastX.Exceptions;
using FastX.Interfaces;
using FastX.Models;
using FastX.Repositories;
using System.Diagnostics.CodeAnalysis;

namespace FastX.Services
{
    public class PaymentService : IPaymentService
    {

        private readonly ILogger<PaymentService> _logger;
        private readonly IBookingRepository<int, Booking> _bookingRepository;
        private readonly IRepository<int, Payment> _paymentRepository;
        //private readonly IRepository<int, Seat> _seatRepository;
        private readonly ISeatService _seatService;

        public PaymentService(

            IBookingRepository<int, Booking> bookingRepository,
            IRepository<int, Payment> paymentRepository,
            //IRepository<int, Seat> seatRepository,
            ISeatService seatService,
            ILogger<PaymentService> logger)
        {

            _logger = logger;
            _bookingRepository = bookingRepository;
            _paymentRepository = paymentRepository;
            _seatService = seatService;


        }
        public async Task<Payment> AddPayment(Payment payment)
        {
            payment = await _paymentRepository.Add(payment);
            return payment;
        }

        public async Task<Payment> GetPaymentBy(int id)
        {
            var payment = await _paymentRepository.GetAsync(id);
            return payment;

        }

        public async Task<List<Payment>> GetPaymentList()
        {
            var payment = await _paymentRepository.GetAsync();
            return payment;
        }

        [ExcludeFromCodeCoverage]
        public async Task CreatePayment(int bookingId)
        {
            try
            {
                _logger.LogInformation($"Attempting to create payment for BookingId: {bookingId}");

                var booking = await _bookingRepository.GetAsync(bookingId);

                if (booking != null)
                {
                    // Calculate total price based on seat prices and number of seats
                    float totalPrice = await CalculateTotalPriceAsync(booking);

                    var payment = new Payment
                    {
                        BookingId = bookingId,
                        Amount = totalPrice,
                        PaymentStatus = "Complete",
                        PaymentDate = DateTime.Now
                    };

                    await _paymentRepository.Add(payment);



                    _logger.LogInformation($"Payment created successfully. BookingId: {bookingId}, Amount: {totalPrice}");

                    booking.Status = "Complete";
                    await _bookingRepository.Update(booking);
                    _logger.LogInformation($"Updating booking status to Complete. BookingId: {bookingId}");

                }
                else
                {
                    _logger.LogError($"Booking not found. BookingId: {bookingId}");
                    throw new BookingNotFoundException($"Booking with Id {bookingId} not found.");
                }
            }
            catch (BookingNotFoundException ex)
            {
                _logger.LogError($"Booking not found. Error: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An unexpected error occurred. Error: {ex.Message}");
                throw new Exception("Internal Server Error");
            }
        }

        [ExcludeFromCodeCoverage]
        private async Task<float> CalculateTotalPriceAsync(Booking booking)
        {
            try
            {
                float totalPrice = 0;

                foreach (var ticket in booking.Tickets)
                {
                    float seatPrice = await _seatService.GetSeatPriceAsync(ticket.SeatId, ticket.BusId);
                    totalPrice += seatPrice;
                }

                return totalPrice;
            }
            catch (NoSeatsAvailableException ex)
            {
                _logger.LogError($"Seat not found. Error: {ex.Message}");
                throw; // Re-throw the exception for the controller to handle
            }
            catch (Exception ex)
            {
                _logger.LogError($"An unexpected error occurred. Error: {ex.Message}");
                throw new Exception("Internal Server Error");
            }
        }

        //public async Task<float> FindRefundPrice(int userId, int bookingId)
        //{
        //    var payments = await GetPaymentList();
        //    var filteredPayments = payments.Where(payment => payment.Booking.UserId == userId &&
        //    payment.BookingId == bookingId).ToList();
        //    return filteredPayments[0].Amount;
        //}
        public async Task<float> FindRefundPrice(int bookingId)
        {
            var payments = await GetPaymentList();
            var filteredPayments = payments.Where(payment => payment.BookingId == bookingId).ToList();
            return filteredPayments[0].Amount;
        }


    }
}