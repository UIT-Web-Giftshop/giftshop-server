using MongoDB.Bson;

namespace Domain.Models
{
    public class RefreshTokenModel
    {
        public string UserId { get; set; }
        public string Token { get; set; }
        public BsonTimestamp ExpireDate { get; set; }
    }
}