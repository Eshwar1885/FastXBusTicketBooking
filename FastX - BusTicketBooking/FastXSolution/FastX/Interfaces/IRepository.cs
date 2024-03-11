namespace FastX.Interfaces
{
    public interface IRepository<K, T>
    {
        public Task<List<T>> GetAsync();
        public Task<T> GetAsync(K key);
        public Task<T> Add(T item);
        public Task<T> Update(T item);
        public Task<T> Delete(K key);
    }
}
