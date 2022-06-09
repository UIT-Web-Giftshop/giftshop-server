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
        
        public string Sku { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int Stock { get; set; }

        public double Price { get; set; }

        public object Detail { get; set; }

        public List<string> Traits { get; set; }

        public string ImageUrl { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdateAt { get; set; }

        public void OnInit()
        {
            CreatedAt = DateTime.UtcNow;
            UpdateAt = DateTime.UtcNow;
        }
    }
}