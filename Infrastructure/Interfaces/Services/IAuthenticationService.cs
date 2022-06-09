using System.Collections.Generic;
using System.Security.Claims;
using Domain.Entities;
using Domain.Entities.Account;
using Domain.Models;

namespace Infrastructure.Interfaces.Services
{
    public interface IAuthenticationService
    {
        /// <summary>
        /// Generate a JWT access token for user with email, role, expiration.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        string GenerateAccessToken(User user);
        
        /// <summary>
        /// Generate a refresh token for user which is valid for 7 days
        /// </summary>
        /// <returns></returns>
        RefreshTokenModel GenerateRefreshToken(string ipAddress);
        
        /// <summary>
        /// Parse access token and return claims list
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        List<Claim> ValidateToken(string token);
    }
}