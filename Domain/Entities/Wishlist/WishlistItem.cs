using MongoDB.Bson;

namespace Domain.Entities.Wishlist
{
    public class WishlistItem
    {
        public ObjectId ProductId { get; set; }
    }
}