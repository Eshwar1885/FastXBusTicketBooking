//namespace FastXAppTest
//{
//    public class AllUserServiceTest
//    {
//        [SetUp]
//        public void Setup()
//        {
//        }

//        [Test]
//        public void Test1()
//        {
//            Assert.Pass();
//        }
//    }
//}

using NUnit.Framework;
using Moq;
using System.Threading.Tasks;
using FastX.Interfaces;
using FastX.Services;
using FastX.Models.DTOs;
using FastX.Models;
using FastX.Exceptions;
using Microsoft.Extensions.Logging;

namespace FastX.Tests
{
    [TestFixture]
    public class AllUserServiceTest
    {
        private IAllUserService _userService;
        private Mock<IRepository<int, User>> _mockUserRepository;
        private Mock<IRepository<int, Admin>> _mockAdminRepository;
        private Mock<IRepository<int, BusOperator>> _mockBusOperatorRepository;
        private Mock<IRepository<string, AllUser>> _mockAllUserRepository;
        private Mock<ITokenService> _mockTokenService;
        private Mock<ILogger<AllUserService>> _mockLogger;

        [SetUp]
        public void Setup()
        {
            _mockUserRepository = new Mock<IRepository<int, User>>();
            _mockAdminRepository = new Mock<IRepository<int, Admin>>();
            _mockBusOperatorRepository = new Mock<IRepository<int, BusOperator>>();
            _mockAllUserRepository = new Mock<IRepository<string, AllUser>>();
            _mockTokenService = new Mock<ITokenService>();
            _mockLogger = new Mock<ILogger<AllUserService>>();

            _userService = new AllUserService(
                _mockUserRepository.Object,
                _mockAdminRepository.Object,
                _mockBusOperatorRepository.Object,
                _mockAllUserRepository.Object,
                _mockTokenService.Object,
                _mockLogger.Object);
        }

        [Test]
        public async Task Login_ValidUser_ReturnsLoginUserDTO()
        {
            // Arrange
            var loginUserDTO = new LoginUserDTO
            {
                Username = "testuser",
                Password = "password",
            };
            var allUser = new AllUser
            {
                Username = "testuser",
                Password = new byte[] { 1, 2, 3 }, // Example password hash
                Role = "user",
                Key = new byte[] { 1, 2, 3 }, // Example key for password hashing
            };

            _mockAllUserRepository.Setup(repo => repo.GetAsync(loginUserDTO.Username)).ReturnsAsync(allUser);
            _mockTokenService.Setup(service => service.GenerateToken(loginUserDTO)).ReturnsAsync("example_token");

            // Act
            var result = await _userService.Login(loginUserDTO);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("testuser", result.Username);
            Assert.AreEqual("user", result.Role);
            Assert.AreEqual("example_token", result.Token);
            Assert.IsEmpty(result.Password); // Password should be cleared
        }

        [Test]
        public void Login_InvalidUser_ThrowsInvalidUserException()
        {
            // Arrange
            var loginUserDTO = new LoginUserDTO
            {
                Username = "nonexistentuser",
                Password = "password",
            };

            _mockAllUserRepository.Setup(repo => repo.GetAsync(loginUserDTO.Username)).ReturnsAsync((AllUser)null);

            // Act & Assert
            Assert.ThrowsAsync<InvlidUserException>(() => _userService.Login(loginUserDTO));
        }

        // Add more test methods for Register method and other scenarios as needed
    }
}
