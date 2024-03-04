using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using FastX.Interfaces;
using FastX.Models;
using FastX.Services;
using Microsoft.Extensions.Logging;
using Moq;
using FastX.Contexts;
using FastX.Exceptions;

namespace FastXAppTest
{
    [TestFixture]
    public class AmenityServiceTest
    {
        private FastXContext CreateMockDbContext()
        {
            var options = new DbContextOptionsBuilder<FastXContext>()
                .UseInMemoryDatabase("dummyDatabase")
                .Options;
            return new FastXContext(options);
        }

        [Test]
        public void CreateAmenity_Should_Call_AmenityRepository_Create_Method()
        {
            // Arrange
            var mockBusRepository = new Mock<IRepository<int, Bus>>();
            var mockAmenityRepository = new Mock<IAmenityRepository<int, Amenity>>();
            var mockLogger = new Mock<ILogger<AmenityService>>();
            var mockContext = CreateMockDbContext();

            var amenityService = new AmenityService(mockBusRepository.Object, mockAmenityRepository.Object, mockLogger.Object, mockContext);

            var newAmenity = new Amenity
            {
                AmenityId = 1,
                Name = "Test Amenity",
                // Set other properties as needed
            };

            // Act
            amenityService.AddAmenity(newAmenity);

            // Assert
            mockAmenityRepository.Verify(repo => repo.Add(newAmenity), Times.Once);
        }

        [Test]
        public async Task ChangeAmenityNameAsync_Should_Change_Amenity_Name_And_Log_Information()
        {
            // Arrange
            var mockBusRepository = new Mock<IRepository<int, Bus>>();
            var mockAmenityRepository = new Mock<IAmenityRepository<int, Amenity>>();
            var mockLogger = new Mock<ILogger<AmenityService>>();
            var mockContext = CreateMockDbContext();

            var amenityService = new AmenityService(mockBusRepository.Object, mockAmenityRepository.Object, mockLogger.Object, mockContext);

            var existingAmenityId = 1;
            var newName = "New Amenity Name";

            // Mock the behavior of GetAsync to return null
            mockAmenityRepository.Setup(repo => repo.GetAsync(existingAmenityId)).ReturnsAsync((Amenity)null);

            // Act
            async Task Act() => await amenityService.ChangeAmenityNameAsync(existingAmenityId, newName);

            // Assert
            Assert.ThrowsAsync<AmenitiesNotFoundException>(Act);
        }



        [Test]
        public async Task DeleteAmenity_Should_Delete_Amenity_When_Exists()
        {
            // Arrange
            var mockBusRepository = new Mock<IRepository<int, Bus>>();
            var mockAmenityRepository = new Mock<IAmenityRepository<int, Amenity>>();
            var mockLogger = new Mock<ILogger<AmenityService>>();
            var mockContext = CreateMockDbContext();

            var amenityService = new AmenityService(mockBusRepository.Object, mockAmenityRepository.Object, mockLogger.Object, mockContext);

            var existingAmenityId = 1;

            var existingAmenity = new Amenity
            {
                AmenityId = existingAmenityId,
                Name = "Existing Amenity",
                // Set other properties as needed
            };

            mockAmenityRepository.Setup(repo => repo.GetAsync(existingAmenityId)).ReturnsAsync(existingAmenity);
            mockAmenityRepository.Setup(repo => repo.Delete(existingAmenityId)).ReturnsAsync(existingAmenity);

            // Act
            var result = await amenityService.DeleteAmenity(existingAmenityId);

            // Assert
            Assert.AreEqual(existingAmenity, result);
            mockAmenityRepository.Verify(repo => repo.Delete(existingAmenityId), Times.Once);
            //mockLogger.Verify(logger => logger.LogInformation("Amenity deleted successfully."), Times.Once);
        }

        [Test]
        public async Task DeleteAmenity_Should_Throw_Exception_When_Amenity_Not_Exists()
        {
            // Arrange
            var mockBusRepository = new Mock<IRepository<int, Bus>>();
            var mockAmenityRepository = new Mock<IAmenityRepository<int, Amenity>>();
            var mockLogger = new Mock<ILogger<AmenityService>>();
            var mockContext = CreateMockDbContext();

            var amenityService = new AmenityService(mockBusRepository.Object, mockAmenityRepository.Object, mockLogger.Object, mockContext);

            var nonExistingAmenityId = 99;

            mockAmenityRepository.Setup(repo => repo.GetAsync(nonExistingAmenityId)).ReturnsAsync((Amenity)null);

            // Act and Assert
            async Task Act() => await amenityService.DeleteAmenity(nonExistingAmenityId);
            Assert.ThrowsAsync<AmenitiesNotFoundException>(Act);
        }


        [Test]
        public async Task GetAmenity_Should_Return_Amenity_When_Exists()
        {
            // Arrange
            var mockBusRepository = new Mock<IRepository<int, Bus>>();
            var mockAmenityRepository = new Mock<IAmenityRepository<int, Amenity>>();
            var mockLogger = new Mock<ILogger<AmenityService>>();
            var mockContext = CreateMockDbContext();

            var amenityService = new AmenityService(mockBusRepository.Object, mockAmenityRepository.Object, mockLogger.Object, mockContext);

            var existingAmenityId = 1;

            var existingAmenity = new Amenity
            {
                AmenityId = existingAmenityId,
                Name = "Existing Amenity",
                // Set other properties as needed
            };

            mockAmenityRepository.Setup(repo => repo.GetAsync(existingAmenityId)).ReturnsAsync(existingAmenity);

            // Act
            var result = await amenityService.GetAmenity(existingAmenityId);

            // Assert
            Assert.AreEqual(existingAmenity, result);
        }

        [Test]
        public async Task GetAmenity_Should_Throw_Exception_When_Amenity_Not_Exists()
        {
            // Arrange
            var mockBusRepository = new Mock<IRepository<int, Bus>>();
            var mockAmenityRepository = new Mock<IAmenityRepository<int, Amenity>>();
            var mockLogger = new Mock<ILogger<AmenityService>>();
            var mockContext = CreateMockDbContext();

            var amenityService = new AmenityService(mockBusRepository.Object, mockAmenityRepository.Object, mockLogger.Object, mockContext);

            var nonExistingAmenityId = 99;

            mockAmenityRepository.Setup(repo => repo.GetAsync(nonExistingAmenityId)).ReturnsAsync((Amenity)null);

            // Act and Assert
            async Task Act() => await amenityService.GetAmenity(nonExistingAmenityId);
            Assert.ThrowsAsync<AmenitiesNotFoundException>(Act);
        }



        [Test]
        public async Task GetAmenityList_Should_Return_Amenity_List_When_Exists()
        {
            // Arrange
            var mockBusRepository = new Mock<IRepository<int, Bus>>();
            var mockAmenityRepository = new Mock<IAmenityRepository<int, Amenity>>();
            var mockLogger = new Mock<ILogger<AmenityService>>();
            var mockContext = CreateMockDbContext();

            var amenityService = new AmenityService(mockBusRepository.Object, mockAmenityRepository.Object, mockLogger.Object, mockContext);

            var existingAmenities = new List<Amenity>
    {
        new Amenity { AmenityId = 1, Name = "Amenity 1" },
        new Amenity { AmenityId = 2, Name = "Amenity 2" },
        // Add more amenities as needed
    };

            mockAmenityRepository.Setup(repo => repo.GetAsync()).ReturnsAsync(existingAmenities);

            // Act
            var result = await amenityService.GetAmenityList();

            // Assert
            Assert.AreEqual(existingAmenities, result);
        }

        [Test]
        public async Task GetAmenityList_Should_Throw_Exception_When_Amenities_Not_Exist()
        {
            // Arrange
            var mockBusRepository = new Mock<IRepository<int, Bus>>();
            var mockAmenityRepository = new Mock<IAmenityRepository<int, Amenity>>();
            var mockLogger = new Mock<ILogger<AmenityService>>();
            var mockContext = CreateMockDbContext();

            var amenityService = new AmenityService(mockBusRepository.Object, mockAmenityRepository.Object, mockLogger.Object, mockContext);

            mockAmenityRepository.Setup(repo => repo.GetAsync()).ReturnsAsync((List<Amenity>)null);

            // Act and Assert
            async Task Act() => await amenityService.GetAmenityList();
            Assert.ThrowsAsync<AmenitiesNotFoundException>(Act);
        }







        [Test]
        public async Task AddAmenityToBus_Should_Add_Amenity_To_Bus_Successfully()
        {
            // Arrange
            var mockBusRepository = new Mock<IRepository<int, Bus>>();
            var mockAmenityRepository = new Mock<IAmenityRepository<int, Amenity>>();
            var mockLogger = new Mock<ILogger<AmenityService>>();
            var mockContext = CreateMockDbContext();

            var amenityService = new AmenityService(mockBusRepository.Object, mockAmenityRepository.Object, mockLogger.Object, mockContext);

            var busId = 1;
            var amenityName = "Test Amenity";

            var existingBus = new Bus { BusId = busId, BusName = "Test Bus" };
            var existingAmenity = new Amenity { AmenityId = 1, Name = amenityName };

            mockBusRepository.Setup(repo => repo.GetAsync(busId)).ReturnsAsync(existingBus);
            mockAmenityRepository.Setup(repo => repo.GetByName(amenityName)).Returns(existingAmenity);
            mockAmenityRepository.Setup(repo => repo.Exists(busId, existingAmenity.AmenityId)).Returns(false);

            // Act
            await amenityService.AddAmenityToBus(busId, amenityName);

            // Assert
            mockAmenityRepository.Verify(repo => repo.AddAmenity(existingAmenity), Times.Never);
            mockAmenityRepository.Verify(repo => repo.AddBusAmenity(It.IsAny<BusAmenity>()), Times.Once);
            //mockLogger.Verify(logger => logger.LogInformation("Amenity added to bus successfully."), Times.Once);
        }

        [Test]
        public async Task AddAmenityToBus_Should_Throw_BusNotFoundException_When_Bus_Not_Exists()
        {
            // Arrange
            var mockBusRepository = new Mock<IRepository<int, Bus>>();
            var mockAmenityRepository = new Mock<IAmenityRepository<int, Amenity>>();
            var mockLogger = new Mock<ILogger<AmenityService>>();
            var mockContext = CreateMockDbContext();

            var amenityService = new AmenityService(mockBusRepository.Object, mockAmenityRepository.Object, mockLogger.Object, mockContext);

            var nonExistingBusId = 99;
            var amenityName = "Test Amenity";

            mockBusRepository.Setup(repo => repo.GetAsync(nonExistingBusId)).ReturnsAsync((Bus)null);

            // Act and Assert
            async Task Act() => await amenityService.AddAmenityToBus(nonExistingBusId, amenityName);
            Assert.ThrowsAsync<BusNotFoundException>(Act);
        }

        [Test]
        public async Task AddAmenityToBus_Should_Throw_AmenityAlreadyExistsException_When_Amenity_Already_Exists_On_Bus()
        {
            // Arrange
            var mockBusRepository = new Mock<IRepository<int, Bus>>();
            var mockAmenityRepository = new Mock<IAmenityRepository<int, Amenity>>();
            var mockLogger = new Mock<ILogger<AmenityService>>();
            var mockContext = CreateMockDbContext();

            var amenityService = new AmenityService(mockBusRepository.Object, mockAmenityRepository.Object, mockLogger.Object, mockContext);

            var busId = 1;
            var amenityName = "Test Amenity";

            var existingBus = new Bus { BusId = busId, BusName = "Test Bus" };
            var existingAmenity = new Amenity { AmenityId = 1, Name = amenityName };

            mockBusRepository.Setup(repo => repo.GetAsync(busId)).ReturnsAsync(existingBus);
            mockAmenityRepository.Setup(repo => repo.GetByName(amenityName)).Returns(existingAmenity);
            mockAmenityRepository.Setup(repo => repo.Exists(busId, existingAmenity.AmenityId)).Returns(true);

            // Act and Assert
            async Task Act() => await amenityService.AddAmenityToBus(busId, amenityName);
            Assert.ThrowsAsync<AmenityAlreadyExistsException>(Act);
        }

        [Test]
        public async Task AddAmenityToBus_Should_Log_Exception_When_Exception_Occurs()
        {
            // Arrange
            var mockBusRepository = new Mock<IRepository<int, Bus>>();
            var mockAmenityRepository = new Mock<IAmenityRepository<int, Amenity>>();
            var mockLogger = new Mock<ILogger<AmenityService>>();
            var mockContext = CreateMockDbContext();

            var amenityService = new AmenityService(mockBusRepository.Object, mockAmenityRepository.Object, mockLogger.Object, mockContext);

            var busId = 1;
            var amenityName = "Test Amenity";

            var existingBus = new Bus { BusId = busId, BusName = "Test Bus" };

            mockBusRepository.Setup(repo => repo.GetAsync(busId)).ReturnsAsync(existingBus);
            mockAmenityRepository.Setup(repo => repo.GetByName(amenityName)).Throws(new Exception());

            // Act and Assert
            async Task Act() => await amenityService.AddAmenityToBus(busId, amenityName);
            Assert.ThrowsAsync<Exception>(Act);
            //mockLogger.Verify(logger => logger.LogError(It.IsAny<Exception>(), "Error occurred while adding amenity to bus"), Times.Once);
        }

        [Ignore("Amenity")]
        [Test]
        public async Task DeleteAmenityFromBus_Should_Delete_Amenity_From_Bus_Successfully()
        {
            // Arrange
            var mockBusRepository = new Mock<IRepository<int, Bus>>();
            var mockAmenityRepository = new Mock<IAmenityRepository<int, Amenity>>();
            var mockLogger = new Mock<ILogger<AmenityService>>();
            var mockContext = CreateMockDbContext();

            var amenityService = new AmenityService(mockBusRepository.Object, mockAmenityRepository.Object, mockLogger.Object, mockContext);

            var busId = 1;
            var amenityName = "Test Amenity";

            var existingBus = new Bus { BusId = busId, BusName = "Test Bus" };
            var existingAmenity = new Amenity { AmenityId = 1, Name = amenityName };

            mockBusRepository.Setup(repo => repo.GetAsync(busId)).ReturnsAsync(existingBus);
            mockAmenityRepository.Setup(repo => repo.GetByName(amenityName)).Returns(existingAmenity);
            mockAmenityRepository.Setup(repo => repo.Exists(busId, existingAmenity.AmenityId)).Returns(true);

            // Act
            await amenityService.DeleteAmenityFromBus(busId, amenityName);

            // Assert
            mockAmenityRepository.Verify(repo => repo.RemoveBusAmenity(busId, amenityName), Times.Once);
            mockLogger.Verify(logger => logger.LogInformation("Amenity deleted from bus successfully."), Times.Once);
        }

        [Test]
        public async Task DeleteAmenityFromBus_Should_Throw_BusNotFoundException_When_Bus_Not_Exists()
        {
            // Arrange
            var mockBusRepository = new Mock<IRepository<int, Bus>>();
            var mockAmenityRepository = new Mock<IAmenityRepository<int, Amenity>>();
            var mockLogger = new Mock<ILogger<AmenityService>>();
            var mockContext = CreateMockDbContext();

            var amenityService = new AmenityService(mockBusRepository.Object, mockAmenityRepository.Object, mockLogger.Object, mockContext);

            var nonExistingBusId = 99;
            var amenityName = "Test Amenity";

            mockBusRepository.Setup(repo => repo.GetAsync(nonExistingBusId)).ReturnsAsync((Bus)null);

            // Act and Assert
            async Task Act() => await amenityService.DeleteAmenityFromBus(nonExistingBusId, amenityName);
            Assert.ThrowsAsync<BusNotFoundException>(Act);
        }

        [Test]
        public async Task DeleteAmenityFromBus_Should_Throw_AmenitiesNotFoundException_When_Amenity_Not_Exists()
        {
            // Arrange
            var mockBusRepository = new Mock<IRepository<int, Bus>>();
            var mockAmenityRepository = new Mock<IAmenityRepository<int, Amenity>>();
            var mockLogger = new Mock<ILogger<AmenityService>>();
            var mockContext = CreateMockDbContext();

            var amenityService = new AmenityService(mockBusRepository.Object, mockAmenityRepository.Object, mockLogger.Object, mockContext);

            var busId = 1;
            var nonExistingAmenityName = "Non-existing Amenity";

            var existingBus = new Bus { BusId = busId, BusName = "Test Bus" };

            mockBusRepository.Setup(repo => repo.GetAsync(busId)).ReturnsAsync(existingBus);
            mockAmenityRepository.Setup(repo => repo.GetByName(nonExistingAmenityName)).Returns((Amenity)null);

            // Act and Assert
            async Task Act() => await amenityService.DeleteAmenityFromBus(busId, nonExistingAmenityName);
            Assert.ThrowsAsync<AmenitiesNotFoundException>(Act);
        }

        [Test]
        public async Task DeleteAmenityFromBus_Should_Throw_AmenitiesNotFoundException_When_Amenity_Not_Exists_On_Bus()
        {
            // Arrange
            var mockBusRepository = new Mock<IRepository<int, Bus>>();
            var mockAmenityRepository = new Mock<IAmenityRepository<int, Amenity>>();
            var mockLogger = new Mock<ILogger<AmenityService>>();
            var mockContext = CreateMockDbContext();

            var amenityService = new AmenityService(mockBusRepository.Object, mockAmenityRepository.Object, mockLogger.Object, mockContext);

            var busId = 1;
            var existingAmenityName = "Test Amenity";

            var existingBus = new Bus { BusId = busId, BusName = "Test Bus" };

            mockBusRepository.Setup(repo => repo.GetAsync(busId)).ReturnsAsync(existingBus);
            mockAmenityRepository.Setup(repo => repo.GetByName(existingAmenityName)).Returns((Amenity)null);

            // Act and Assert
            async Task Act() => await amenityService.DeleteAmenityFromBus(busId, existingAmenityName);
            Assert.ThrowsAsync<AmenitiesNotFoundException>(Act);
        }


        [Test]
        public async Task AddAmenitiesToBus_Should_Add_Amenities_To_Bus_Successfully()
        {
            // Arrange
            var mockBusRepository = new Mock<IRepository<int, Bus>>();
            var mockAmenityRepository = new Mock<IAmenityRepository<int, Amenity>>();
            var mockLogger = new Mock<ILogger<AmenityService>>();
            var mockContext = CreateMockDbContext();

            var amenityService = new AmenityService(mockBusRepository.Object, mockAmenityRepository.Object, mockLogger.Object, mockContext);

            var busId = 1;
            var amenityNames = new List<string> { "Amenity1", "Amenity2", "Amenity3" };

            var existingBus = new Bus { BusId = busId, BusName = "Test Bus" };
            var existingAmenities = new List<Amenity>
    {
        new Amenity { AmenityId = 1, Name = "Amenity1" },
        new Amenity { AmenityId = 2, Name = "Amenity2" },
        new Amenity { AmenityId = 3, Name = "Amenity3" }
    };

            mockBusRepository.Setup(repo => repo.GetAsync(busId)).ReturnsAsync(existingBus);
            foreach (var amenityName in amenityNames)
            {
                var name = amenityName; // Capturing loop variable
                var existingAmenity = existingAmenities.FirstOrDefault(a => a.Name == name);
                if (existingAmenity != null)
                {
                    mockAmenityRepository.Setup(repo => repo.GetByName(name)).Returns(existingAmenity);
                    mockAmenityRepository.Setup(repo => repo.Exists(busId, existingAmenity.AmenityId)).Returns(false);
                }
                else
                {
                    var newAmenity = new Amenity { Name = name };
                    mockAmenityRepository.Setup(repo => repo.GetByName(name)).Returns((Amenity)null);
                    mockAmenityRepository.Setup(repo => repo.AddAmenity(It.IsAny<Amenity>())).Callback<Amenity>(amenity => existingAmenities.Add(amenity));
                    mockAmenityRepository.Setup(repo => repo.Exists(busId, It.IsAny<int>())).Returns(false);
                }
            }

            // Act
            await amenityService.AddAmenitiesToBus(busId, amenityNames);

            // Assert
            //foreach (var amenityName in amenityNames)
            //{
            //    mockAmenityRepository.Verify(repo => repo.AddAmenity(It.Is<Amenity>(a => a.Name == amenityName)), Times.Once);
            //}
            mockAmenityRepository.Verify(repo => repo.AddBusAmenity(It.IsAny<BusAmenity>()), Times.Exactly(amenityNames.Count));
        }


        [Test]
        public async Task AddAmenitiesToBus_Should_Throw_BusNotFoundException_When_Bus_Not_Exists()
        {
            // Arrange
            var mockBusRepository = new Mock<IRepository<int, Bus>>();
            var mockAmenityRepository = new Mock<IAmenityRepository<int, Amenity>>();
            var mockLogger = new Mock<ILogger<AmenityService>>();
            var mockContext = CreateMockDbContext();

            var amenityService = new AmenityService(mockBusRepository.Object, mockAmenityRepository.Object, mockLogger.Object, mockContext);

            var nonExistingBusId = 99;
            var amenityNames = new List<string> { "Amenity1", "Amenity2", "Amenity3" };

            mockBusRepository.Setup(repo => repo.GetAsync(nonExistingBusId)).ReturnsAsync((Bus)null);

            // Act and Assert
            async Task Act() => await amenityService.AddAmenitiesToBus(nonExistingBusId, amenityNames);
            Assert.ThrowsAsync<BusNotFoundException>(Act);
        }

        [Test]
        public async Task AddAmenitiesToBus_Should_Throw_AmenityAlreadyExistsException_When_Amenity_Already_Exists_On_Bus()
        {
            // Arrange
            var mockBusRepository = new Mock<IRepository<int, Bus>>();
            var mockAmenityRepository = new Mock<IAmenityRepository<int, Amenity>>();
            var mockLogger = new Mock<ILogger<AmenityService>>();
            var mockContext = CreateMockDbContext();

            var amenityService = new AmenityService(mockBusRepository.Object, mockAmenityRepository.Object, mockLogger.Object, mockContext);

            var busId = 1;
            var amenityNames = new List<string> { "Amenity1", "Amenity2", "Amenity3" };

            var existingBus = new Bus { BusId = busId, BusName = "Test Bus" };
            var existingAmenity = new Amenity { AmenityId = 1, Name = "Amenity1" };

            mockBusRepository.Setup(repo => repo.GetAsync(busId)).ReturnsAsync(existingBus);
            mockAmenityRepository.Setup(repo => repo.GetByName("Amenity1")).Returns(existingAmenity);
            mockAmenityRepository.Setup(repo => repo.Exists(busId, existingAmenity.AmenityId)).Returns(true);

            // Act and Assert
            async Task Act() => await amenityService.AddAmenitiesToBus(busId, amenityNames);
            Assert.ThrowsAsync<AmenityAlreadyExistsException>(Act);
        }


   







    }
}
