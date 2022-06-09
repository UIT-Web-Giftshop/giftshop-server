using System;
using System.Collections.Generic;
using Domain.Attributes;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Entities.Order
{
    [BsonCollection("orders")]
    public class Order
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string UserEmail { get; set; }

        public HashSet<OrderItem> Items { get; set; }

        public double PromotionPercent { get; set; }

        public double TotalPrice { get; set; }
        
        public double TotalPaid { get; set; }

        public bool IsPaid { get; set; }

        public string Status { get; set; }
        
        public string Address { get; set; }

        public DateTime CreatedAt { get; set; }
        
        public DateTime CheckoutAt { get; set; }
        
        public void AddItem(OrderItem item)
        {
            Items ??= new HashSet<OrderItem>();
            Items.Add(item);
            TotalPrice += item.Sum();
        }
    }
}