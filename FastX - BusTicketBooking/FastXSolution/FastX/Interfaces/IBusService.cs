using FastX.Models;
using FastX.Models.DTOs;

namespace FastX.Interfaces
{
    public interface IBusService
    {
        //public Task<List<Bus>> GetBusList();
        //public Task<Bus> GetBus(int id);
        public Task<Bus> AddBus(string busName, string busType, int totalSeats, int busOperatorId);
        public Task<Bus> DeleteBus(int busId);

        public Task<List<BusDTOForUser>> GetAvailableBuses(string origin, string destination, DateTime travelDate);

        //Task<List<BusDtoForUser>> SearchBusesAsync(string origin, string destination, DateTime date);
        Task<List<BusDTOForUser>> GetAvailableBuses(string origin, string destination, DateTime date, string busType);



        public Task<List<Bus>> GetBusList();
        public Task<Bus> GetBus(int id);






    }
}




