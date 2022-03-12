﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Domain.Entities;
using Domain.Settings;
using Infrastructure.Interfaces.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

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

        public string Authenticate(User user)
        {
            var claims = new List<Claim>();
            // add claim to subject list
            claims.Add(new Claim(ClaimTypes.Email, user.Email));
            //todo add roles

            // credentials
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authSettings.SecretKey));
            var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            // describe token
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddMinutes(_authSettings.ExpirationMinutes),
                SigningCredentials = credentials
            };
            
            // create token
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
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