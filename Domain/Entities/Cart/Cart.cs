using System.Collections.Generic;
using Domain.Attributes;
using Domain.ViewModels.Cart;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Entities.Cart
{
    [BsonCollection("carts")]
    public class Cart
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        
        public HashSet<CartItem> Items { get; set; }
        
        public IList<ProductInCart> Products { get;set; }

        public bool AddItem(CartItem item)
        {
            Items ??= new HashSet<CartItem>();

            foreach (var i in Items)
            {
                if (i.ProductId != item.ProductId) continue;
                i.Quantity += item.Quantity;
                return true;
            }
            
            return Items.Add(item);
        }
    }
}