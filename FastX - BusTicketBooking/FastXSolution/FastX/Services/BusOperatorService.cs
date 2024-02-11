using FastX.Exceptions;
using FastX.Interfaces;
using FastX.Models;
using FastX.Models.DTOs;
using FastX.Repositories;
using Microsoft.Extensions.Logging;

namespace FastX.Services
{
    public class BusOperatorService : IBusOperatorService
    {
        private readonly IRepository<int, Bus> _busRepository;
        private readonly ILogger<BusService> _logger;

        public BusOperatorService(IRepository<int, Bus> busRepository, ILogger<BusService> logger)
        {
            _busRepository = busRepository;
            _logger = logger;
        }

        public async Task<List<BusDTOForOperator>> GetAllBuses(int busOperatorId)
        {
            try
            {
                var allBuses = await _busRepository.GetAsync();
                var buses= allBuses.Where(b => b.BusOperatorId == busOperatorId);
                //var buses = await _busRepository.GetAsync(b => b.BusOperatorId == busOperatorId);

                if (buses == null || !buses.Any())
                {
                    throw new BusOperatorNotFoundException();
                }

                return buses.Select(b => new BusDTOForOperator
                {
                    BusId = b.BusId,
                    BusName = b.BusName,
                    BusOperatorId = b.BusOperatorId,
                    BusType = b.BusType
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving buses for bus operator ID: {BusOperatorId}", busOperatorId);
                throw;
            }
        }

    }
}
