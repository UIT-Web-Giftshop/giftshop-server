﻿using Domain.Entities;
using Infrastructure.Context;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Moq;
using Xunit;

namespace Web.Tests
{
    internal class MockIConfigurationMongo : Mock<IConfiguration>
    {
        public MockIConfigurationMongo MockSettings()
        {
            SetupGet(x => x[It.Is<string>(s => s == "MongoSettings:ConnectionString")])
                .Returns("mongodb://contextdb:27017");
            SetupGet(x => x[It.Is<string>(s => s == "MongoSettings:Database")])
                .Returns("giftshop-demo");
            return this;
        }
    }

    [Collection("MongoContext")]
    public class MongoContextTests
    {
        private Mock<IMongoDatabase> _mockDb;
        private Mock<IMongoClient> _mockClient;

        public MongoContextTests()
        {
            _mockDb = new Mock<IMongoDatabase>();
            _mockClient = new Mock<IMongoClient>();
        }

        [Fact]
        [Trait("Category", "Context")]
        public void MongoContext_GetCollection()
        {
            // arrange
            var mockConfigure = new MockIConfigurationMongo().MockSettings();
            var mockLogger = Mock.Of<ILogger<MongoContext>>();
            _mockClient
                .Setup(x => x.GetDatabase(mockConfigure.Object["MongoSettings:Database"], null))
                .Returns(_mockDb.Object);
            
            // act
            var context = new MongoContext(mockConfigure.Object, mockLogger);
            var collection = context.GetCollection<Product>();

            // assert
            Assert.NotNull(collection);
            Assert.Equal("products", collection.CollectionNamespace.CollectionName);
            Assert.Equal("giftshop-demo.products", collection.CollectionNamespace.FullName);
        }
    }
}