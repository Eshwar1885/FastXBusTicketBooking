using FastX.Models;

namespace FastX.Interfaces
{
    public interface IPaymentService
    {
        public Task<Payment> AddPayment(Payment payment);
        public Task<List<Payment>> GetPaymentList();
        public Task<Payment> GetPaymentBy(int id);
        public Task CreatePayment(int bookingId);
        //public Task<float> FindRefundPrice(int userId, int bookingId);

        public Task<float> FindRefundPrice(int bookingId);
        public Task<float> CalculateTotalPriceAsync(Booking booking);





    }
}