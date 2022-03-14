using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Attributes;
using Domain.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Infrastructure.Persistence
{
    public static class SeedData
    {
        /// <summary>
        /// Seed data to database
        /// </summary>
        /// <param name="database"></param>
        public static void Seed(this IMongoDatabase database)
        {
            // check and seed some data for product collection
            var productCollection = database.GetCollection<Product>(BsonCollection.GetCollectionName<Product>());
            var existedData = productCollection
                .Find(q => true).Any();
            if (!existedData)
            {
                var products = InstantiateProducts();
                productCollection.InsertMany(products);
                var saveFlagName = BsonCollection.GetCollectionName<SaveFlag>();
                database.GetCollection<SaveFlag>(saveFlagName)
                    .InsertOne(new SaveFlag()
                    {
                        CollectionName = BsonCollection.GetCollectionName<Product>(), CurrentCount = products.Count()
                    });
            }
        }
        
        
        /// <summary>
        /// Init a list of products for seeding
        /// </summary>
        /// <returns></returns>
        private static IEnumerable<Product> InstantiateProducts()
        {
            return new List<Product>()
            {
                new Product()
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    Sku = "test01",
                    Name = "gift demo 01",
                    Description = "a initial gift demo product",
                    Stock = 10,
                    Price = 12.30,
                    Detail = new
                    {
                        Color = "red",
                        Size = "M"
                    },
                    Traits = new List<string>() { "gift", "demo", "friend" },
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                },
                new Product()
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    Sku = "test02",
                    Name = "gift demo 02",
                    Description = "the perfect gift for family",
                    Stock = 34,
                    Price = 25.50,
                    Detail = new
                    {
                        Meterial = "fabric",
                        Color = "blue",
                    },
                    Traits = new List<string>() { "gift", "demo", "family" },
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                },
                new Product()
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    Sku = "test03",
                    Name = "gift demo 03",
                    Description = "A sweet valentine gift for girlfriend",
                    Stock = 54,
                    Price = 50.25,
                    Traits = new List<string>() { "gift", "demo", "valentine", "girl" },
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                },
            };
        }
    }
}