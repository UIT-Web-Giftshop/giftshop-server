using System;
using System.Collections.Generic;
using Domain.Attributes;

namespace Domain.Entities
{
    [BsonCollection("products")]
    public class Product : IdentifiableObject
    {
        public string Sku { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public uint Stock { get; set; }

        public double Price { get; set; }

        public object Detail { get; set; }

        public List<string> Traits { get; set; }

        public string ImageUrl { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public override void Update()
        {
            this.UpdatedAt = DateTime.UtcNow;
        }
    }
}