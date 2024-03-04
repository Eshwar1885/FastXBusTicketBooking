using FastX.Exceptions;
using FastX.Interfaces;
using FastX.Models;
using FastX.Models.DTOs;
using FastX.Repositories;
using System;

namespace FastX.Services
{
    public class BusService : IBusService
    {
        //private readonly IBusRepository _busRepository;
        private readonly IRepository<int, Bus> _busRepository;
        private readonly ILogger<BusService> _logger;
        private IRepository<int, BusOperator> _busOperatorRepository;
        private ISeatRepository<int, Seat> _seatRepository;



        public BusService(

            IRepository<int, Bus> busRepository,
             IRepository<int, BusOperator> busOperatorRepository,
            ILogger<BusService> logger,
            ISeatRepository<int, Seat> seatRepository)
        {

            _busRepository = busRepository;
            _busOperatorRepository = busOperatorRepository;
            _logger = logger;
            _seatRepository = seatRepository;
        }

        //public async Task<List<BusDto>> SearchBusesAsync(string origin, string destination, DateTime date)
        //{
        //    try
        //    {
        //        var bus = await _busRepository.GetAsync(id);
        //        if (bus == null )
        //        {
        //            throw new BusNotFoundException();

        //        }
        //        return bus;
        //    }
        //    catch(Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //        // You can throw a custom exception or return a default value based on your requirements
        //        throw new BusNotFoundException();

        //    }

        //        var busDtos = buses.Select(bus => new BusDto
        //        {
        //            BusId = bus.BusId,
        //            BusName = bus.BusName,
        //            BusType = bus.BusType,
        //            TotalSeats = bus.TotalSeats,
        //            Origin = bus.Origin,

        //            Destination = bus.Destination,
        //        }).ToList();

        public async Task<Bus> AddBus(string busName, string busType, int totalSeats, int busOperatorId, int seatPrice)
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
                await _busRepository.Add(bus);

                //int seatNumber = 1;
                for (int i = 1; i <= totalSeats; i++)
                {
                    _logger.LogInformation($"Attempting to Add Seats");
                    _logger.LogInformation($"Attempting to make booking for BusId: {bus.BusId}, SeatIds:{i}");
                    var seats = new Seat
                    {
                        BusId = bus.BusId,
                        IsAvailable = true,
                        SeatId = i,
                        SeatPrice = seatPrice

                    };
                    await _seatRepository.Add( seats );
                }
                return bus;


            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding bus");
                throw; // Re-throw the exception for the caller to handle
            }
        }

        //    public async Task<List<BusDto>> SearchBusesAsync(string origin, string destination, DateTime date, string busType)
        //    {
        //        try
        //        {
        //            // Implement logic to filter available buses based on origin, destination, and travel date
        //            //var availableBuses = _busRepository.GetBusesByRoute(origin, destination);
        //            var buses = await _busRepository.GetAsync();
        //            var availableBuses = buses
        //.Where(b =>
        //    b.BusRoute != null &&
        //    b.BusRoute.Any(r =>
        //        r.Route != null &&
        //        r.Route.Origin == origin &&
        //        r.Route.Destination == destination &&
        //         r.Route.TravelDate == travelDate.Date))
        //.ToList();

        //            if (availableBuses == null || !availableBuses.Any())
        //            {
        //                throw new BusNotFoundException();
        //            }

        //            }
        //            var busDtos = buses.Select(bus => new BusDto
        //            {
        //                BusId = bus.BusId,
        //                BusName = bus.BusName,
        //                BusType = bus.BusType ,
        //                Origin = origin,
        //                Destination = destination
        //            }).ToList();
        //        }
        //        catch (Exception ex)
        //        {
        //            _logger.LogError($"Error while getting available buses: {ex.Message}");
        //            throw;
        //        }
        //    }
        public async Task<Bus> DeleteBus(int busId)
        {
            try
            {
                return await _busRepository.Delete(busId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting bus");
                throw;
            }
        }



        public async Task<List<BusDTOForUser>> GetAvailableBuses(string origin, string destination, DateTime travelDate)
        {
            try
            {
                //to filter available buses based on origin, destination, and travel date
                var buses = await _busRepository.GetAsync();
                var availableBuses = buses
                    .Where(b => b.BusRoute != null && b.BusRoute.Any(r => r.Route != null && r.Route.Origin == origin &&
                                                      r.Route.Destination == destination &&
                                                      r.Route.TravelDate == travelDate.Date))
                    .ToList();
                _logger.LogInformation($"Origin: {origin}, Destination: {destination}, TravelDate: {travelDate}");
                _logger.LogInformation($"Number of available buses: {availableBuses.Count}");
                foreach (var bus in availableBuses)
                {
                    _logger.LogInformation($"Bus ID: {bus.BusId}, Bus Name: {bus.BusName}, Bus Type: {bus.BusType}");
                }


                if (!availableBuses.Any())
                {
                    throw new BusNotFoundException();
                }
                //_logger.LogInformation($"Origin: {origin}, Destination: {destination}, TravelDate: {travelDate}");
                //_logger.LogInformation($"{availableBuses}");

                return availableBuses.Select(bus => new BusDTOForUser
                {
                    BusId = bus.BusId,
                    BusName = bus.BusName,
                    BusType = bus.BusType,
                    Origin = origin,
                    Destination = destination
                }).ToList();

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while getting available buses: {ex.Message}");
                throw;
            }
        }



        //--------------
        public async Task<List<BusDTOForUser>> GetAvailableBuses(string origin, string destination, DateTime travelDate, string busType)
        {
            try
            {
                //to filter available buses based on origin, destination, and travel date
                var buses = await _busRepository.GetAsync();
                var availableBuses = buses.Where((b => b.BusType==busType && b.BusRoute != null && b.BusRoute.Any(r => r.Route != null && r.Route.Origin == origin &&
                                                  r.Route.Destination == destination &&
                                                  r.Route.TravelDate == travelDate.Date))).ToList();

                //.Where(b => b.BusRoute != null && b.BusRoute.Any(r => r.Route != null && r.Route.Origin == origin &&
                //                                  r.Route.Destination == destination &&
                //                                  r.Route.TravelDate == travelDate.Date))
                //.ToList();
                _logger.LogInformation($"Origin: {origin}, Destination: {destination}, TravelDate: {travelDate}");
                _logger.LogInformation($"Number of available buses: {availableBuses.Count}");
                foreach (var bus in availableBuses)
                {
                    _logger.LogInformation($"Bus ID: {bus.BusId}, Bus Name: {bus.BusName}, Bus Type: {bus.BusType}");
                }


                if (!availableBuses.Any())
                {
                    throw new BusNotFoundException();
                }
                //_logger.LogInformation($"Origin: {origin}, Destination: {destination}, TravelDate: {travelDate}");
                //_logger.LogInformation($"{availableBuses}");

                return availableBuses.Select(bus => new BusDTOForUser
                {
                    BusId = bus.BusId,
                    BusName = bus.BusName,
                    BusType = bus.BusType,
                    Origin = origin,
                    Destination = destination
                }).ToList();

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while getting available buses: {ex.Message}");
                throw;
            }
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
            var buses = await _busRepository.GetAsync();
            if (buses == null || buses.Count == 0) // Check if the list is empty
            {
                throw new BusNotFoundException();
            }
            return buses;
        }

        // Updated method to get total seats by bus ID
        public async Task<int> GetTotalSeatsAsync(int busId)
        {
            var bus = await GetBus(busId);

            if (bus != null)
                return bus.TotalSeats;

            throw new BusNotFoundException();
        }


    }
}

