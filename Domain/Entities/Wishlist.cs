using System.Collections.Generic;
using Domain.Attributes;
using Domain.ViewModels.Wishlist;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Entities
{
    [BsonCollection("wishlists")]
    public class Wishlist
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        
        public HashSet<string> Items { get; set; }
        
        public IList<ProductInWishlist> Products { get; set; }

        public bool AddItem(string item)
        {
            Items ??= new HashSet<string>();
            return Items.Add(item);
        }
        
        public bool RemoveItem(string item)
        {
            return Items != null && Items.Remove(item);
        }
    }
}