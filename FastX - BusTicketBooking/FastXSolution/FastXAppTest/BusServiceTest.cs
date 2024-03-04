using NUnit.Framework;
using Moq;
using FastX.Services;
using FastX.Models;
using FastX.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FastX.Interfaces;
using FastX.Models.DTOs;

namespace FastX.Tests
{
    [TestFixture]
    public class BusServiceTests
    {
        private BusService _busService;
        private Mock<IRepository<int, Bus>> _mockBusRepository;
        private Mock<ILogger<BusService>> _mockLogger;
        private Mock<IRepository<int, BusOperator>> _mockBusOperatorRepository;
        private Mock<ISeatRepository<int, Seat>> _mockSeatRepository;

        [SetUp]
        public void Setup()
        {
            _mockBusRepository = new Mock<IRepository<int, Bus>>();
            _mockLogger = new Mock<ILogger<BusService>>();
            _mockBusOperatorRepository = new Mock<IRepository<int, BusOperator>>();
            _mockSeatRepository = new Mock<ISeatRepository<int, Seat>>();

            _busService = new BusService(
                _mockBusRepository.Object,
                _mockBusOperatorRepository.Object,
                _mockLogger.Object,
                _mockSeatRepository.Object
            );
        }

        [Test]
        public async Task AddBus_Should_Add_Bus()
        {
            // Arrange
            var expectedBus = new Bus();
            var expectedBusOperator = new BusOperator();

            _mockBusOperatorRepository.Setup(repo => repo.GetAsync(It.IsAny<int>()))
                .ReturnsAsync(expectedBusOperator);

            _mockBusRepository.Setup(repo => repo.Add(It.IsAny<Bus>()))
                .ReturnsAsync(expectedBus);

            // Act
            var addedBus = await _busService.AddBus("TestBus", "TestType", 10, 1, 100);

            // Assert
            Assert.AreEqual(expectedBus, addedBus);
        }


        [Test]
        public async Task DeleteBus_Should_Delete_Bus()
        {
            // Arrange
            var busId = 1;
            var expectedBus = new Bus();

            _mockBusRepository.Setup(repo => repo.Delete(busId))
                .ReturnsAsync(expectedBus);

            // Act
            var deletedBus = await _busService.DeleteBus(busId);

            // Assert
            Assert.AreEqual(expectedBus, deletedBus);
        }

        [Test]
        public async Task GetAvailableBuses_Should_Return_List()
        {
            // Arrange
            var origin = "Origin";
            var destination = "Destination";
            var travelDate = DateTime.Now.Date;
            var busType = "BusType";

            var expectedBuses = new List<Bus>
    {
        new Bus { BusId = 1, BusName = "Bus1", BusType = busType },
        new Bus { BusId = 2, BusName = "Bus2", BusType = busType }
    };

            _mockBusRepository.Setup(repo => repo.GetAsync())
                .ReturnsAsync(expectedBuses);

            // Act
            var availableBuses = await _busService.GetAvailableBuses(origin, destination, travelDate, busType);

            // Assert
            Assert.IsNotNull(availableBuses);
            Assert.AreEqual(expectedBuses.Count, availableBuses.Count);
            Assert.IsTrue(availableBuses.All(bus => bus.Origin == origin && bus.Destination == destination && bus.BusType == busType));
        }



        [Test]
        public async Task GetBus_Should_Return_Bus()
        {
            // Arrange
            var busId = 1;
            var expectedBus = new Bus();

            _mockBusRepository.Setup(repo => repo.GetAsync(busId))
                .ReturnsAsync(expectedBus);

            // Act
            var bus = await _busService.GetBus(busId);

            // Assert
            Assert.AreEqual(expectedBus, bus);
        }

        [Test]
        public async Task GetBusList_Should_Return_List()
        {
            // Arrange
            var expectedList = new List<Bus> { new Bus(), new Bus() }; // Non-empty list of buses

            _mockBusRepository.Setup(repo => repo.GetAsync())
                .ReturnsAsync(expectedList);

            // Act
            var busList = await _busService.GetBusList();

            // Assert
            Assert.AreEqual(expectedList, busList);
        }


        [Test]
        public async Task GetTotalSeatsAsync_Should_Return_Int()
        {
            // Arrange
            var busId = 1;
            var expectedSeats = 50;

            _mockBusRepository.Setup(repo => repo.GetAsync(busId))
                .ReturnsAsync(new Bus { TotalSeats = expectedSeats });

            // Act
            var totalSeats = await _busService.GetTotalSeatsAsync(busId);

            // Assert
            Assert.AreEqual(expectedSeats, totalSeats);
        }

        // Add more test cases for other methods as needed
    }
}
