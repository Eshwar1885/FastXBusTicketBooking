using FastX.Models;

namespace FastX.Interfaces
{
    public interface IAmenityRepository<K, T>
    {
        public Task<List<T>> GetAsync();
        public Task<T> GetAsync(K key);
        public Task<T> Add(T item);
        public Task<T> Update(T item);
        public Task<T> Delete(K key);
        //------------------------------------
        Amenity GetByName(string name);
        void AddAmenity(Amenity amenity);
        //----------------------------------
        bool Exists(int busId, int amenityId);
        void AddBusAmenity(BusAmenity busAmenity);
        void RemoveBusAmenity(int busId, string amenityName);
    }
}