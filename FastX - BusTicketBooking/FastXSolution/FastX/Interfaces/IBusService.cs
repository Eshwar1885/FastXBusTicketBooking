using FastX.Models;

namespace FastX.Interfaces
{
    public interface IBusService
    {
        //public Task<Bus> AddBus(Bus bus);
        public Task<Bus> AddBus(string busName, string busType, int totalSeats, int busOperatorId);


        public Task<List<Bus>> GetBusList();
        public Task<Bus> GetBus(int id);

        //public Task<Bus> DeleteBus(int id);
        public Task<Bus> DeleteBus(int busId);



    }
}
