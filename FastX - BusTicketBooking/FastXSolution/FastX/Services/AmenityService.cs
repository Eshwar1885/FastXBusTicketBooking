using FastX.Contexts;
using FastX.Exceptions;
using FastX.Interfaces;
using FastX.Models;
using FastX.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace FastX.Services
{
    public class AmenityService : IAmenityService
    {
        private readonly IRepository<int, Bus> _busRepository;
        private readonly IAmenityRepository<int, Amenity> _repo;
        private readonly ILogger<BusService> _logger;
        private readonly FastXContext _context;

        public AmenityService(IRepository<int, Bus> busRepository,
            IAmenityRepository<int, Amenity> repo,
            ILogger<BusService> logger, FastXContext context)
        {
            _busRepository = busRepository;
            _logger = logger;
            _context = context;
            _repo = repo;
        }




        public async Task<Amenity> AddAmenity(Amenity amenity)
        {
            amenity = await _repo.Add(amenity);
            return amenity;
        }

        public async Task<Amenity> ChangeAmenityNameAsync(int id, string name)
        {
            var amenity = await _repo.GetAsync(id);
            if (amenity != null)
            {
                amenity.Name = name;
                amenity = await _repo.Update(amenity);
                return amenity;
            }
            throw new AmenitiesNotFoundException();
        }

        public async Task<Amenity> DeleteAmenity(int id)
        {
            var amenity = await GetAmenity(id);
            if (amenity != null)
            {
                amenity = await _repo.Delete(id);
                return amenity;
            }
            throw new AmenitiesNotFoundException();
        }

        public async Task<Amenity> GetAmenity(int id)
        {
            var amenity = await _repo.GetAsync(id);
            if (amenity == null)
            {
                throw new AmenitiesNotFoundException();
            }
            return amenity;
        }

        public async Task<List<Amenity>> GetAmenityList()
        {
            var amenity = await _repo.GetAsync();
            if (amenity == null)
            {
                throw new AmenitiesNotFoundException();
            }
            return amenity;
        }
        //--------------------
        public async Task AddAmenityToBus(int busId, string amenityName)
        {
            try
            {
                var bus = await _busRepository.GetAsync(busId);
                if (bus == null)
                {
                    throw new BusNotFoundException();
                }

                var amenity = _repo.GetByName(amenityName);
                if (amenity == null)
                {
                    amenity = new Amenity { Name = amenityName };
                    _repo.AddAmenity(amenity);
                }

                if (_repo.Exists(busId, amenity.AmenityId))
                {
                    throw new AmenityAlreadyExistsException();
                }

                _repo.AddBusAmenity(new BusAmenity { BusId = busId, AmenityId = amenity.AmenityId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding amenity to bus");
                throw;
            }
        }




        public async Task DeleteAmenityFromBus(int busId, string amenityName)
        {
            try
            {
                var bus = await _busRepository.GetAsync(busId);
                if (bus == null)
                {
                    throw new BusNotFoundException();
                }

                var amenity = _repo.GetByName(amenityName);
                if (amenity == null)
                {
                    throw new AmenitiesNotFoundException();
                }

                if (!_repo.Exists(busId, amenity.AmenityId))
                {
                    throw new AmenitiesNotFoundException();
                }

                _repo.RemoveBusAmenity(busId, amenityName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting amenity from bus");
                throw;
            }
        }


        //public async Task<List<AmenityDto>> GetBusAmenitiesAsync(int busId)
        //{
        //    try
        //    {
        //        _logger.LogInformation($"Fetching amenities for Bus ID: {busId}");

        //        // Call the repository method to retrieve the bus by ID
        //        //var bus = await _busRepository.GetAsync(busId);
        //        var bus = await _context.Buses
        //    .Include(b => b.BusAmenities)
        //    .ThenInclude(ba => ba.Amenity)
        //    .FirstOrDefaultAsync(b => b.BusId == busId);

        //        if (bus == null)
        //        {
        //            _logger.LogWarning($"Bus not found for ID: {busId}");
        //            throw new BusNotFoundException();
        //        }

        //        // Access the bus's amenities and select AmenityId and Name
        //        //    var amenities = bus.BusAmenities
        //        //.Select(ba => ba.Amenity)
        //        //.ToList();
        //        var amenities = bus.BusAmenities
        //        .Select(ba => new AmenityDto
        //        {
        //            AmenityId = ba.Amenity.AmenityId,
        //            Name = ba.Amenity.Name
        //        })
        //        .ToList();


        //        if (amenities == null || !amenities.Any())
        //        {
        //            _logger.LogWarning($"No amenities found for Bus ID: {busId}");
        //            throw new AmenitiesNotFoundException();
        //        }

        //        _logger.LogInformation($"Amenities fetched successfully for Bus ID: {busId}");

        //        return amenities;
        //    }
        //    catch (BusNotFoundException)
        //    {
        //        // Rethrow the exception if the bus is not found
        //        throw;
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log the exception for debugging purposes
        //        _logger.LogError($"An error occurred: {ex}");
        //        throw new Exception("An error occurred while processing your request.");
        //    }
        //}


    }
}