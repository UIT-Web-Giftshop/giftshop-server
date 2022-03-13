using System;
using System.Collections.Generic;
using Domain.Attributes;

namespace Domain.Entities.Order
{
    [BsonCollection("orders")]
    public class Order : IdentifiableObject
    {
        public string UserId { get; set; }

        public List<OrderItem> Items { get; set; }

        public OrderPromotion Promotion { get; set; }

        public double TotalPrice { get; set; }

        public bool IsPaid { get; set; }

        public OrderStatus Status { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime SuccessAt { get; set; }
    }
}