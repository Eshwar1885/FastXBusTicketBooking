using NUnit.Framework;
using Moq;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using FastX.Services;
using FastX.Models;
using FastX.Interfaces;
using System.Collections.Generic;
using FastX.Exceptions;

namespace FastXAppTest
{
    [TestFixture]
    public class PaymentServiceTest
    {
        private Mock<ILogger<PaymentService>> _mockLogger;
        private Mock<IBookingRepository<int, Booking>> _mockBookingRepository;
        private Mock<IRepository<int, Payment>> _mockPaymentRepository;
        private Mock<ISeatService> _mockSeatService;
        private PaymentService _paymentService;
        private Mock<PaymentService> _mockPaymentService;


        [SetUp]
        public void Setup()
        {
            _mockLogger = new Mock<ILogger<PaymentService>>();
            _mockBookingRepository = new Mock<IBookingRepository<int, Booking>>();
            _mockPaymentRepository = new Mock<IRepository<int, Payment>>();
            _mockSeatService = new Mock<ISeatService>();
            //_mockPaymentService = new Mock<IPaymentService>();
            _mockPaymentService = new Mock<PaymentService>();


            _paymentService = new PaymentService(
                _mockBookingRepository.Object,
                _mockPaymentRepository.Object,
                _mockSeatService.Object,
                _mockLogger.Object
                //_mockPaymentService.Object
            );
        }


        //[Ignore("payment")]
        //[Test]
        //public async Task CreatePayment_Should_Successfully_Create_Payment_For_Existing_Booking()
        //{
        //    // Arrange
        //    var bookingId = 123;
        //    var booking = new Booking { BookingId = bookingId };
        //    var totalPrice = 100.0f; // Mock total price

        //    // Mock the CalculateTotalPriceAsync method result
        //    //_mockPaymentService
        //    //    .Setup(service => service.CalculateTotalPriceAsync(booking))
        //    //    .ReturnsAsync(totalPrice);

        //    // Setup other mocks
        //    _mockBookingRepository.Setup(repo => repo.GetAsync(bookingId)).ReturnsAsync(booking);
        //    _mockPaymentRepository.Setup(repo => repo.Add(It.IsAny<Payment>())).ReturnsAsync(new Payment());
        //    _mockBookingRepository.Setup(repo => repo.Update(It.IsAny<Booking>())).ReturnsAsync(booking);

        //    // Act
        //    await _paymentService.CreatePayment(bookingId);

        //    // Assert
        //    // Verify that the payment repository's Add method is called once
        //    _mockPaymentRepository.Verify(repo => repo.Add(It.IsAny<Payment>()), Times.Once);

        //    // Verify that the booking repository's Update method is called once
        //    //_mockBookingRepository.Verify(repo => repo.Update(booking), Times.Once);
        //}













        [Test]
        public async Task AddPayment_Should_Add_Payment_Successfully()
        {
            // Arrange
            var payment = new Payment();
            _mockPaymentRepository.Setup(repo => repo.Add(payment)).ReturnsAsync(payment);

            // Act
            var result = await _paymentService.AddPayment(payment);

            // Assert
            Assert.AreEqual(payment, result);
        }

        [Test]
        public async Task GetPaymentBy_Should_Return_Payment_By_Id()
        {
            // Arrange
            var paymentId = 123;
            var payment = new Payment { PaymentId = paymentId };
            _mockPaymentRepository.Setup(repo => repo.GetAsync(paymentId)).ReturnsAsync(payment);

            // Act
            var result = await _paymentService.GetPaymentBy(paymentId);

            // Assert
            Assert.AreEqual(payment, result);
        }

        [Test]
        public async Task GetPaymentList_Should_Return_List_Of_Payments()
        {
            // Arrange
            var payments = new List<Payment> { new Payment(), new Payment(), new Payment() };
            _mockPaymentRepository.Setup(repo => repo.GetAsync()).ReturnsAsync(payments);

            // Act
            var result = await _paymentService.GetPaymentList();

            // Assert
            Assert.AreEqual(payments, result);
        }

        [Test]
        public async Task FindRefundPrice_Should_Return_Refund_Price_For_Booking()
        {
            // Arrange
            var bookingId = 123;
            var payments = new List<Payment>
        {
            new Payment { BookingId = bookingId, Amount = 50.0f },
            new Payment { BookingId = bookingId, Amount = 75.0f }
        };
            _mockPaymentRepository.Setup(repo => repo.GetAsync()).ReturnsAsync(payments);

            // Act
            var result = await _paymentService.FindRefundPrice(bookingId);

            // Assert
            Assert.AreEqual(50.0f, result);
        }

    }
}
