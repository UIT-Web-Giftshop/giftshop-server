using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Infrastructure.Interfaces.Services;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services
{
    public class Accessor : IAccessor
    {
        private readonly HttpContextAccessor _contextAccessor;

        public Accessor(HttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public string Email()
        {
            return _contextAccessor.HttpContext?.User
                .FindFirstValue(ClaimTypes.Email);
        }

        public string Id()
        {
            return _contextAccessor.HttpContext?.User
                .FindFirstValue(ClaimTypes.NameIdentifier);
        }

        public List<string> Roles()
        {
            return _contextAccessor.HttpContext?.User
                .FindAll(q => q.Type == ClaimTypes.Role)
                .Select(q => q.Value)
                .ToList();
        }

        public bool IsInRole(string role)
        {
            return _contextAccessor.HttpContext?.User.IsInRole(role) ?? false;
        }
    }
}