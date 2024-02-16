using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FastX.Exceptions;
using FastX.Interfaces;
using FastX.Models;
using FastX.Models.DTOs;
using FastX.Services;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace FastX.Tests.Services
{
    [TestFixture]
    public class BusOperatorServiceTest
    {
        private Mock<IRepository<int, Bus>> _mockBusRepository;
        private Mock<ILogger<BusService>> _mockLogger;
        private BusOperatorService _busOperatorService;

        [SetUp]
        public void Setup()
        {
            _mockBusRepository = new Mock<IRepository<int, Bus>>();
            _mockLogger = new Mock<ILogger<BusService>>();
            _busOperatorService = new BusOperatorService(_mockBusRepository.Object, _mockLogger.Object);
        }

        [Test]
        public async Task GetAllBuses_WithExistingBuses_ShouldReturnListOfBusDTOForOperator()
        {
            // Arrange
            int busOperatorId = 1;
            var mockBuses = new List<Bus>
            {
                new Bus { BusId = 1, BusName = "Bus1", BusOperatorId = busOperatorId, BusType = "Type1" },
                new Bus { BusId = 2, BusName = "Bus2", BusOperatorId = busOperatorId, BusType = "Type2" }
            };
            _mockBusRepository.Setup(repo => repo.GetAsync()).ReturnsAsync(mockBuses);

            // Act
            var result = await _busOperatorService.GetAllBuses(busOperatorId);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<List<BusDTOForOperator>>(result);
            Assert.AreEqual(mockBuses.Count, result.Count);
            Assert.IsTrue(result.All(bus => bus.BusOperatorId == busOperatorId));
        }

        [Test]
        public void GetAllBuses_WithNoBusesFound_ShouldThrowBusOperatorNotFoundException()
        {
            // Arrange
            int busOperatorId = 1;
            _mockBusRepository.Setup(repo => repo.GetAsync()).ReturnsAsync(new List<Bus>());

            // Act & Assert
            Assert.ThrowsAsync<BusOperatorNotFoundException>(() => _busOperatorService.GetAllBuses(busOperatorId));
        }
    }
}
