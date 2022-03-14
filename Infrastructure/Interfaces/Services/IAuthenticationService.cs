using System.Collections.Generic;
using System.Security.Claims;
using Domain.Entities;

namespace Infrastructure.Interfaces.Services
{
    public interface IAuthenticationService
    {
        string Authenticate(User user);
        List<Claim> ValidateToken(string token);
    }
}