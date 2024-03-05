using FastX.Contexts;
using FastX.Exceptions;
using FastX.Interfaces;
using FastX.Models;
using FastX.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging; // Added namespace for ILogger
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace FastX.Services
{
    public class AmenityService : IAmenityService
    {
        private readonly IRepository<int, Bus> _busRepository;
        private readonly IAmenityRepository<int, Amenity> _repo;
        private readonly ILogger<AmenityService> _logger;
        private readonly FastXContext _context;

        // Constructor
        public AmenityService(IRepository<int, Bus> busRepository,
            IAmenityRepository<int, Amenity> repo,
            ILogger<AmenityService> logger, FastXContext context)
        {
            _busRepository = busRepository;
            _logger = logger;
            _context = context;
            _repo = repo;
        }

        /// <summary>
        /// Add a new amenity.
        /// </summary>
        /// <param name="amenity">The amenity to add.</param>
        /// <returns>The added amenity.</returns>
        public async Task<Amenity> AddAmenity(Amenity amenity)
        {
            amenity = await _repo.Add(amenity);
            _logger.LogInformation("Amenity added successfully.");
            return amenity;
        }

        /// <summary>
        /// Change the name of an existing amenity.
        /// </summary>
        /// <param name="id">The ID of the amenity to change.</param>
        /// <param name="name">The new name for the amenity.</param>
        /// <returns>The updated amenity.</returns>
        public async Task<Amenity> ChangeAmenityNameAsync(int id, string name)
        {
            var amenity = await _repo.GetAsync(id);
            if (amenity != null)
            {
                amenity.Name = name;
                amenity = await _repo.Update(amenity);
                _logger.LogInformation("Amenity name changed successfully.");
                return amenity;
            }
            throw new AmenitiesNotFoundException();
        }

        /// <summary>
        /// Delete an amenity by ID.
        /// </summary>
        /// <param name="id">The ID of the amenity to delete.</param>
        /// <returns>The deleted amenity.</returns>
        public async Task<Amenity> DeleteAmenity(int id)
        {
            var amenity = await GetAmenity(id);
            if (amenity != null)
            {
                amenity = await _repo.Delete(id);
                _logger.LogInformation("Amenity deleted successfully.");
                return amenity;
            }
            throw new AmenitiesNotFoundException();
        }

        /// <summary>
        /// Get an amenity by ID.
        /// </summary>
        /// <param name="id">The ID of the amenity to get.</param>
        /// <returns>The retrieved amenity.</returns>
        public async Task<Amenity> GetAmenity(int id)
        {
            var amenity = await _repo.GetAsync(id);
            if (amenity == null)
            {
                throw new AmenitiesNotFoundException();
            }
            return amenity;
        }

        /// <summary>
        /// Get a list of all amenities.
        /// </summary>
        /// <returns>A list of amenities.</returns>
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
        /// <summary>
        /// Add an amenity to a bus.
        /// </summary>
        /// <param name="busId">The ID of the bus.</param>
        /// <param name="amenityName">The name of the amenity to add.</param>
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
                _logger.LogInformation("Amenity added to bus successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding amenity to bus");
                throw;
            }
        }

        /// <summary>
        /// Delete an amenity from a bus.
        /// </summary>
        /// <param name="busId">The ID of the bus.</param>
        /// <param name="amenityName">The name of the amenity to delete.</param>
        [ExcludeFromCodeCoverage]
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
                _logger.LogInformation("Amenity deleted from bus successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting amenity from bus");
                throw;
            }
        }


        public async Task AddAmenitiesToBus(int busId, List<string> amenityNames)
        {
            try
            {
                var bus = await _busRepository.GetAsync(busId);
                if (bus == null)
                {
                    throw new BusNotFoundException();
                }

                foreach (var amenityName in amenityNames)
                {
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
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding amenities to bus");
                throw;
            }
        }



        //public async Task<List<string>> GetAmenityNamesByBusId(int busId)
        //{
        //    var bus = await _busRepository.GetAsync(busId);

        //    if (bus != null)
        //    {
        //        var amenityNames = bus.BusAmenities
        //            .Select(ba => ba.Amenity.Name)
        //            .ToList();

        //        return amenityNames;
        //    }

        //    throw new BusNotFoundException();
        //}

        [ExcludeFromCodeCoverage]
        public async Task<List<AmenityDTO>> GetBusAmenitiesAsync(int busId)
        {
            try
            {
                _logger.LogInformation($"Fetching amenities for Bus ID: {busId}");

                // Call the repository method to retrieve the bus by ID
                //var bus = await _busRepository.GetAsync(busId);
                var bus = await _context.Buses
            .Include(b => b.BusAmenities)
            .ThenInclude(ba => ba.Amenity)
            .FirstOrDefaultAsync(b => b.BusId == busId);

                if (bus == null)
                {
                    _logger.LogWarning($"Bus not found for ID: {busId}");
                    throw new BusNotFoundException();
                }

                // Access the bus's amenities and select AmenityId and Name
                //    var amenities = bus.BusAmenities
                //.Select(ba => ba.Amenity)
                //.ToList();
                var amenities = bus.BusAmenities
                .Select(ba => new AmenityDTO
                {
                    AmenityId = ba.Amenity.AmenityId,
                    Name = ba.Amenity.Name
                })
                .ToList();


                if (amenities == null || !amenities.Any())
                {
                    _logger.LogWarning($"No amenities found for Bus ID: {busId}");
                    throw new AmenitiesNotFoundException();
                }

                _logger.LogInformation($"Amenities fetched successfully for Bus ID: {busId}");

                return amenities;
            }
            catch (BusNotFoundException)
            {
                // Rethrow the exception if the bus is not found
                throw;
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes
                _logger.LogError($"An error occurred: {ex}");
                throw new Exception("An error occurred while processing your request.");
            }
        }






    }
}