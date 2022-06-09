using MongoDB.Driver;

namespace Infrastructure.Extensions.Mongo
{
    public static class UpdateResultExtension
    {
        public static bool AnyDocumentMatched(this UpdateResult result)
            => result.MatchedCount > 0;
        
        public static bool AnyDocumentModified(this UpdateResult result)
            => result.IsModifiedCountAvailable && result.ModifiedCount > 0;
    }
}