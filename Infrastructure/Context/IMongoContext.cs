using System;
using MongoDB.Driver;

namespace Infrastructure.Context
{
    public interface IMongoContext : IDisposable
    {
        IMongoCollection<T> GetCollection<T>() where T : class;
    }
}