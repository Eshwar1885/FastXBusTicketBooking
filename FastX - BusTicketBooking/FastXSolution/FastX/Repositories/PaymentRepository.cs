using FastX.Contexts;
using FastX.Exceptions;
using FastX.Interfaces;
using FastX.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace FastX.Repositories
{
    public class PaymentRepository : IRepository<int,Payment>
    {
        private readonly FastXContext _context;

        public PaymentRepository(FastXContext context)
        {
            _context=context;
        }

        public async Task<Payment> Add(Payment item)
        {
            _context.Add(item);
            _context.SaveChanges();
            return item;
        }

        public Task<Payment> Delete(int key)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Payment>> GetAsync()
        {
            var payments = _context.Payments.ToList();
            return payments;
        }

        public async Task<Payment> GetAsync(int key)
        {
            var payments = await GetAsync();
            var payment = payments.FirstOrDefault(e => e.PaymentId == key);
            if (payment != null)
                return payment;
            throw new NoPaymentsAvailableException();
        }

        public Task<Payment> Update(Payment item)
        {
            throw new NotImplementedException();
        }
    }
}

