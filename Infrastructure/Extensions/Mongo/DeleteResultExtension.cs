using MongoDB.Driver;

namespace Infrastructure.Extensions.Mongo
{
    public static class DeleteResultExtension
    {
        public static bool AnyDocumentDeleted(this DeleteResult result)
            => result.IsAcknowledged && result.DeletedCount > 0;
    }
}