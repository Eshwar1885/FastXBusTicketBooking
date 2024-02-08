using FastX.Exceptions;
using FastX.Interfaces;
using FastX.Models;
using System.Numerics;

namespace FastX.Services
{
    public class RouteeService : IRouteeService
    {
        private IRepository<int, Routee> _repo;
        //private readonly ILogger<RouteeService> _logger;

        public RouteeService(IRepository<int, Routee> repo)
        {
            _repo = repo;

        }
        public async Task<Routee> AddRoutee(Routee routee)
        {
            routee = await _repo.Add(routee);
            return routee;
        }

        public async Task<Routee> DeleteRoutee(int id)
        {
            var routee = await GetRoutee(id);
            if (routee != null)
            {
                routee = await _repo.Delete(id);
                return routee;
            }
            throw new NoSuchRouteeException();
        }

        public async Task<Routee> GetRoutee(int id)
        {
            var routee = await _repo.GetAsync(id);
            return routee;
        }

        public async Task<List<Routee>> GetRouteeList()
        {
            var routee = await _repo.GetAsync();
            return routee;
        }
    }

}
