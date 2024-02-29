using FastX.Exceptions;
using FastX.Interfaces;
using FastX.Models;
using FastX.Repositories;
using Microsoft.AspNetCore.Routing;
using System.Numerics;

namespace FastX.Services
{
    public class RouteService : IRouteeService
    {
        private IRepository<int, Routee> _routeRepository;
        private readonly IRepository<int, Bus> _busRepository;
        private readonly IRepository<int, BusRoute> _busRouteRepository;


        //private readonly ILogger<RouteeService> _logger;

        public RouteService(IRepository<int, Routee> routeRepository,IRepository<int, Bus> busRepository, IRepository<int, BusRoute> busRouteRepository)
        {
            _routeRepository = routeRepository;
            _busRepository = busRepository;
            _busRouteRepository = busRouteRepository;
        }
        public async Task<Routee> AddRoutee(Routee routee)
        {
            routee = await _routeRepository.Add(routee);
            return routee;
        }

        public async Task<Routee> DeleteRoutee(int id)
        {
            var routee = await GetRoutee(id);
            if (routee != null)
            {
                routee = await _routeRepository.Delete(id);
                return routee;
            }
            throw new NoSuchRouteeException();
        }

        public async Task<Routee> GetRoutee(int id)
        {
            var routee = await _routeRepository.GetAsync(id);
            return routee;
        }

        public async Task<List<Routee>> GetRouteeList()
        {
            var routee = await _routeRepository.GetAsync();
            return routee;
        }

        public async Task AddRouteeToBus(int busId, string origin, string destination, DateTime travelDate)
        {
            var bus = await _busRepository.GetAsync(busId);
            if (bus == null)
            {
                throw new BusNotFoundException();
            }
            var routes = await GetRouteeList();
            //routes.select
            var matchingRoutes = routes.Where(r => r.Origin == origin && r.Destination == destination).ToList();

            if (matchingRoutes.Count == 0)
            {
                var route = new Routee
                {
                    Origin = origin,
                    Destination = destination,
                    TravelDate = travelDate
                };
                await _routeRepository.Add(route);

                var busRoute = new BusRoute
                {
                    BusId = busId,
                    RouteId = route.RouteId
                };
                await _busRouteRepository.Add(busRoute);
            }
            else
            {
                var busRoute = new BusRoute
                {
                    BusId = busId,
                    RouteId = matchingRoutes[0].RouteId

                };
                await _busRouteRepository.Add(busRoute);
            }
            
        }


        //public async Task AddAmenityToBus(int busId, string amenityName)
        //{
        //    try
        //    {
        //        var bus = await _busRepository.GetAsync(busId);
        //        if (bus == null)
        //        {
        //            throw new BusNotFoundException();
        //        }

        //        var amenity = _repo.GetByName(amenityName);
        //        if (amenity == null)
        //        {
        //            amenity = new Amenity { Name = amenityName };
        //            _repo.AddAmenity(amenity);
        //        }

        //        if (_repo.Exists(busId, amenity.AmenityId))
        //        {
        //            throw new AmenityAlreadyExistsException();
        //        }

        //        _repo.AddBusAmenity(new BusAmenity { BusId = busId, AmenityId = amenity.AmenityId });
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error occurred while adding amenity to bus");
        //        throw;
        //    }
        //}
    }

}