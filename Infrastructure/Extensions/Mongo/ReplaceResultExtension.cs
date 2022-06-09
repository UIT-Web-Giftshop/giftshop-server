using MongoDB.Driver;

namespace Infrastructure.Extensions.Mongo
{
    public static class ReplaceResultExtension
    {
        public static bool AnyDocumentReplaced(this ReplaceOneResult result)
         => result.IsModifiedCountAvailable && result.ModifiedCount > 0;
    }
}