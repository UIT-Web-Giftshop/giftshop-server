using System.Collections.Generic;

namespace Infrastructure.Interfaces.Services
{
    public interface IAccessor
    {
        string Email();
        string Id();
        List<string> Roles();
        bool IsInRole(string role);
    }
}