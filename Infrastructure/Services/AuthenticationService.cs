using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Domain.Entities;
using Domain.Entities.Account;
using Domain.Models;
using Domain.Settings;
using Infrastructure.Interfaces.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;

namespace Infrastructure.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly AuthenticationSettings _authSettings;
        private readonly ILogger<AuthenticationService> _logger;

        public AuthenticationService(IOptions<AuthenticationSettings> authSettings, ILogger<AuthenticationService> logger)
        {
            _logger = logger;
            _authSettings = authSettings.Value;
        }

        public string GenerateAccessToken(User user)
        {
            var claims = new List<Claim> {
                new (ClaimTypes.Email, user.Email),
                new (ClaimTypes.Role, user.Role)
            };

            // credentials
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authSettings.SecretKey));
            var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            // describe token
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddMinutes(_authSettings.ExpirationMinutes),
                SigningCredentials = credentials,
            };
            
            // create token
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public RefreshTokenModel GenerateRefreshToken(string ipAddress)
        {
            using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            var randomBytes = new byte[64];
            rngCryptoServiceProvider.GetBytes(randomBytes);

            var refreshToken = new RefreshTokenModel()
            {
                IpAddress = ipAddress,
                Token = Convert.ToBase64String(randomBytes),
                ExpiredAt = DateTime.UtcNow.AddDays(7)
            };
            
            return refreshToken;
        }

        public List<Claim> ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

            if (jwtToken == null)
                return null;

            var symmetricKey = Encoding.UTF8.GetBytes(_authSettings.SecretKey);

            var validationParameters = new TokenValidationParameters()
            {
                RequireExpirationTime = true,
                ValidateIssuer = false,
                ValidateAudience = false,
                IssuerSigningKey = new SymmetricSecurityKey(symmetricKey)
            };

            var principal = tokenHandler.ValidateToken(token, validationParameters, out var securityToken);

            return principal.Claims.ToList();
        }
    }
}