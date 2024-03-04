using FastX.Interfaces;
using FastX.Models.DTOs;
using FastX.Services;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastXAppTest
{
    internal class TokenServiceTest
    {
        [Test]
        public async Task GenerateToken()
        {
            // Arrange
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(e => e["SecretKey"]).Returns("This is a Dummy key created for authentication purpose");
            ITokenService tokenService = new TokenService(mockConfiguration.Object);


            var username = "abb";
             var   role = "user";

            // Act
            var token = await tokenService.GenerateToken(username,role);

            // Assert
            Assert.IsNotNull(token);
        }
    }
}