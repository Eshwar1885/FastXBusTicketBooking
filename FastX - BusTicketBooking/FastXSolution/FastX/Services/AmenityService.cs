using FastX.Exceptions;
using FastX.Interfaces;
using FastX.Models;
using Microsoft.AspNetCore.Server.IIS.Core;
using System.Numerics;

namespace FastX.Services
{
    public class AmenityService : IAmenityService
    {
        private readonly IAmenityRepository<int, Amenity> _repo;
        private readonly IRepository<int, Bus> _busRepository;
        private readonly ILogger<AmenityService> _logger;

        public AmenityService(IAmenityRepository<int, Amenity> repo,
            IRepository<int, Bus> busRepository,
            ILogger<AmenityService> logger)
        {
            _repo = repo;
            _busRepository = busRepository;
            _logger = logger;
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
                    throw new AmenityAlreadyExistsException("Amenity already exists for this bus");
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

    }
}
