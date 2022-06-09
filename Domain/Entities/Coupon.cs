using System;
using Domain.Attributes;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Entities
{
    [BsonCollection("coupons")]
    public class Coupon
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string EventCode { get; set; }
        
        public float DiscountPercent { get; set; }

        public bool IsActive { get; set; }

        public DateTime ValidFrom { get; set; }

        public DateTime ValidTo { get; set; }
        
        public DateTime CreatedAt { get; set; }

        public void Deactivate()
        {
            IsActive = false;
        }

        public bool IsValid()
            => IsActive && ValidFrom <= DateTime.UtcNow && ValidTo >= DateTime.UtcNow;
    }
}