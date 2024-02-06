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
        public async Task<Bus> AddBus(Bus Bus)
        {
            Bus = await _repo.Add(Bus);
            return Bus;
        }
        public async Task<Bus> DeleteBus(int id)
        {
            var Bus = await GetBus(id);
            if (Bus != null)
            {
                Bus = await _repo.Delete(id);
                return Bus;
            }
            throw new NoSuchBusException();

        }

        public async Task<Bus> GetBus(int id)
        {
            var Bus = await _repo.GetAsync(id);
            return Bus;

        }
        public async Task<List<Bus>> GetBusList()
        {
            var Bus = await _repo.GetAsync();
            return Bus;
        }


    }
}
