using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Infrastructure.Interfaces.Services;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services
{
    public class AccessorService : IAccessorService
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public AccessorService(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public string Email()
        {
            return _contextAccessor.HttpContext?.User
                .FindFirstValue(ClaimTypes.Email);
        }

        public string Role()
        {
            return _contextAccessor.HttpContext?.User
                .FindFirstValue(ClaimTypes.Role);
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

        public void AppendSession(string key, string value)
        {
            _contextAccessor.HttpContext?.Session.SetString(key, value);
        }

        public string GetHeader(string key)
        {
            return _contextAccessor.HttpContext?.Request.Headers[key].ToString();
        }

        public HttpContext GetHttpContext()
        {
            return _contextAccessor.HttpContext;
        }
    }
}