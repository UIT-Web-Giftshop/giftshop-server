using System;
using Domain.Attributes;
using Infrastructure.ConventionConfigure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using MongoDB.Driver.Core.Events;

namespace Infrastructure.Context
{
    public class MongoContext : IMongoContext
    {
        private IMongoDatabase MongoDatabase { get; set; }
        private IClientSessionHandle SessionHandle { get; set; }
        private MongoClient MongoClient { get; set; }
        
        private readonly IConfiguration _configuration;
        
        private readonly ILogger<MongoContext> _logger;

        public MongoContext(IConfiguration configuration, ILogger<MongoContext> logger)
        {
            _configuration = configuration;
            _logger = logger;
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
            _logger.LogInformation("Init Mongo session");
            
            MongoConventionConfigure.Configure();
            var mongoClientSetting = ConfigureMongoClientSettings();
            MongoClient = new MongoClient(mongoClientSetting);
            MongoDatabase = MongoClient.GetDatabase(_configuration["MongoSettings:Database"]);
        }

        private MongoClientSettings ConfigureMongoClientSettings()
        {
            var mongoUrl = new MongoUrl(_configuration["MongoSettings:ConnectionString"]);
            var mongoClientSetting = MongoClientSettings.FromUrl(mongoUrl);
            mongoClientSetting.MaxConnectionIdleTime = TimeSpan.FromMinutes(5);
            mongoClientSetting.MaxConnectionLifeTime = TimeSpan.FromMinutes(10);
            
            // log query
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
            {
                mongoClientSetting.ClusterConfigurator = cb =>
                {
                    cb.Subscribe<CommandStartedEvent>(e =>
                        _logger.LogInformation("{0}", e.Command.ToString()));
                };
            }

            return mongoClientSetting;
        }
    }
}