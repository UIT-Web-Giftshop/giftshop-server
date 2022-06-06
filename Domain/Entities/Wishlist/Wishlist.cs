using System.Collections.Generic;
using Domain.Attributes;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Entities.Wishlist
{
    [BsonCollection("wishlist")]
    public class Wishlist
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        
        public HashSet<WishlistItem> Items { get; set; }
    }
}