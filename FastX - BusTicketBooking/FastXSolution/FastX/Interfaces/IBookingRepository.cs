using FastX.Models;

namespace FastX.Interfaces
{
    public interface IBookingRepository<K, T>
    {
        public Task<Booking?> GetOngoingBookingAsync(int busId, int userId, DateTime travelDate);
        public Task<List<T>> GetAsync();
        public Task<T> GetAsync(K key);
        public Task<T> Add(T item);
        public Task<T> Update(T item);
        public Task<T> Delete(K key);
    }
}