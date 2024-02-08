using FastX.Interfaces;
using FastX.Models;

namespace FastX.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IRepository<int, Payment> _repo;

        public PaymentService(IRepository<int, Payment> repo)
        {
            _repo = repo;

        }
        public async Task<Payment> AddPayment(Payment Payment)
        {
            Payment = await _repo.Add(Payment);
            return Payment;
        }

        public async Task<Payment> GetPaymentBy(int id)
        {
            var Payment = await _repo.GetAsync(id);
            return Payment;

        }

        public async Task<List<Payment>> GetPaymentList()
        {
            var Payment = await _repo.GetAsync();
            return Payment;
        }
    }
}
