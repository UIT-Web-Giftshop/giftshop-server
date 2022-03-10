using System;
using System.Collections.Generic;
using Domain.Attributes;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Entities
{
    [BsonCollection("orders")]
    public class Order
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string UserId { get; set; }
        public List<OrderItem> Items { get; set; }
        public OrderPromotion Promotion { get; set; }
        public double TotalPrice { get; set; }
        public bool IsPaid { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime SuccessAt { get; set; }
    }

    public class OrderItem
    {
        public string ProductId { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
    }

    public class OrderPromotion
    {
        public double Cash { get; set; }
        public double Percent { get; set; }
    }

    /// <summary>
    /// Call GetOrderStatus() to get the status name.
    /// See Web.Tests for example.
    /// </summary>
    public enum OrderStatus : int
    {
        Pending = 0,
        Delivered = 1,
        Success = 2,
        Canceled = 3,
    }

    public static class OrderStatusExtension
    {
        /// <summary>
        /// Get the OrderStatus name
        /// </summary>
        /// <param name="status">OrderStatus type</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static string GetOrderStatus(this OrderStatus status)
        {
            return status switch
            {
                OrderStatus.Pending => nameof(OrderStatus.Pending),
                OrderStatus.Delivered => nameof(OrderStatus.Delivered),
                OrderStatus.Success => nameof(OrderStatus.Success),
                OrderStatus.Canceled => nameof(OrderStatus.Canceled),
                _ => throw new ArgumentOutOfRangeException(nameof(status), status, null)
            };
        }
    }
}