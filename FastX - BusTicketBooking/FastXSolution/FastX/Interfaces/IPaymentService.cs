using FastX.Models;

namespace FastX.Interfaces
{
    public interface IPaymentService
    {
        public Task<Payment>AddPayment(Payment payment);
        public Task<List<Payment>> GetPaymentList();
        public Task<Payment> GetPaymentBy(int id);

    }
}
