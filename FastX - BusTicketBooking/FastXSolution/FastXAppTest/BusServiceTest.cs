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
using FastX.Exceptions;
using Microsoft.AspNetCore.Routing;

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

            // Act
            var addedBus = await _busService.AddBus("TestBus", "TestType", 10, 1, 100);

            // Assert
            _mockBusRepository.Verify(repo => repo.Add(It.IsAny<Bus>()), Times.Once);
           
            Assert.AreEqual("TestType", addedBus.BusType);
        }

        [Test]
        public async Task AddBus_Should_Throw_BusOperatorNotFoundException_When_Operator_Not_Found()
        {
            // Arrange
            _mockBusOperatorRepository.Setup(repo => repo.GetAsync(It.IsAny<int>()))
                .ReturnsAsync((BusOperator)null);

            // Act & Assert
             Assert.ThrowsAsync<BusOperatorNotFoundException>(() =>
                _busService.AddBus("TestBus", "TestType", 10, 1, 100));
        }

        [Test]
        public async Task DeleteBus_Should_Throw_Exception_When_Repository_Throws()
        {
            // Arrange
            int busId = 1;
            _mockBusRepository.Setup(repo => repo.Delete(busId))
                .ThrowsAsync(new Exception("Repository exception message"));

            // Act & Assert
            Assert.ThrowsAsync<Exception>(() =>
                _busService.DeleteBus(busId));
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
        public async Task GetAvailableBuses_Should_Return_Available_Buses()
        {
            // Arrange
            string origin = "Origin";
            string destination = "Destination";
            DateTime travelDate = DateTime.Now.Date;

            // Mock the list of buses returned by the repository
            var mockBuses = new List<Bus>
    {
        new Bus { BusId = 1, BusName = "Bus1", BusType = "Type1", BusRoute = new List<BusRoute>
        {
            new BusRoute { Route = new Routee { Origin = origin, Destination = destination, TravelDate = travelDate } }
        }},
        new Bus { BusId = 2, BusName = "Bus2", BusType = "Type2", BusRoute = new List<BusRoute>
        {
            new BusRoute { Route = new Routee { Origin = origin, Destination = destination, TravelDate = travelDate } }
        }}
        // Add more buses as needed
    };

            _mockBusRepository.Setup(repo => repo.GetAsync())
                .ReturnsAsync(mockBuses);

            // Act
            var availableBuses = await _busService.GetAvailableBuses(origin, destination, travelDate);

            // Assert
            Assert.IsNotNull(availableBuses);

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
            var expectedList = new List<Bus> { new Bus(), new Bus() };

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

    }
}
