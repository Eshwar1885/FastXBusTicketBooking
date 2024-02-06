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
    }

}
