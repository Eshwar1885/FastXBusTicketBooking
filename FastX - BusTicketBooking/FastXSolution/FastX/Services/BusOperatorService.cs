using FastX.Exceptions;
using FastX.Interfaces;
using FastX.Models;
using FastX.Models.DTOs;
using FastX.Repositories;
using Microsoft.Extensions.Logging;

namespace FastX.Services
{
    public class BusOperatorService : IBusOperatorService
    {
        private readonly IRepository<int, Bus> _busRepository;
        private readonly ILogger<BusService> _logger;
        private readonly IPaymentService _paymentService;
        private readonly IBookingService _bookingService;
        private readonly IRepository<int, User> _userRepository;
        private readonly IBookingRepository<int, Booking> _bookingRepository;
        private readonly ITicketService _ticketService;




        public BusOperatorService(IRepository<int, Bus> busRepository, ILogger<BusService> logger,
            IPaymentService paymentService, IBookingService bookingService,
            IRepository<int, User> userRepository, IBookingRepository<int, Booking> bookingRepository, 
            ITicketService ticketService)
        {
            _busRepository = busRepository;
            _logger = logger;
            _paymentService = paymentService;
            _bookingService = bookingService;
            _userRepository = userRepository;
            _bookingRepository = bookingRepository;
            _ticketService = ticketService;
        }

        public async Task<List<BusDTOForOperator>> GetAllBuses(int busOperatorId)
        {
            try
            {
                var allBuses = await _busRepository.GetAsync();
                var buses = allBuses.Where(b => b.BusOperatorId == busOperatorId);
                //var buses = await _busRepository.GetAsync(b => b.BusOperatorId == busOperatorId);

                if (buses == null || !buses.Any())
                {
                    throw new BusOperatorNotFoundException();
                }

                return buses.Select(b => new BusDTOForOperator
                {
                    BusId = b.BusId,
                    BusName = b.BusName,
                    BusOperatorId = b.BusOperatorId,
                    BusType = b.BusType
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving buses for bus operator ID: {BusOperatorId}", busOperatorId);
                throw;
            }
        }

        //public async Task<List<RefundDTO>> RefundRequest(int userId)
        //{
        //    var cancelledBookings = await _bookingService.GetCancelledBookings(userId);

        //    if (cancelledBookings == null || cancelledBookings.Count == 0)
        //    {
        //        throw new BookingNotFoundException();
        //    }

        //    var refundDTOs = new List<RefundDTO>();

        //    foreach (var cancelledBooking in cancelledBookings)
        //    {
        //        var price = await _paymentService.FindRefundPrice(userId, cancelledBooking.BookingId);
        //        var bus = await _bookingService.GetBookingInfo(cancelledBooking.BookingId, cancelledBooking.BookedForWhichDate);

        //        var refundDTO = new RefundDTO
        //        {
        //            BookingId = cancelledBooking.BookingId,
        //            BusName = bus?.BusName,
        //            BusType = bus?.BusType,
        //            NumberOfSeats = cancelledBooking.NumberOfSeats,
        //            BookedForWhichDate = cancelledBooking.BookedForWhichDate,
        //            Origin = bus?.BusRoute?.FirstOrDefault()?.Route?.Origin,
        //            Destination = bus?.BusRoute?.FirstOrDefault()?.Route?.Destination,
        //            SeatNumbers = cancelledBooking.Tickets != null ? string.Join(",", cancelledBooking.Tickets.Select(t => t.SeatId)) : "",
        //            TotalCost = price
        //        };

        //        refundDTOs.Add(refundDTO);
        //    }

        //    return refundDTOs;
        //}



        //public async Task<List<RefundDTO>> RefundRequest(int userId)
        //{
        //    var cancelledBookings = await _bookingService.GetCancelledBookings(userId);

        //    if (cancelledBookings == null || cancelledBookings.Count == 0)
        //    {
        //        throw new BookingNotFoundException();
        //    }

        //    var refundDTOs = new List<RefundDTO>();

        //    foreach (var cancelledBooking in cancelledBookings)
        //    {
        //        var price = await _paymentService.FindRefundPrice(userId, cancelledBooking.BookingId);

        //        var refundDTO = new RefundDTO
        //        {
        //            BookingId = cancelledBooking.BookingId,
        //            BusName = cancelledBooking.BusName,
        //            BusType = cancelledBooking.BusType,
        //            NumberOfSeats = cancelledBooking.NumberOfSeats,
        //            BookedForWhichDate = cancelledBooking.BookedForWhichDate,
        //            Origin = cancelledBooking.Origin,
        //            Destination = cancelledBooking.Destination,
        //            SeatNumbers = cancelledBooking.SeatNumbers, // Access SeatNumbers directly
        //            TotalCost = price
        //        };

        //        refundDTOs.Add(refundDTO);
        //    }

        //    return refundDTOs;
        //}

        public async Task<List<RefundDTO>> GetRefundDetailsForCancelledBookings()
        {
            var users = await _userRepository.GetAsync();

            if (users == null || !users.Any())
            {
                throw new NoSuchUserException();
            }

            var refundDetails = new List<RefundDTO>();

            foreach (var user in users)
            {
                var cancelledBookings = user.Bookings
                    .Where(b => b.Status == "cancelled");

                foreach (var booking in cancelledBookings)
                {
                    //var totalCost = CalculateTotalCostForBooking(booking);
                    var bus = await _bookingService.GetBookingInfo(booking.BookingId, booking.BookedForWhichDate);

                    var totalCost = await _paymentService.FindRefundPrice(booking.BookingId);
                    var refundDTO = new RefundDTO
                    {
                        UserId = user.UserId,
                        BookingId = booking.BookingId,
                        BusName = bus?.BusName,
                        NumberOfSeats = booking.NumberOfSeats,
                        BookedForWhichDate = booking.BookedForWhichDate,
                        SeatNumbers = booking.Tickets != null ? string.Join(",", booking.Tickets.Select(t => t.SeatId)) : "",
                        TotalCost = totalCost,
                        UserName = user.Username
                    };

                    refundDetails.Add(refundDTO);
                }
            }

            return refundDetails;
        }

        public async Task AcceptRefund(int bookingId, int userId)
        {
            var booking = await _bookingRepository.GetAsync(bookingId);

            booking.Status = "refunded";
            await _bookingRepository.Update(booking);
            await _ticketService.DeleteCancelledBookingTickets(bookingId, userId);
        }



    }
}

        ////--------------- delete bus op and user by admin
        //public async Task DeleteBusOperatorAsync(string username)
        //{
        //    var admin = await _adminRepository.GetByIdAsync(username);
        //    if (admin != null)
        //    {
        //        _adminRepository.DeleteAsync(username);
        //        await _adminRepository.SaveChangesAsync();
        //    }
        //}

        //public async Task DeleteUserAsync(string username)
        //{
        //    var admin = await _adminRepository.GetByIdAsync(username);
        //    if (admin != null)
        //    {
        //        _adminRepository.DeleteAsync(username);
        //        await _adminRepository.SaveChangesAsync();
        //    }
        //}
