using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Context;
using Infrastructure.Interfaces;
using MongoDB.Driver;
using ServiceStack;

namespace Infrastructure.Repositories
{
    public abstract class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly IMongoContext _context;
        private readonly IMongoCollection<T> _collection;

        protected BaseRepository(IMongoContext context)
        {
            _context = context;
            _collection = context.GetCollection<T>();
        }

        public void Dispose() => _context?.Dispose();

        public async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            await _collection.InsertOneAsync(entity, cancellationToken: cancellationToken);
            return entity;
        }

        public async Task<T> GetByIdAsync(string id)
        {
            var filter = Builders<T>.Filter.Eq("Id", id);
            var data = await _collection.Find(filter).FirstOrDefaultAsync();
            return data;
        }

        public async Task<IEnumerable<T>> GetManyAsync(Expression<Func<T, bool>> expression = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, CancellationToken cancellationToken = default)
        {
            var filter = Builders<T>.Filter.Empty;
            if (expression != null)
            {
                filter = Builders<T>.Filter.Where(expression);
            }

            var dataList = await _collection.Find(filter).ToListAsync(cancellationToken);
            var query = dataList.AsQueryable();
            if (orderBy != null)
            {
                query = orderBy(query);
            }

            return query;
        }

        public async Task<bool> UpdateAsync(T entity, CancellationToken cancellationToken = default)
        {
            var filter = Builders<T>.Filter.Eq("Id", entity.GetId());
            var affected = await _collection.ReplaceOneAsync(filter, entity, cancellationToken: cancellationToken);
            return affected.IsAcknowledged && affected.ModifiedCount > 0;
        }

        public async Task<bool> RemoveAsync(string id, CancellationToken cancellationToken = default)
        {
            var filter = Builders<T>.Filter.Eq("Id", id);
            var affected = await _collection.DeleteOneAsync(filter, cancellationToken);
            return affected.IsAcknowledged && affected.DeletedCount > 0;
        }

        public async Task<bool> RemoveManyAsync(Expression<Func<T, bool>> expression, CancellationToken cancellationToken = default)
        {
            var filter = Builders<T>.Filter.Where(expression);
            var affected = await _collection.DeleteManyAsync(filter, cancellationToken);
            return affected.IsAcknowledged && affected.DeletedCount > 0;
        }
    }
}