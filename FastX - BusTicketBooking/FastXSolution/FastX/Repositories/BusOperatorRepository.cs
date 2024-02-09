using FastX.Contexts;
using FastX.Interfaces;
using FastX.Models;

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

        public Task<List<BusOperator>> GetAsync()
        {
            throw new NotImplementedException();
        }

        public Task<BusOperator> GetAsync(int key)
        {
            throw new NotImplementedException();
        }

        public Task<BusOperator> Update(BusOperator item)
        {
            throw new NotImplementedException();
        }
    }
}
