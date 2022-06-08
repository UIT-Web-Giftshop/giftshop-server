using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Interfaces.Services
{
    public interface IAccessorService
    {
        string Email();
        string Id();
        List<string> Roles();
        void AppendSession(string key, string value);
        string GetHeader(string key);
        
        HttpContext GetHttpContext();
    }
}