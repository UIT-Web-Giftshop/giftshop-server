using MongoDB.Bson;

namespace Domain.Entities.Cart
{
    public class CartItem
    {
        public ObjectId ProductId { get; set; }
        public int Quantity { get; set; }
    }
}