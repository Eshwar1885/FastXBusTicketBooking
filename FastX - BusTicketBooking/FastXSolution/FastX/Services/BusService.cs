using FastX.Exceptions;
using FastX.Interfaces;
using FastX.Models;

namespace FastX.Services
{
    public class BusService : IBusService
    {
        private IRepository<int, Bus> _repo;
        //private readonly ILogger<RouteeService> _logger;

        public BusService(IRepository<int, Bus> repo)
        {
            _repo = repo;

        }
        public async Task<Bus> AddBus(Bus bus)
        {
            bus = await _repo.Add(bus);
            return bus;
        }
        public async Task<Bus> DeleteBus(int id)
        {
            var bus = await GetBus(id);
            if (bus != null)
            {
                bus = await _repo.Delete(id);
                return bus;
            }
            throw new NoSuchBusException();

        }

        public async Task<Bus> GetBus(int id)
        {
            var bus = await _repo.GetAsync(id);
            return bus;

        }
        public async Task<List<Bus>> GetBusList()
        {
            var bus = await _repo.GetAsync();
            if (bus == null)
            {
                throw new NoSuchBusException();
            }
            return bus;
        }


    }
}
