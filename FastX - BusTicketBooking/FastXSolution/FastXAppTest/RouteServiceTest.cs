using FastX.Exceptions;
using FastX.Interfaces;
using FastX.Models;
using FastX.Services;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FastX.Tests.Services
{
    [TestFixture]
    public class RouteServiceTest
    {
        private Mock<IRepository<int, Routee>> _mockRouteRepository;
        private Mock<IRepository<int, Bus>> _mockBusRepository;
        private Mock<IRepository<int, BusRoute>> _mockBusRouteRepository;
        private RouteService _routeService;

        [SetUp]
        public void Setup()
        {
            _mockRouteRepository = new Mock<IRepository<int, Routee>>();
            _mockBusRepository = new Mock<IRepository<int, Bus>>();
            _mockBusRouteRepository = new Mock<IRepository<int, BusRoute>>();
            _routeService = new RouteService(_mockRouteRepository.Object, _mockBusRepository.Object, _mockBusRouteRepository.Object);
        }

        [Test]
        public async Task AddRoutee_Should_Add_Routee_Successfully()
        {
            // Arrange
            var routee = new Routee();
            _mockRouteRepository.Setup(repo => repo.Add(routee)).ReturnsAsync(routee);

            // Act
            var result = await _routeService.AddRoutee(routee);

            // Assert
            Assert.AreEqual(routee, result);
        }

        [Test]
        public async Task DeleteRoutee_Should_Delete_Routee_Successfully_If_Exists()
        {
            // Arrange
            var routeeId = 1;
            var existingRoutee = new Routee { RouteId = routeeId };
            _mockRouteRepository.Setup(repo => repo.GetAsync(routeeId)).ReturnsAsync(existingRoutee);
            _mockRouteRepository.Setup(repo => repo.Delete(routeeId)).ReturnsAsync(existingRoutee);

            // Act
            var result = await _routeService.DeleteRoutee(routeeId);

            // Assert
            Assert.AreEqual(existingRoutee, result);
        }

        [Test]
        public void DeleteRoutee_Should_Throw_NoSuchRouteeException_If_Routee_Does_Not_Exist()
        {
            // Arrange
            var nonExistingRouteeId = 1;
            _mockRouteRepository.Setup(repo => repo.GetAsync(nonExistingRouteeId)).ReturnsAsync((Routee)null);

            // Act & Assert
            Assert.ThrowsAsync<NoSuchRouteeException>(() => _routeService.DeleteRoutee(nonExistingRouteeId));
        }

        [Test]
        public async Task GetRoutee_Should_Return_Routee_IfExists()
        {
            // Arrange
            var routeeId = 1;
            var existingRoutee = new Routee { RouteId = routeeId };
            _mockRouteRepository.Setup(repo => repo.GetAsync(routeeId)).ReturnsAsync(existingRoutee);

            // Act
            var result = await _routeService.GetRoutee(routeeId);

            // Assert
            Assert.AreEqual(existingRoutee, result);
        }

        [Test]
        public async Task GetRoutee_Should_Return_Null_If_Routee_Does_Not_Exist()
        {
            // Arrange
            var nonExistingRouteeId = 1;
            _mockRouteRepository.Setup(repo => repo.GetAsync(nonExistingRouteeId)).ReturnsAsync((Routee)null);

            // Act
            var result = await _routeService.GetRoutee(nonExistingRouteeId);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task GetRouteeList_Should_Return_List_Of_Routees()
        {
            // Arrange
            var routees = new List<Routee> { new Routee(), new Routee() };
            _mockRouteRepository.Setup(repo => repo.GetAsync()).ReturnsAsync(routees);

            // Act
            var result = await _routeService.GetRouteeList();

            // Assert
            Assert.AreEqual(routees, result);
        }

        [Test]
        public async Task AddRouteeToBus_Should_Add_Routee_To_Bus_Successfully_If_Routee_Does_Not_Exist()
        {
            // Arrange
            var busId = 1;
            var origin = "Origin";
            var destination = "Destination";
            var routes = new List<Routee>();
            _mockBusRepository.Setup(repo => repo.GetAsync(busId)).ReturnsAsync(new Bus());
            _mockRouteRepository.Setup(repo => repo.GetAsync()).ReturnsAsync(routes);
            _mockRouteRepository.Setup(repo => repo.Add(It.IsAny<Routee>())).ReturnsAsync(new Routee());
            _mockBusRouteRepository.Setup(repo => repo.Add(It.IsAny<BusRoute>())).ReturnsAsync(new BusRoute());

            // Act
            await _routeService.AddRouteeToBus(busId, origin, destination, DateTime.Now);

            // Assert
            _mockRouteRepository.Verify(repo => repo.Add(It.IsAny<Routee>()), Times.Once);
            _mockBusRouteRepository.Verify(repo => repo.Add(It.IsAny<BusRoute>()), Times.Once);
        }

        [Test]
        public async Task AddRouteeToBus_Should_Add_Routee_To_Bus_Successfully_If_Routee_Exists()
        {
            // Arrange
            var busId = 1;
            var origin = "Origin";
            var destination = "Destination";
            var routes = new List<Routee> { new Routee { Origin = origin, Destination = destination, RouteId = 1 } };
            _mockBusRepository.Setup(repo => repo.GetAsync(busId)).ReturnsAsync(new Bus());
            _mockRouteRepository.Setup(repo => repo.GetAsync()).ReturnsAsync(routes);
            _mockBusRouteRepository.Setup(repo => repo.Add(It.IsAny<BusRoute>())).ReturnsAsync(new BusRoute());

            // Act
            await _routeService.AddRouteeToBus(busId, origin, destination, DateTime.Now);

            // Assert
            _mockBusRouteRepository.Verify(repo => repo.Add(It.IsAny<BusRoute>()), Times.Once);
        }

        [Test]
        public void AddRouteeToBus_Should_Throw_BusNotFoundException_If_Bus_Does_Not_Exist()
        {
            // Arrange
            var busId = 1;
            _mockBusRepository.Setup(repo => repo.GetAsync(busId)).ReturnsAsync((Bus)null);

            // Act & Assert
            Assert.ThrowsAsync<BusNotFoundException>(() => _routeService.AddRouteeToBus(busId, "Origin", "Destination", DateTime.Now));
        }
    }
}
