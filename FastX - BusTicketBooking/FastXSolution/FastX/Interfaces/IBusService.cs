using FastX.Models;

namespace FastX.Interfaces
{
    public interface IBusService
    {
        public Task<Bus> AddBus(Bus Bus);

        public Task<List<Bus>> GetBusList();
        public Task<Bus> GetBus(int id);

        public Task<Bus> DeleteBus(int id);


    }
}
