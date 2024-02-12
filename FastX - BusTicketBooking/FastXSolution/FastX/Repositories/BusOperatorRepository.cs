using FastX.Contexts;
using FastX.Exceptions;
using FastX.Interfaces;
using FastX.Models;
using Microsoft.EntityFrameworkCore;

namespace FastX.Repositories
{
    public class BusOperatorRepository : IRepository<int, BusOperator>
    {
        private readonly FastXContext _context;

        public BusOperatorRepository(FastXContext context)
        {
            _context = context;
        }
        public async Task<BusOperator> Add(BusOperator item)
        {
            _context.Add(item);
            _context.SaveChanges();
            return item;
        }



        public Task<BusOperator> Delete(int key)
        {
            throw new NotImplementedException();
        }

        public async Task<List<BusOperator>> GetAsync()
        {
            var busOperator = await _context.BusOperators.ToListAsync();
            if (busOperator != null)
            {
                return busOperator;
            }
            throw new BusOperatorNotFoundException();
        }

        public async Task<BusOperator> GetAsync(int key)
        {
            var busOperators = await GetAsync();
            var busOperator = busOperators.SingleOrDefault(u => u.BusOperatorId == key);
            if (busOperator != null)
            {
                return busOperator;
            }
            throw new BusOperatorNotFoundException();
        }

        public Task<BusOperator> Update(BusOperator item)
        {
            throw new NotImplementedException();
        }
    }
}