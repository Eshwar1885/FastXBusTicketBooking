////namespace FastXAppTest
////{
////    public class AllUserServiceTest
////    {
////        [SetUp]
////        public void Setup()
////        {
////        }

////        [Test]
////        public void Test1()
////        {
////            Assert.Pass();
////        }
////    }
////}

//using NUnit.Framework;
//using Moq;
//using System.Threading.Tasks;
//using FastX.Interfaces;
//using FastX.Services;
//using FastX.Models.DTOs;
//using FastX.Models;
//using FastX.Exceptions;
//using Microsoft.Extensions.Logging;

//namespace FastX.Tests
//{
//    [TestFixture]
//    public class AllUserServiceTest
//    {
//        private IAllUserService _userService;
//        private Mock<IRepository<int, User>> _mockUserRepository;
//        private Mock<IRepository<int, Admin>> _mockAdminRepository;
//        private Mock<IRepository<int, BusOperator>> _mockBusOperatorRepository;
//        private Mock<IRepository<string, AllUser>> _mockAllUserRepository;
//        private Mock<ITokenService> _mockTokenService;
//        private Mock<ILogger<AllUserService>> _mockLogger;

//        [SetUp]
//        public void Setup()
//        {
//            _mockUserRepository = new Mock<IRepository<int, User>>();
//            _mockAdminRepository = new Mock<IRepository<int, Admin>>();
//            _mockBusOperatorRepository = new Mock<IRepository<int, BusOperator>>();
//            _mockAllUserRepository = new Mock<IRepository<string, AllUser>>();
//            _mockTokenService = new Mock<ITokenService>();
//            _mockLogger = new Mock<ILogger<AllUserService>>();

//            _userService = new AllUserService(
//                _mockUserRepository.Object,
//                _mockAdminRepository.Object,
//                _mockBusOperatorRepository.Object,
//                _mockAllUserRepository.Object,
//                _mockTokenService.Object,
//                _mockLogger.Object);
//        }

//        [Test]
//        public async Task Login_ValidUser_ReturnsLoginUserDTO()
//        {
//            // Arrange
//            var loginUserDTO = new LoginUserDTO
//            {
//                Username = "testuser",
//                Password = "password",
//            };
//            var allUser = new AllUser
//            {
//                Username = "testuser",
//                Password = new byte[] { 1, 2, 3 }, // Example password hash
//                Role = "user",
//                Key = new byte[] { 1, 2, 3 }, // Example key for password hashing
//            };

//            _mockAllUserRepository.Setup(repo => repo.GetAsync(loginUserDTO.Username)).ReturnsAsync(allUser);
//            _mockTokenService.Setup(service => service.GenerateToken(loginUserDTO)).ReturnsAsync("example_token");

//            // Act
//            var result = await _userService.Login(loginUserDTO);

//            // Assert
//            Assert.IsNotNull(result);
//            //Assert.AreEqual("testuser", result.Username);
//            //Assert.AreEqual("user", result.Role);
//            //Assert.AreEqual("example_token", result.Token);
//            //Assert.IsEmpty(result.Password); // Password should be cleared
//        }

//        [Test]
//        public void Login_InvalidUser_ThrowsInvalidUserException()
//        {
//            // Arrange
//            var loginUserDTO = new LoginUserDTO
//            {
//                Username = "nonexistentuser",
//                Password = "password",
//            };

//            _mockAllUserRepository.Setup(repo => repo.GetAsync(loginUserDTO.Username)).ReturnsAsync((AllUser)null);

//            // Act & Assert
//            Assert.ThrowsAsync<InvlidUserException>(() => _userService.Login(loginUserDTO));
//        }

//        // Add more test methods for Register method and other scenarios as needed
//    }
//}
//---------------------
using System.Threading.Tasks;
using NUnit.Framework;
using Moq;
using FastX.Services;
using FastX.Models.DTOs;
using FastX.Repositories;
using FastX.Models;
using FastX.Exceptions;
using Microsoft.Extensions.Logging;
using FastX.Interfaces;
using System.Text;
using System.Security.Cryptography;


namespace FastX.Tests
{
    [TestFixture]
    public class AllUserServiceTests
    {
        private AllUserService _allUserService;
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

            _allUserService = new AllUserService(
                _mockUserRepository.Object,
                _mockAdminRepository.Object,
                _mockBusOperatorRepository.Object,
                _mockAllUserRepository.Object,
                _mockTokenService.Object,
                _mockLogger.Object);
        }

        [Test]
        public async Task Register_NewUser_ReturnsLoginUserDTO()
        {
            // Arrange
            var registerUserDTO = new RegisterUserDTO
            {
                Username = "testuser",
                Password = "password",
                Role = "user",
                Name = "Test User",
                ContactNumber = "1234567890"
            };
            var registerBusOperatorDTO = new RegisterUserDTO
            {
                Username = "testbusoperator",
                Password = "password",
                Role = "busoperator",
                Name = "Test Bus Operator",
                ContactNumber = "1234567890"
            };


            HMACSHA512 hmac = new HMACSHA512();

            var allUser = new AllUser
            {

                Username = registerUserDTO.Username,
                Password = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerUserDTO.Password)),
                Key = hmac.Key,

            Role = registerUserDTO.Role
                // add other properties as needed
            };
            var busOperator = new AllUser
            {
                Username = registerUserDTO.Username,
                Password = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerUserDTO.Password)),
                Key = hmac.Key,

                Role = registerUserDTO.Role
                // add other properties as needed
            };
            // Arrange
            //var registerUserDTO = new RegisterUserDTO
            //{
            //    Username = "testuser",
            //    Password = Encoding.UTF8.GetBytes("password"), // Convert string password to byte array
            //    Role = "user",
            //    Name = "Test User",
            //    ContactNumber = "1234567890"
            //};

            _mockAllUserRepository.Setup(r => r.GetAsync(registerUserDTO.Username)).ReturnsAsync((AllUser)null);
            _mockAllUserRepository.Setup(r => r.Add(It.IsAny<AllUser>())).ReturnsAsync(allUser);

            // Act
            var result = await _allUserService.Register(registerUserDTO);

            // Assert
            Assert.IsNotNull(result);
            //Assert.AreEqual(registerUserDTO.Username, result.Username);
            //Assert.AreEqual(registerUserDTO.Role, result.Role);
            // Add more assertions as needed
        }

        [Test]
        public void Register_ExistingUser_ThrowsUserAlreadyExistsException()
        {
            // Arrange
            var registerUserDTO = new RegisterUserDTO
            {
                Username = "existinguser",
                Password = "password",
                Role = "user",
                Name = "Existing User",
                ContactNumber = "1234567890"
            };

            HMACSHA512 hmac = new HMACSHA512();

            var existingUser = new AllUser
            {

                Username = registerUserDTO.Username,
                Password = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerUserDTO.Password)),
                Key = hmac.Key,

                Role = registerUserDTO.Role
            };

            //var existingUser = new AllUser
            //{
            //    Username = registerUserDTO.Username,
            //    Password = registerUserDTO.Password,
            //    Role = registerUserDTO.Role
            //    // add other properties as needed
            //};

            _mockAllUserRepository.Setup(r => r.GetAsync(registerUserDTO.Username)).ReturnsAsync(existingUser);

            // Act & Assert
            Assert.ThrowsAsync<UserAlreadyExistsException>(async () => await _allUserService.Register(registerUserDTO));
        }
    }
}
