using FastX.Models.DTOs;

namespace FastX.Interfaces
{
    public interface IBusOperatorService
    {
        public Task<List<BusDTOForOperator>> GetAllBuses(int busOperatorId);
        //Task DeleteBusOperatorAsync(string username);
        //Task DeleteUserAsync(string username);
        //public Task<List<RefundDTO>> RefundRequest(int userId);
        public Task<List<RefundDTO>> GetRefundDetailsForCancelledBookings();
        public Task AcceptRefund(int bookingId, int userId);


    }
}
