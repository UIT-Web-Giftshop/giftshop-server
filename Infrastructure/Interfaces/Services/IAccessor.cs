using System.Collections.Generic;

namespace Infrastructure.Interfaces.Services
{
    public interface IAccessor
    {
        string Email();
        string Id();
        List<string> Roles();
        void AppendSession(string key, string value);
        string GetHeader(string key);
    }
}