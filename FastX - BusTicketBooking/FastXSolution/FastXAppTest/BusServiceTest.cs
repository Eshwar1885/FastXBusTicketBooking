using NUnit.Framework;
using Moq;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using FastX.Services;
using FastX.Repositories;
using FastX.Exceptions;
using FastX.Models;
using FastX.Interfaces;
using FastX.Models.DTOs;
using Microsoft.AspNetCore.Routing;

namespace FastX.Tests.Services
{
    [TestFixture]
    public class BusServiceTests
    {
        private Mock<IRepository<int, Bus>> _mockBusRepository;
        private Mock<IRepository<int, BusOperator>> _mockBusOperatorRepository;
        private Mock<ILogger<BusService>> _mockLogger;
        private readonly IRepository<int, Bus> _busRepository;
        private readonly IRepository<int, BusOperator> _busOperatorRepository;
        private readonly ILogger<BusService> _logger;
        private BusService _busService;

        public BusServiceTests()
        {
            _mockBusRepository = new Mock<IRepository<int, Bus>>();
            _mockBusOperatorRepository = new Mock<IRepository<int, BusOperator>>();
            _mockLogger = new Mock<ILogger<BusService>>();
            _busRepository = _mockBusRepository.Object;
            _busOperatorRepository = _mockBusOperatorRepository.Object;
            _logger = _mockLogger.Object;
        }

        [SetUp]
        public void Setup()
        {
            _busService = new BusService(_busRepository, _busOperatorRepository, _logger);
        }

        [Test]
        public async Task AddBus_WhenBusOperatorNotFound_ShouldThrowException()
        {
            // Arrange
            _mockBusOperatorRepository.Setup(repo => repo.GetAsync(It.IsAny<int>())).ReturnsAsync((BusOperator)null);

            // Act & Assert
            Assert.ThrowsAsync<BusOperatorNotFoundException>(() => _busService.AddBus("Test Bus", "Test Type", 50, 1));
        }

        [Test]
        public async Task GetAvailableBuses_WhenBusesFound_ShouldReturnList()
        {
            // Arrange
            var mockBuses = new List<Bus>
            {
                new Bus { BusId = 1, BusName = "Bus 1", BusType = "Type A", BusRoute = new List<BusRoute> {
                    new BusRoute { Route = new Routee { Origin = "Origin", Destination = "Destination", TravelDate = DateTime.Now.Date } }
                } }
            };
            _mockBusRepository.Setup(repo => repo.GetAsync()).ReturnsAsync(mockBuses);

            // Act
            var result = await _busService.GetAvailableBuses("Origin", "Destination", DateTime.Now);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<List<BusDTOForUser>>(result);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Bus 1", result[0].BusName);
        }

        [Test]
        public async Task GetAvailableBuses_WhenNoBusesFound_ShouldThrowException()
        {
            // Arrange
            _mockBusRepository.Setup(repo => repo.GetAsync()).ReturnsAsync(new List<Bus>());

            // Act & Assert
            Assert.ThrowsAsync<BusNotFoundException>(() => _busService.GetAvailableBuses("Origin", "Destination", DateTime.Now));
        }

        [Test]
        public void GetBusList_WhenNoBusesFound_ShouldThrowException()
        {
            // Arrange
            _mockBusRepository.Setup(repo => repo.GetAsync()).ReturnsAsync(new List<Bus>());

            // Act & Assert
            Assert.ThrowsAsync<BusNotFoundException>(() => _busService.GetBusList());
        }




        // Add more tests for AddBus method to cover other scenarios

        // Add more tests for GetBusList method to cover other scenarios
    }
}
