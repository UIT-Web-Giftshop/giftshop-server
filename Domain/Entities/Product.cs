using System;
using System.Collections.Generic;
using Domain.Attributes;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Entities
{
    [BsonCollection("products")]
    public class Product
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonRequired]
        public string Sku { get; set; }
        [BsonRequired]
        public string Name { get; set; }
        public string Description { get; set; }
        public uint Quantity { get; set; }
        public double Price { get; set; }
        public List<string> Traits { get; set; }
        public string ImageUrl { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdateAt { get; set; }
    }
}