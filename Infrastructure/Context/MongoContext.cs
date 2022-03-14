using System;
using Domain.Attributes;
using Infrastructure.ConventionConfigure;
using Infrastructure.Persistence;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Infrastructure.Context
{
    public class MongoContext : IMongoContext
    {
        private IMongoDatabase MongoDatabase { get; set; }
        private IClientSessionHandle SessionHandle { get; set; }
        private MongoClient MongoClient { get; set; }
        
        private readonly IConfiguration _configuration;

        public MongoContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Dispose()
        {
            SessionHandle?.Dispose();
            GC.SuppressFinalize(this);
        }

        public IMongoDatabase GetContextDatabase()
        {
            return this.MongoDatabase;
        }

        public IMongoCollection<T> GetCollection<T>() where T : class
        {
            Configure();
            var collectionName = BsonCollection.GetCollectionName<T>();
            var collection = MongoDatabase.GetCollection<T>(collectionName);
            return collection;
        }

        private void Configure()
        {
            if (MongoClient != null)
            {
                return;
            }
            
            MongoConventionConfigure.Configure();
            MongoClient = new MongoClient(_configuration["MongoSettings:ConnectionString"]);
            MongoDatabase = MongoClient.GetDatabase(_configuration["MongoSettings:Database"]);
            
            // Seed data
            MongoDatabase.Seed();
        }
    }
}