using System;
using System.Security.Claims;
using Domain.Entities;
using Domain.Models;
using Domain.Settings;
using Infrastructure.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace Web.Tests
{
    internal class MockIConfigurationAuth : Mock<IOptions<AuthenticationSettings>>
    {
        public MockIConfigurationAuth Setup()
        {
            SetupGet(x => x.Value).Returns(new AuthenticationSettings
            {
                SecretKey = "yxAxeBr9fm3sFu6UjWUprJhbPOFZwyy4",
                ExpirationMinutes = 1
            });
            return this;
        }
    }

    [Collection("Authentication")]
    public class AuthenticateTests
    {
        private readonly AuthenticationService _authService;
        private readonly string _email = "test@gm.com";
        private readonly User _user;

        public AuthenticateTests()
        {
            var mockOptions = new MockIConfigurationAuth().Setup();
            var mockLogger = Mock.Of<ILogger<AuthenticationService>>();
            _user = new User(){Email = _email};
            _authService = new AuthenticationService(mockOptions.Object, mockLogger);
        }

        [Fact]
        [Trait("Category", "Authentication")]
        public void GenerateAccessToken_GivenUser_ReturnTokenString()
        {
            // arrange

            // act
            var token = _authService.GenerateAccessToken(_user);
            
            // assert
            Assert.True(token.Length > 15);
            Assert.Contains('.', token);
        }

        [Fact]
        [Trait("Category", "Authentication")]
        public void ValidateToken_GivenToken_ReturnValidClaim()
        {
            // arrange
            var token = _authService.GenerateAccessToken(_user);
            
            // act
            var claim = _authService.ValidateToken(token);
            var email = claim.Find(q => q.Type == ClaimTypes.Email)?.Value;
            var exp = claim.Find(q => q.Type == "exp")?.Value;
            var nowStamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
            
            // assert
            Assert.Equal(_email, email);
            Assert.True(long.Parse(exp) > nowStamp);
        }
        
        [Fact]
        [Trait("Category", "Authentication")]
        public void ValidateToken_GivenExpireToken_ReturnInvalidClaim()
        {
            // arrange

            // act
            var token = _authService.GenerateAccessToken(_user);
            var claim = _authService.ValidateToken(token);
            var exp = claim.Find(q => q.Type == "exp")?.Value;
            var nowStamp = new DateTimeOffset(DateTime.UtcNow.AddMinutes(10)).ToUnixTimeSeconds();

            // assert
            Assert.False(long.Parse(exp) > nowStamp);
        }

        [Fact] 
        [Trait("Category", "Authentication")]
        public void GenerateRefreshToken_GivenUser_ReturnTokenString()
        {
            // arrange
            var ip = "127.0.0.1";

            // act
            var token = _authService.GenerateRefreshToken(ip);

            // assert
            Assert.IsType<RefreshTokenModel>(token);
            Assert.IsType<DateTime>(token.ExpiredAt);
            Assert.True(DateTime.UtcNow.AddDays(7) > token.ExpiredAt);
            Assert.NotNull(token.Token);
            Assert.Equal(ip, token.IpAddress);
        }
    }
}