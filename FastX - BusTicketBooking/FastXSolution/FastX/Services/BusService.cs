using FastX.Exceptions;
using FastX.Interfaces;
using FastX.Models;
using FastX.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FastX.Services
{
    public class BusService : IBusService
    {
        private IRepository<int, Bus> _busRepository;
        private readonly ILogger<BusService> _logger;
        private IRepository<int, BusOperator> _busOperatorRepository;

        //private readonly ILogger<RouteeService> _logger;
        public BusService(IRepository<int, Bus> busRepository, IRepository<int, BusOperator> busOperatorRepository, ILogger<BusService> logger)
        {
            _busRepository = busRepository;
            _busOperatorRepository = busOperatorRepository;
            _logger = logger;

        }


        //public async Task<Bus> AddBus(Bus bus)
        //{
        //    bus = await _repo.Add(bus);
        //    return bus;
        //}


        //public async Task<Bus> DeleteBus(int id)
        //{
        //    var bus = await GetBus(id);
        //    if (bus != null)
        //    {
        //        bus = await _repo.Delete(id);
        //        return bus;
        //    }
        //    throw new NoSuchBusException();

        //}

        public async Task<Bus> GetBus(int id)
        {
            var bus = await _busRepository.GetAsync(id);
            return bus;

        }
        public async Task<List<Bus>> GetBusList()
        {
            var bus = await _busRepository.GetAsync();
            if (bus == null)
            {
                throw new NoSuchBusException();
            }
            return bus;
        }

        //---------------------------
        public async Task<Bus> AddBus(string busName, string busType, int totalSeats, int busOperatorId)
        {
            try
            {
                // Check if the bus operator exists
                var busOperator = await _busOperatorRepository.GetAsync(busOperatorId);
                if (busOperator == null)
                {
                    throw new BusOperatorNotFoundException();
                }
                var bus = new Bus
                {
                    BusName = busName,
                    BusType = busType,
                    TotalSeats = totalSeats,
                    BusOperatorId = busOperatorId
                };

                return await _busRepository.Add(bus);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding bus");
                throw; // Re-throw the exception for the caller to handle
            }
        }

        public async Task<Bus> DeleteBus(int busId)
        {
            try
            {
                return await _busRepository.Delete(busId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting bus");
                throw; // Re-throw the exception for the caller to handle
            }
        }
    }
}
