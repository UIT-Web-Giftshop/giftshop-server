using System;

namespace Infrastructure.Extensions
{
    public static class GuidExtension
    {
        public static bool IsGuid(this string guid)
        {
            return Guid.TryParse(guid, out _);
        }
    }
}




