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
        public async Task<Payment> AddPayment(Payment payment)
        {
            payment = await _repo.Add(payment);
            return payment;
        }

        public async Task<Payment> GetPaymentBy(int id)
        {
            var payment = await _repo.GetAsync(id);
            return payment;

        }

        public async Task<List<Payment>> GetPaymentList()
        {
            var payment = await _repo.GetAsync();
            return payment;
        }
    }
}