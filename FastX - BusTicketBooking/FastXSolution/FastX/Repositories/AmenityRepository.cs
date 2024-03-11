using FastX.Contexts;
using FastX.Exceptions;
using FastX.Interfaces;
using FastX.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace FastX.Repositories
{
    public class AmenityRepository : IAmenityRepository<int, Amenity>
    {
        private readonly FastXContext _context;
        public AmenityRepository(FastXContext context)
        {
            _context = context;
        }

        public async Task<Amenity> Add(Amenity item)
        {
            _context.Add(item);
            _context.SaveChanges();
            return item;
        }

        public async Task<Amenity> Delete(int key)
        {
            var amenity = await GetAsync(key);
            _context.Amenities.Remove(amenity);
            _context.SaveChanges();
            //_logger.LogInformation("Amenity deleted " + key);
            return amenity;
        }


        public async Task<Amenity> GetAsync(int key)
        {
            var amenitys = await GetAsync();
            var amenity = amenitys.SingleOrDefault(u => u.AmenityId == key);
            if (amenity != null)
            {
                return amenity;
            }
            throw new AmenitiesNotFoundException();

        }


        public async Task<List<Amenity>> GetAsync()
        {
            var amenity = _context.Amenities.ToList();
            return amenity;
        }


        public async Task<Amenity> Update(Amenity item)
        {
            var amenity = await GetAsync(item.AmenityId);
            _context.Entry<Amenity>(item).State = EntityState.Modified;
            _context.SaveChanges();
            // _logger.LogInformation("Amenity updated " + item.Id);
            return amenity;
        }

        //---------------------------
        public bool Exists(int busId, int amenityId)
        {
            return _context.BusAmenities.Any(ba => ba.BusId == busId && ba.AmenityId == amenityId);
        }

        public void AddBusAmenity(BusAmenity busAmenity)
        {
            _context.BusAmenities.Add(busAmenity);
            _context.SaveChanges();

        }

        public Amenity GetByName(string name)
        {
            return _context.Amenities.FirstOrDefault(a => a.Name == name);
        }

        public void AddAmenity(Amenity amenity)
        {
            _context.Amenities.Add(amenity);
            _context.SaveChanges();

        }


        public void RemoveBusAmenity(int busId, string amenityName)
        {
            var busAmenity = _context.BusAmenities.FirstOrDefault(ba => ba.BusId == busId && ba.Amenity.Name == amenityName);
            if (busAmenity != null)
            {
                _context.BusAmenities.Remove(busAmenity);
                _context.SaveChanges();
            }
        }

        //-------------------------

    }
}
