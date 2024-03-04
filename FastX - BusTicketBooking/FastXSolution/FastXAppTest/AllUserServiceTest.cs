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
            };
            var busOperator = new AllUser
            {
                Username = registerUserDTO.Username,
                Password = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerUserDTO.Password)),
                Key = hmac.Key,

                Role = registerUserDTO.Role
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



        //[Test]
        //public async Task LoginTest()
        //{
        //    // Arrange
        //    var loginUserDTO = new LoginUserInputDTO
        //    {
        //        Username = "test",
        //        Password = "abcd",
        //        //Role = "user"
        //    };

        //    var user = new AllUser
        //    {
        //        Username = "test",
        //        Password = Encoding.UTF8.GetBytes("abcd"), // Set the correct password here
        //        Key = Encoding.UTF8.GetBytes("This is a Dummy key created for authentication purpose"),
        //        Role = "user"
        //    };

        //    _mockAllUserRepository.Setup(repo => repo.GetAsync("test")).ReturnsAsync(user);
        //    _mockTokenService.Setup(tokenService => tokenService.GenerateToken(user.Username, user.Role)).ReturnsAsync("TestToken");

        //    // Act
        //    var result = await _allUserService.Login(loginUserDTO);

        //    // Assert
        //    //Assert.AreEqual("test", result.Username);
        //    //Assert.AreEqual("user", result.Role);
        //    //Assert.AreEqual("TestToken", result.Token);
        //    Assert.IsNotNull(result.UserId);
        //}

    }
}
