using System;
using MongoDB.Driver;

namespace Infrastructure.Context
{
    public interface IMongoContext
    {
        IMongoDatabase GetContextDatabase();
        IMongoCollection<T> GetCollection<T>() where T : class;
    }
}