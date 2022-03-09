﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Domain.Paging;
using Infrastructure.Context;
using Infrastructure.Interfaces;
using MongoDB.Driver;

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

        public virtual async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            await _collection.InsertOneAsync(entity, cancellationToken: cancellationToken);
            return entity;
        }

        public virtual async Task<T> GetOneAsync(Expression<Func<T, bool>> expression, CancellationToken cancellationToken = default)
        {
            var filter = Builders<T>.Filter.Where(expression);
            var data = await _collection.Find(filter).FirstOrDefaultAsync(cancellationToken);
            return data;
        }

        public virtual async Task<IEnumerable<T>> GetPagingAsync(
            PagingRequest pagingRequest,
            Expression<Func<T, bool>> expression = null,
            string sortBy = null,
            bool sortAscending = true, 
            CancellationToken cancellationToken = default)
        {
            var filter = expression != null ? Builders<T>.Filter.Where(expression) : Builders<T>.Filter.Empty;
            var sortDefinition = sortAscending ? Builders<T>.Sort.Ascending(sortBy) : Builders<T>.Sort.Descending(sortBy);

            var dataList = await _collection
                .Find(filter)
                .Sort(sortDefinition)
                .Skip((pagingRequest.PageIndex - 1) * pagingRequest.PageSize)
                .Limit(pagingRequest.PageSize)
                .ToListAsync(cancellationToken);

            return dataList;
        }


        public virtual async Task<IEnumerable<T>> GetManyAsync(
            Expression<Func<T, bool>> expression = null, 
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, 
            CancellationToken cancellationToken = default)
        {
            var filter = Builders<T>.Filter.Empty;
            if (expression != null)
            {
                filter = Builders<T>.Filter.Where(expression);
            }

            var dataList = await _collection.Find(filter).ToListAsync(cancellationToken);
            return dataList;
        }

        public virtual async Task<bool> UpdateAsync(
            Expression<Func<T, bool>> expression,
            T entity, 
            CancellationToken cancellationToken = default)
        {
            //TODO use expression for filter
            var filter = Builders<T>.Filter.Where(expression);
            var affected = await _collection.ReplaceOneAsync(filter, entity, cancellationToken: cancellationToken);
            return affected.IsAcknowledged && affected.ModifiedCount > 0;
        }

        public virtual async Task<bool> PatchOneFieldAsync<TValue>(
            Expression<Func<T, bool>> expression, 
            Expression<Func<T, TValue>> fieldName, 
            TValue fieldValue,
            CancellationToken cancellationToken = default)
        {
            var filter = Builders<T>.Filter.Where(expression);
            var update = Builders<T>.Update.Set(fieldName, fieldValue);
            var options = new FindOneAndUpdateOptions<T>();
            var affected = await _collection.FindOneAndUpdateAsync(filter, update, options, cancellationToken);
            return affected != null;
        }

        public virtual async Task<bool> DeleteOneAsync(
            Expression<Func<T, bool>> expression,
            CancellationToken cancellationToken = default)
        {
            var filter = Builders<T>.Filter.Where(expression);
            var affected = await _collection.DeleteOneAsync(filter, cancellationToken);
            return affected.IsAcknowledged && affected.DeletedCount > 0;
        }
        
        public virtual async Task<bool> DeleteManyAsync<TValue>(
            Expression<Func<T, TValue>> expression,
            IEnumerable<TValue> values,
            CancellationToken cancellationToken = default)
        {
            var filter = Builders<T>.Filter.In(expression, values);
            var affected = await _collection.DeleteManyAsync(filter, cancellationToken);
            return affected.IsAcknowledged && affected.DeletedCount > 0;
        }
    }
}